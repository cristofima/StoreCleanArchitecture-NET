# Store Clean Architecture
Clean Architecture's implementation in 3 layers (Application Core, Infrastructure and Web API)

To build this project
```
dotnet restore
dotnet build
```

Configure the database settings in src\Store.WebApi\appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DemoStore;User ID=sa;Password=coronadoserver2018;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

To create the tables in the database, go to src\Store.WebApi
```
dotnet ef database update
```

To run, go to src\Store.WebApi
```
dotnet run
```

To test Unit and Integration projects, go to solution folder
```
dotnet test
```
