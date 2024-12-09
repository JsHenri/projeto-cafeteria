## Documentação do Projeto Cafeteria

### Introdução

Este documento tem como objetivo fornecer uma visão geral completa do projeto Cafeteria, incluindo sua arquitetura, funcionalidades e instruções de instalação e uso. O projeto foi desenvolvido utilizando a plataforma .NET e tem como finalidade gerenciar as operações de uma cafeteria, desde o cadastro de produtos até a gestão de pedidos.

### Arquitetura

O projeto Cafeteria segue a arquitetura MVC (Model-View-Controller), o que permite uma clara separação de responsabilidades entre os componentes:

* **Model:** Representa a camada de dados, definindo as classes que mapeiam as entidades do sistema (produtos, pedidos, clientes, etc.).
* **View:** Responsável pela interface do usuário, exibindo as informações e permitindo a interação com o sistema.
* **Controller:** Atua como intermediário entre a view e o model, processando as requisições do usuário e atualizando o modelo de dados.

### Funcionalidades

* **Cadastro e gestão de produtos:** Permite adicionar, editar e remover produtos do cardápio, incluindo informações como nome, preço e descrição.
* **Gerenciamento de pedidos:** Facilita a criação, edição e consulta de pedidos, permitindo adicionar itens e calcular o valor total.
* **Relatórios:** Gera relatórios personalizados sobre vendas.
* **Controle de estoque:** Permite acompanhar a quantidade de cada produto em estoque.

### Tecnologias Utilizadas

* **.NET 6.0:** Framework de desenvolvimento da Microsoft utilizado para construir a aplicação.
* **C#:** Linguagem de programação principal do projeto.
* **Entity Framework Core:** ORM (Object-Relational Mapper) utilizado para interagir com o banco de dados.
* **SQL Lite:** Banco de dados relacional utilizado para armazenar as informações do sistema.

### Pré-requisitos

Para executar o projeto, é necessário ter os seguintes softwares instalados em sua máquina:

* **.NET SDK 6.0 ou superior:** [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
* **Visual Studio 2022** ou outro editor de código compatível com .NET.
* **SQL Server** ou outro banco de dados compatível com Entity Framework Core.
* **Git:** Para clonar o repositório do projeto.

### Instalação e Execução

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/JsHenri/projeto-cafeteria.git
   cd Cafeteria
   ```
2. **Restaurar os pacotes:**
   ```bash
   dotnet restore
   ```
3. **Configurar a string de conexão:**
   Edite o arquivo `appsettings.json` e configure a string de conexão com o seu banco de dados.
4. **Aplicar as migrações:**
   ```bash
   dotnet ef database update
   ```
5. **Executar o projeto:**
   ```bash
   dotnet run
   ```

### Estrutura do Projeto

* **Controllers:** Contém os controladores que manipulam as requisições HTTP e respondem ao usuário.
* **Models:** Define as classes que representam os dados do domínio (produtos, pedidos, etc.).
* **Views:** Contém as páginas HTML que compõem a interface do usuário.
* **Migrations:** Contém os scripts utilizados para criar e atualizar o banco de dados.

### Considerações Finais

Este documento apresenta uma visão geral do projeto Cafeteria. Para obter informações mais detalhadas sobre cada componente do sistema, consulte o código fonte e a documentação oficial das tecnologias utilizadas.
