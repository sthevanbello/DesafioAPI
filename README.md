# Desafio módulo de API
## Desafio Edusync/BRQ -  Módulo de construção de API

### Objetivo
#### Construção de uma API REST utilizando Asp.Net Core e ADO.Net sem utilizar template

### O que foi utilizado?
 - Linguagem C#
 - Asp.Net Core API 
 - Conceito de inversão de controle com injeção de dependência do repositório das models
 - Utilizado o GetConnectionString de IConfiguration para recuperar o endereço da connection string do banco de dados
    - Isso foi feito para isolar no arquivo appsettings.json a string de conexão com o banco de dados
 - ADO.Net para acesso aos dados contidos em um banco de dados
 - SQL Server 
    - Criação de tabelas com script DDL
    - Inserção de dados com Script DML
    - Consultas dos dados com Script DQL
    - Consultas utilizando relacionamentos entre as tabelas
 - Criação de toda a estrutura de acesso a dados utilizando Repository Pattern
 - Documentação via Swagger
 
 ### Configurações necessárias
```
  - Executar os scripts T-SQL contidos na pasta Scripts que consta no projeto
    - Executar na seguinte ordem: 
      1º DDL
      2º DML
  - Trocar a string de conexão no arquivo appsettings.json
```
>No arquivo  appsettings.json substituir a string de conexão
```
"ConnectionStrings": {
    "ForumGames": "  colocar aqui a string completa de conexão com o banco de dados  "
  }
```
