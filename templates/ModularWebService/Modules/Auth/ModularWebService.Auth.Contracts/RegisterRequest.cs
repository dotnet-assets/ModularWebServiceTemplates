using MediatR;

namespace ModularWebService.Auth.Contracts;

public record RegisterRequest(
        string Username,
        string Password)
    : IRequest<UserDto>;