using Microsoft.EntityFrameworkCore;
using MyPetProject.SomeModule.Model;

namespace MyPetProject.SomeModule;

internal class SomeModuleDbContext : DbContext
{
    public SomeModuleDbContext(DbContextOptions<SomeModuleDbContext> options)
        : base(options)
    {
    }

    public DbSet<Entity> Entities => Set<Entity>();
}