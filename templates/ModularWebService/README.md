# ModularWebService

## Migrations

```
dotnet ef migrations add InitialCreate --context AuthDbContext -p Modules/Auth/ModularWebService.Auth/ModularWebService.Auth.csproj -s ModularWebService.WebApi/ModularWebService.WebApi.csproj -o Data/Migrations
dotnet ef database update --context AuthDbContext -p ModularWebService.WebApi/ModularWebService.WebApi.csproj
```