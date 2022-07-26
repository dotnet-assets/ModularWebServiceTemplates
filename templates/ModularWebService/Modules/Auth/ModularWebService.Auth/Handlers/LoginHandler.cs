using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModularWebService.Auth.Contracts;
using ModularWebService.Auth.Data;
using ModularWebService.Auth.Model;
using ModularWebService.Modularity;

namespace ModularWebService.Auth.Handlers;

internal class LoginHandler : IRequestHandler<LoginRequest, UserDto>
{
    public LoginHandler(AuthDbContext dbContext, IConfiguration configuration, ILogger<LoginHandler> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<UserDto> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        Account account = await AuthenticateUser(request.Username, request.Password);
        JwtSecurityToken token = CreateJwtToken(
            account.Username,
            account.Role.ToString(),
            _configuration["Jwt:Key"],
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            TimeSpan.FromMinutes(int.Parse(_configuration["Jwt:TokenTimeoutMinutes"])));

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        UserDto dto = new(account.Username, account.Role.ToString(), account.Created, tokenString, token.ValidTo);
        return dto;
    }

    private async Task<Account> AuthenticateUser(string username, string password)
    {
        Account? account = await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Username == username);
        if (account is null)
        {
            _logger.LogInformation($"Login failed: Account with username '{username}' not found");
            throw new AppException("Login failed");
        }

        if (!account.CheckPassword(password))
        {
            _logger.LogInformation($"Login failed: Invalid password for account '{username}'");
            throw new AppException("Login failed");
        }

        return account;
    }

    private JwtSecurityToken CreateJwtToken(string username, string role, string signingKey, string issuer,
        string audience, TimeSpan expiration)
    {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role),
        };

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(signingKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.UtcNow.Add(expiration),
            claims: claims,
            signingCredentials: credentials);
    }

    private readonly AuthDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginHandler> _logger;
}