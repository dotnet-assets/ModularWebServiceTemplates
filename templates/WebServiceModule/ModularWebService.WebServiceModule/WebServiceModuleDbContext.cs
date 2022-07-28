using Microsoft.EntityFrameworkCore;
using ModularWebService.WebServiceModule.Model;

namespace ModularWebService.WebServiceModule;

internal class WebServiceModuleDbContext : DbContext
{
    public WebServiceModuleDbContext(DbContextOptions<WebServiceModuleDbContext> options)
        : base(options)
    {
    }

    public DbSet<Entity> Entities => Set<Entity>();
}