# MyPetProject

## Migrations

```
dotnet ef migrations add InitialCreate --context AuthDbContext -p Modules/Auth/MyPetProject.Auth/MyPetProject.Auth.csproj -s MyPetProject.WebApi/MyPetProject.WebApi.csproj -o Data/Migrations
dotnet ef database update --context AuthDbContext -p MyPetProject.WebApi/MyPetProject.WebApi.csproj
```