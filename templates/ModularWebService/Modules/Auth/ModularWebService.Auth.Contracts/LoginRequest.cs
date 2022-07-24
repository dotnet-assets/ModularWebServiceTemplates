using MediatR;

namespace ModularWebService.Auth.Contracts;

public record LoginRequest(
        string Username,
        string Password)
    : IRequest<UserDto>;