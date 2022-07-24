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
using ModularWebService.Auth;
using ModularWebService.Auth.Model;

namespace ModularWebService.Tests;

internal class ApplicationFactory : WebApplicationFactory<Program>
{
    public static async Task<IApiClient> CreateApiClient()
    {
        ApplicationFactory applicationFactory = new();
        return await applicationFactory.StartAndCreateApiClient();
    }

    public static async Task<IApiClient> CreateApiClientAndLogin(string username, string password)
    {
        ApplicationFactory applicationFactory = new();
        return await applicationFactory.StartAndCreateApiClient(username, password);
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
            EnsureDbCreated(services);
            InitDb(services);
        });
    }

    private ApplicationFactory()
    {
        _dbContainer = BuildDbContainer();
    }

    private async Task<IApiClient> StartAndCreateApiClient(string? username = null, string? password = null)
    {
        await _dbContainer.StartAsync();

        HttpClient http = CreateClient();

        IApiClient apiClient;
        if (username != null && password != null)
        {
            apiClient = await ApiClientFactory.CreateWithLogin(http, username, password);
        }
        else
        {
            apiClient = ApiClientFactory.Create(http);
        }

        return apiClient;
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

    private void EnsureDbCreated(IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        IServiceProvider scopedServices = scope.ServiceProvider;
        AuthDbContext dbContext = scopedServices.GetRequiredService<AuthDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    private void InitDb(IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();
        IServiceProvider scopedServices = scope.ServiceProvider;
        AuthDbContext dbContext = scopedServices.GetRequiredService<AuthDbContext>();

        dbContext.Accounts.Add(new Account("admin", UserRole.Admin, "123456"));
        dbContext.Accounts.Add(new Account("user", UserRole.User, "654321"));

        dbContext.SaveChanges();
    }

    private readonly PostgreSqlTestcontainer _dbContainer;
}