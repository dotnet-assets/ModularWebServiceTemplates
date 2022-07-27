using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyPetProject.Auth.Contracts;
using MyPetProject.Auth.Model;
using MyPetProject.Modularity;

namespace MyPetProject.Auth.Handlers;

internal class RegisterHandler : IRequestHandler<RegisterRequest, UserDto>
{
    public RegisterHandler(AuthDbContext dbContext, ILogger<RegisterHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<UserDto> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        await ThrowIfUsernameExists(request.Username, cancellationToken);

        bool hasAnyAccount = await _dbContext.Accounts.AnyAsync(cancellationToken: cancellationToken);
        UserRole role = hasAnyAccount ? UserRole.User : UserRole.Admin;
        Account account = new(request.Username, role, request.Password);

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
            _logger.LogInformation($"Register failed: Username '{username}' already exists");
            throw new AppException("Account already exists");
        }
    }

    private readonly AuthDbContext _dbContext;
    private readonly ILogger<RegisterHandler> _logger;
}