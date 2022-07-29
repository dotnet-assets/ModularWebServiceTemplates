using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularWebService.WebServiceModule;

public static class WebServiceModule
{
    public static IServiceCollection AddWebServiceModuleModule(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<WebServiceModuleDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("WebServiceModule")));

        serviceCollection.AddMediatR(typeof(WebServiceModule));
        return serviceCollection;
    }
}