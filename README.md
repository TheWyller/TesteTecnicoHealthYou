# Teste Tecnico HealthYou

O teste consiste em realizar uma Minimal API em C# consumindo um Banco de dados SQL. 

## Features:

- Criação de usuário

- Listagem de usuários

- Listagem de um usuário

- Atualização de um usuário

- Deleção de um usuário

## Como Rodar a Aplicação

- Clonar o Git em sua máquina local

- Abrir arquivo TestTecnico.sln no Visual Studio

- Modificar o arquivo "appsettings.json" com sua string de conexão do banco SQL local, com as caracteristicas abaixo:

```
"ConnectionStrings": {
    "DefaultConnection": "Server={......\\.....};Database=TesteTecnicoSQL;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;User ID={.....};Password={.....}"
  },

```

- Rodar as migrations no Package Manager Console (Console do Gerenciador de Pacotes), com o seguinte código

```
  update-database
```
- Pronto você pode rodar a aplicação

## Documentação:

- Pode ser encontrada após rodar no seguinte endpoint: /swagger/index.html




