using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularWebService.Auth.Contracts;
using ModularWebService.Auth.Model;
using ModularWebService.Modularity;

namespace ModularWebService.Auth.Handlers;

internal class RegisterHandler : IRequestHandler<RegisterRequest, UserDto>
{
    public RegisterHandler(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDto> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        await ThrowIfUsernameExists(request.Username, cancellationToken);

        Account account = new(request.Username, UserRole.User, request.Password);
        await _dbContext.Accounts.AddAsync(account, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        UserDto dto = new(account.Username, account.Role.ToString(), account.Created, null, null);
        return dto;
    }

    private async Task ThrowIfUsernameExists(string username, CancellationToken cancellationToken)
    {
        List<Account> accounts = await _dbContext.Accounts.ToListAsync(cancellationToken);
        bool isUsernameExists = accounts.Any(
            x => string.Compare(x.Username, username, StringComparison.OrdinalIgnoreCase) == 0);
        if (isUsernameExists)
        {
            throw new WebServiceException("Account already exists");
        }
    }

    private readonly AuthDbContext _dbContext;
}