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

https://learn.microsoft.com/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns

## Demo with local database

Make sure you have SQL Server 2025 install. The easiest way to do this is to use Docker or Podman and the VSCode MSSQL extension with a [local SQL Server container](https://learn.microsoft.com/sql/tools/visual-studio-code-extensions/mssql/mssql-local-container?view=sql-server-ver17).

IF you don't want to use a local database, you can also use an Azure SQL Database. You can use the *Free Offer* to have a [completely free Azure SQL database to use](https://learn.microsoft.com/azure/azure-sql/database/free-offer?view=azuresql).

Once you have the database running, create a new database named `dynamic-schema-ef` using the `Database/00-create.sql` script to also create a user for the sample application. 

The `Database/01-hybrid.sql` script is there only for reference in case you want to create the database schema manually, but it is not needed if you use the Entity Framework Core migrations.

Generate migrations of not done yet

```powershell
dotnet ef migrations add InitialCreate
```

Set the environment variable

```powershell
$env:MSSQL="Server=tcp:127.0.0.1,1433;Database=dynamic-schema-ef;User ID=<db-admin-user>;Password=<db-admin-password>;TrustServerCertificate=True"
```

Deploy the database

```powershell
dotnet ef database update
```

# Run the sample app

Run in watch mode

```
dotnet watch
```

then use the `Sample/sample.http` file to test out the API.

Uncomment the `ToDoExtension` properties in the `Entities/ToDo.cs` file and the properties in the `Controllers/ToDoHybridController.cs` file to add more filed to your entity.

The new properties will be automatically serialized and deserialized by EF Core without the need to change the database schema, and you can use them in your application without any additional code.