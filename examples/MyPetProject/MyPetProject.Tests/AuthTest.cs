using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPetProject.ApiClient;
using MyPetProject.Auth.Contracts;
using MyPetProject.Auth.Model;

namespace MyPetProject.Tests;

[TestClass]
public class AuthTest
{
    [TestMethod]
    public async Task FirstRegisteredUserBecomesAdmin()
    {
        IApiClient api = await ApplicationFactory.CreateApiClient(needCreateAccounts: false);

        UserDto user = await api.AuthRegister(new RegisterRequest("Mik", "12345asd"));

        Assert.AreEqual(UserRole.Admin.ToString(), user.Role);
    }

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