using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Data;
using Cafeteria.Models;

namespace Cafeteria.Controllers
{
    public class OrderController : Controller
    {
        private readonly CafeteriaContext _context;

        public OrderController(CafeteriaContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {   
            // Recupera todos os produtos do BD
            var products = await _context.Product.ToListAsync();
            // Seleciona apenas os produtos cujo atributo Quantidade é diferente de zero
            products = products.Where(p => p.Quantity!=0).ToList();

        		// Instancia uma nova ViewModel
            OrderCreateViewModel viewModel = new OrderCreateViewModel
            {
                ProductsSelectList = products.Select(p => new SelectListItem 
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }),
                Products = products
            };

        		// Limpa os dados temporários
            TempData.Clear();

            return View(viewModel);
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel viewModel)
        {
            // Se o usuário não selecionou nenhum produto e clicou no botão "Finalize Order"
            // então será redirecionado para Index e nada acontece
            if(TempData.IsNullOrEmpty())
            {
                return RedirectToAction(nameof(Index));
            }

            // Instancia um novo pedido
            Order order = new Order
            {
                TimeStamp = DateTime.Now,
                TotalPrice = viewModel.TotalPrice
            };

            // Persiste o pedido no Banco de dados
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // Recupera o pedido que acabou de ser salvo para ter acesso ao seu ID
            Order ?o = await _context.Order.FindAsync(order.Id);

            // Retorna um erro caso a consulta ao BD falhe
            if(o==null)
                return BadRequest();

            // Cria uma lista de OrderItem
            List<OrderItem>orderItems = new List<OrderItem>();

            // Instancia um OrderItem para cada elemento salvo em TempData
            foreach(string key in TempData.Keys)
            {
            
                string? stringValue = TempData[key]?.ToString();

                if(stringValue==null)
                    return BadRequest();

                OrderItem orderItem = new OrderItem           
                {
                    IdOrder = o.Id,
                    IdProduct = int.Parse(key),
                    Quantity = int.Parse(stringValue)
                };

                // Adiociona o OrderItem criado ao DbContext
                _context.OrderItem.Add(orderItem);
                orderItems.Add(orderItem);

                // Qtualiza a quantidade do produto consumido no pedido
                Product? productUpdate = await _context.Product.FindAsync(int.Parse(key));

                if(productUpdate==null)
                    return NotFound();

                productUpdate.Quantity -= int.Parse(stringValue);

                _context.Product.Update(productUpdate);
            }

            // Persiste todas as alterações realizadas no banco de dados
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct (OrderCreateViewModel viewModel)
        { 
            // Recupera todos os produtos do BD
            var products = await _context.Product.ToListAsync();
            // Seleciona apenas os produtos cujo atributo Quantidade é diferente de zero
            products = products.Where(p => p.Quantity!=0).ToList();

            // Valida se a consulta ao banco teve sucesso
            if(products==null)
                return NotFound();

            // Valida se o produto selecionado pelo usuário no dropdown foi encontrado no BD
            Product? stockCheck = products.Find(p=>p.Id==viewModel.SelectedProductId);
            if(stockCheck==null)
                return NotFound();

            // Verifica se a quantidade solicitada no produto e suficiente no estoque
            if(stockCheck.Quantity >= viewModel.Quantity)
            {
                TempData[viewModel.SelectedProductId.ToString()]=viewModel.Quantity;
            }else
            {
                viewModel.Message = "There are not enough products in stock.";
            }

            viewModel.TotalPrice = 0;

            // Salva os dados do produto selecionado em TempData para a próxima seção
            foreach (var key in TempData.Keys)
            {
                TempData.Keep(key);

                Product ?p = products.Find(p => p.Id == int.Parse(key));
                string ?q = TempData[key]?.ToString();
                if (p != null && q!=null)
                    viewModel.TotalPrice += p.Price * int.Parse(q);
            }

            // Reabastece a lista de produtos para o dropdown
            viewModel.ProductsSelectList = products.Select(p => new SelectListItem 
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                });
            viewModel.Products = products;

            // Retorna a ViewModel para View/Order/Create
            return View("Create",viewModel);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TimeStamp,TotalPrice")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
