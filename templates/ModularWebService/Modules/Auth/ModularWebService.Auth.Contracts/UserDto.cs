namespace ModularWebService.Auth.Contracts;

public record UserDto(
    string Name,
    string Role,
    DateTime Created,
    string? Token,
    DateTime? TokenValidTo);