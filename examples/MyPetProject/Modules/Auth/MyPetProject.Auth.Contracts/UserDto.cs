namespace MyPetProject.Auth.Contracts;

public record UserDto(
    string Name,
    string Role,
    DateTime Created,
    string? Token,
    DateTime? TokenValidTo);