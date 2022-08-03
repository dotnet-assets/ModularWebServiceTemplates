using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularWebService.ApiClient;
using ModularWebService.Auth.Data;
using ModularWebService.Auth.Model;

namespace ModularWebService.Tests;

internal class ApplicationFactory : WebApplicationFactory<Program>
{
    public ApplicationFactory()
    {
        _dbContainer = BuildDbContainer();
        _needInitDb = false;
        _username = null;
        _password = null;
    }

    public ApplicationFactory InitDb()
    {
        _needInitDb = true;
        return this;
    }

    public ApplicationFactory Login(string username, string password)
    {
        _username = username;
        _password = password;
        return this;
    }

    public IApiClient GetClient()
    {
        _dbContainer.StartAsync().GetAwaiter().GetResult();
        HttpClient http = CreateClient();

        if (_needInitDb)
        {
            InitDbImpl();
        }

        if (_username is not null && _password is not null)
        {
            return ApiClientFactory.CreateWithLogin(http, _username, _password).GetAwaiter().GetResult();
        }

        return ApiClientFactory.Create(http);
    }

    public override async ValueTask DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            RemoveDbContextDescriptor<AuthDbContext>(services);
            AddTestDbContext<AuthDbContext>(services);
        });
    }

    private PostgreSqlTestcontainer BuildDbContainer()
    {
        return new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "test_db",
                Username = "postgres",
                Password = "postgres",
            })
            .WithImage("postgres:11")
            .WithCleanUp(true)
            .Build();
    }

    private void InitDbImpl()
    {
        using IServiceScope scope = Services.CreateScope(); // _services.BuildServiceProvider().CreateScope();
        IServiceProvider scopedServices = scope.ServiceProvider;
        AuthDbContext dbContext = scopedServices.GetRequiredService<AuthDbContext>();

        dbContext.Accounts.Add(new Account("admin", UserRole.Admin, "123456"));
        dbContext.Accounts.Add(new Account("user", UserRole.User, "654321"));

        dbContext.SaveChanges();
    }

    private void RemoveDbContextDescriptor<T>(IServiceCollection services)
        where T : DbContext
    {
        ServiceDescriptor descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<T>));
        services.Remove(descriptor);
    }

    private void AddTestDbContext<T>(IServiceCollection services)
        where T : DbContext
    {
        services.AddDbContext<T>(options => { options.UseNpgsql(_dbContainer.ConnectionString); });
    }

    private readonly PostgreSqlTestcontainer _dbContainer;
    private bool _needInitDb;
    private string? _username;
    private string? _password;
}