using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyPetProject.SomeModule;

public static class SomeModule
{
    public static IServiceCollection AddSomeModule(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<SomeModuleDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SomeModule")));

        serviceCollection.AddMediatR(typeof(SomeModule));
        return serviceCollection;
    }
}