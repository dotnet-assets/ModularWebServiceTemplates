using Microsoft.EntityFrameworkCore;
using ModularWebService.Auth.Model;

namespace ModularWebService.Auth.Data;

internal sealed class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
}