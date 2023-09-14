# Betatalks Testcontainers
This application constitutes a sample of a project containing integration tests using the [Testcontainers for .NET](https://dotnet.testcontainers.org/) NuGet package.
It consists of a simple ASP.NET Core together with an integration test project.

## Getting Started
To run this application locally, you require the following software
- [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [SQL Server with SQL Server Express LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver16).
Note that instead another provider for a SQL database can be used.

## Running the ASP.NET Core API
The connection string for the local database is specified within `src/Betatalks.Testcontainers.Api/appsettings.json`.
By default this points to the LocalDB `BetatalksTestcontainers`.
If you want to use a different database, make sure to change this connection string.
The API can then be started using the following command: `dotnet run --project src/Betatalks.Testcontainers.Api`

## Running integration tests
[Docker](https://www.docker.com/) is required to be installed on the machine prior to running the integration tests.
The integration tests can be ran through `dotnet test`
