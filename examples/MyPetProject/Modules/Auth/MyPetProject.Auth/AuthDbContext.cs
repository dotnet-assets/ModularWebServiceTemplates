using Microsoft.EntityFrameworkCore;
using MyPetProject.Auth.Model;

namespace MyPetProject.Auth;

internal sealed class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
}