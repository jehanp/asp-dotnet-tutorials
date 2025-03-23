
# Main Tutorial
https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio-code

# Setting SQL Server 
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer
- Add following to Program.cs
```c#
// Update connection string to use SQL Server
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Server=localhost;Database=PizzaStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

// Register the DbContext with SQL Server provider
builder.Services.AddDbContext<PizzaDb>(options => options.UseSqlServer(connectionString));
```
- Add following to appsettings.json
```json
{
    "ConnectionStrings": {
        "Pizzas": "Server=localhost;Database=PizzaStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
    }
}
```
- Apply migrations
```zsh
dotnet ef migrations add InitialCreate
dotnet ef database update
```
- Re-applying migration:
```zsh
dotnet ef database drop
dotnet ef database update
```
- Reset migration
```zsh
dotnet ef database drop
dotnet ef migrations add InitialCreate
dotnet ef database update
```