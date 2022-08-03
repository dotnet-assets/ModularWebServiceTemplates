using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularWebService.Auth.Data;

namespace ModularWebService.Auth;

public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Auth")));

        serviceCollection.AddMediatR(typeof(AuthModule));
        return serviceCollection;
    }

    public static IApplicationBuilder UseAuthModule(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        AuthDbContext dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        dbContext.Database.Migrate();

        return app;
    }
}