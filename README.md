# API RESTful para um Site de Filmes

Projeto de uma API RESTful desenvolvida utilizando ASP.NET e Entity Framework para um site de filmes. A API permite o gerenciamento de usuários, filmes vistos e lista de Watchlist. O banco de dados é hospedado em uma instância AWS com MySQL e o deploy está feito em ambiente dockerizado.

[Documentação com Swagger](https://asp-net-api-filmes.onrender.com/swagger/index.html)
![image](https://github.com/moutim/ASP.NET-API-Filmes/assets/88093439/57520313-2814-46e3-827c-82d455886d8c)


## Endpoints

### Cadastro

- **POST** `/api/Cadastro`: Cria um novo usuário.

### Login

- **POST** `/api/Login`: Realiza o login do usuário e retorna um token JWT para autenticação.

### Usuário

- **GET** `/api/Usuario/Infos/{userId}`: Retorna informações do usuário especificado por `userId`.
- **GET** `/api/Usuario/WatchList/{userId}`: Retorna a lista de filmes na watchlist do usuário especificado por `userId`.
- **GET** `/api/Usuario/Vistos/{userId}`: Retorna a lista de filmes vistos pelo usuário especificado por `userId`.
- **DELETE** `/api/Usuario/Deletar/{userId}`: Deleta o usuário especificado por `userId`.
- **PATCH** `/api/Usuario/Atualizar/{userId}`: Atualiza informações do usuário especificado por `userId`.

### Visto

- **POST** `/api/Visto/Adicionar/{userId}`: Adiciona um filme como visto para o usuário especificado por `userId`.
- **DELETE** `/api/Visto/Deletar/{userId}/{movieId}`: Remove um filme da lista de vistos do usuário especificado por `userId`.

### Watchlist

- **POST** `/api/Watchlist/Adicionar/{userId}`: Adiciona um filme à watchlist do usuário especificado por `userId`.
- **DELETE** `/api/Watchlist/Deletar/{userId}/{movieId}`: Remove um filme da watchlist do usuário especificado por `userId`.

## Tecnologias Utilizadas

- ASP.NET: Framework para construção de aplicativos web.
- Entity Framework: ORM para mapeamento objeto-relacional.
- AWS (Amazon Web Services): Plataforma de serviços em nuvem para hospedagem do banco de dados MySQL.
- Docker: Plataforma de contêineres para facilitar a implantação e execução do aplicativo.

## Configuração e Execução com Docker Compose

1. Certifique-se de ter o Docker e o Docker Compose instalados em seu ambiente.
2. Clone o repositório do projeto.
3. No diretório raiz do projeto, crie um arquivo chamado `.env` e defina as variáveis de ambiente necessárias, como as credenciais da AWS e as configurações do banco de dados MySQL.
4. Execute o comando `docker-compose up --build` para construir as imagens do Docker e iniciar os contêineres.
5. A API estará disponível em `http://localhost:8080`.
