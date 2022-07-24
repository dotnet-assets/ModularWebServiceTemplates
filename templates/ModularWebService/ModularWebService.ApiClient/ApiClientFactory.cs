using System.Net.Http.Headers;
using ModularWebService.Auth.Contracts;
using Refit;

namespace ModularWebService.ApiClient;

public static class ApiClientFactory
{
    public static IApiClient Create(HttpClient httpClient)
    {
        return RestService.For<IApiClient>(httpClient);
    }

    public static async Task<IApiClient> CreateWithLogin(HttpClient httpClient, string username,
        string password)
    {
        IApiClient client = RestService.For<IApiClient>(httpClient);
        UserDto user = await client.AuthLogin(new LoginRequest(username, password));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

        return RestService.For<IApiClient>(httpClient);
    }
}