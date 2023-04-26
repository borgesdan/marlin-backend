﻿# MARLIN API

API que tem por objetivo gerenciar uma empresa que oferece cursos de idiomas e que possui turmas de nível básico, intermediário e avançado.
Como proposto para o desafio de vaga para Desenvolvedor .NET C# para a empresa Marlin.


### 📋 Objetivo

API para teste prático requisitado pela empresa Marlin. Desenvolvido em C# com acesso a um banco de dados SQL Server, em ambiente .NET 7, estrutura MVC/DDD, com Code First Mapping para criação da base de dados com Migrations.

A API oferece CRUD de alunos e turmas e segue as regras solicitadas:

* Aluno deve ser cadastrado com no mínimo 1 turma;
* O e-mail e CPF do aluno não pode ser inválido;
* Aluno pode se matricular em diversas turmas, mas não mais de 1x na mesma turma;
* Uma turma não pode ter mais de 5 alunos;
* Aluno não pode ser excluído se estiver associado em uma turma;
* Turma não pode ser excluída se possuir alunos;

## 🛠️ Construído com

* [Visual Studio 2022](https://visualstudio.microsoft.com/) - IDE
* [.NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) - Ambiente de desenvolvimento
* [SQL Server 2019](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) - Banco de dados relacional
* [Entity Framework](https://learn.microsoft.com/pt-br/ef/) - Biblioteca ORM para gerenciamento do acesso ao banco de dados.

## ✒️ Autor

* **Danilo Borges Santos** 
* [Email](mailto:borges_santos89@hotmail.com)
* [Whatsapp](https://api.whatsapp.com/send?phone=5573999403798)
* [Site](https://www.dandev.com.br)