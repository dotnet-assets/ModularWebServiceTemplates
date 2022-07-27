using MyPetProject.Auth.Contracts;
using Refit;

namespace MyPetProject.ApiClient;

public interface IApiClient
{
    // Auth
    [Post("/auth/register")]
    public Task<UserDto> AuthRegister(RegisterRequest request);

    [Post("/auth/login")]
    public Task<UserDto> AuthLogin(LoginRequest request);

    // ApiTest
    [Get("/api/test/anonymous")]
    public Task<string> TestAnonymous();

    [Get("/api/test/authorize")]
    public Task<string> TestAuthorize();

    [Get("/api/test/admin")]
    public Task<string> TestAdmin();

    // Other Modules
}