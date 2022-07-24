using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularWebService.ApiClient;
using ModularWebService.Auth.Contracts;
using ModularWebService.Auth.Model;

namespace ModularWebService.Tests;

[TestClass]
public class AuthTest
{
    [TestMethod]
    public async Task UserCanRegisterAndLogin()
    {
        IApiClient api = await ApplicationFactory.CreateApiClient();

        await api.AuthRegister(new RegisterRequest("new-user", "qwerty"));
        UserDto user = await api.AuthLogin(new LoginRequest("new-user", "qwerty"));

        Assert.AreEqual("new-user", user.Name);
        Assert.AreEqual(UserRole.User.ToString(), user.Role);
        Assert.AreNotEqual(default, user.Created);
        Assert.IsNotNull(user.Token);
        Assert.IsNotNull(user.TokenValidTo);
    }
}