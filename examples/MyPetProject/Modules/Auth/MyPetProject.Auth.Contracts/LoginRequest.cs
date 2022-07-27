using MediatR;

namespace MyPetProject.Auth.Contracts;

public record LoginRequest(
        string Username,
        string Password)
    : IRequest<UserDto>;