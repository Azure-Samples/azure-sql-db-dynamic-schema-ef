---
page_type: sample
languages:
- tsql
- sql
- csharp
products:
- azure-sql-database
- dotnet
- ef-core
- sql-server
description: "Dynamic Schema Management With Azure SQL and Entity Framework "
---

# Dynamic Schema Management With Azure SQL and Entity Framework 

![License](https://img.shields.io/badge/license-MIT-green.svg)

A sample project that shows how to deal with dynamic schema in Azure SQL, using the native JSON support and Entity Framework Core. This repo is a variation of the "hybrid" sample discussed and shown in the 

https://github.com/azure-samples/azure-sql-db-dynamic-schema

repository, but using Entity Framework Core instead of Dapper. Since EF Core 7, in fact, it is now possible to let the framework handle the serialization and deserialization of an object into a JSON column, making the code much cleaner and easier to maintain:

https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns

