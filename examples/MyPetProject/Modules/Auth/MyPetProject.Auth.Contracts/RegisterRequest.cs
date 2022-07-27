using MediatR;

namespace MyPetProject.Auth.Contracts;

public record RegisterRequest(
        string Username,
        string Password)
    : IRequest<UserDto>;