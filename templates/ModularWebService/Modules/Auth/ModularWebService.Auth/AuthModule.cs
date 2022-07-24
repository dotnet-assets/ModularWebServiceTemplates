using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
}