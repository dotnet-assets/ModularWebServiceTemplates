using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPetProject.ApiClient;
using Refit;

namespace MyPetProject.Tests;

[TestClass]
public class ApiTest
{
    [TestMethod]
    public async Task ApiTestAnonymous()
    {
        IApiClient apiClient = await ApplicationFactory.CreateApiClient();

        string result = await apiClient.TestAnonymous();

        Assert.AreEqual("Anonymous - OK", result);
    }

    [TestMethod]
    public async Task ApiTestAuthorize()
    {
        IApiClient apiClient = await ApplicationFactory.CreateApiClientAndLogin("user", "654321");

        string result = await apiClient.TestAuthorize();

        Assert.AreEqual("Authorize - OK", result);
    }

    [TestMethod]
    public async Task ApiTestAdmin()
    {
        IApiClient apiClient = await ApplicationFactory.CreateApiClientAndLogin("admin", "123456");

        string result = await apiClient.TestAdmin();

        Assert.AreEqual("Admin - OK", result);
    }

    [TestMethod]
    public async Task AdminApiMethodsImpossibleUseWithoutAuth()
    {
        IApiClient apiClient = await ApplicationFactory.CreateApiClient();
        await AssertFailWithStatusCode(() => apiClient.TestAdmin(), HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public async Task AdminApiMethodsImpossibleUseWithUserRole()
    {
        IApiClient apiClient = await ApplicationFactory.CreateApiClient();
        await AssertFailWithStatusCode(() => apiClient.TestAdmin(), HttpStatusCode.Unauthorized);
    }

    private async Task AssertFailWithStatusCode(Func<Task> action, HttpStatusCode expectedStatusCode)
    {
        try
        {
            await action();
            Assert.Fail();
        }
        catch (ApiException e)
        {
            Assert.AreEqual(expectedStatusCode, e.StatusCode);
        }
    }
}