using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModularWebService.ApiClient;
using Refit;

namespace ModularWebService.Tests;

[TestClass]
public class ApiTest
{
    [TestMethod]
    public async Task ApiTestAnonymous()
    {
        IApiClient apiClient = new ApplicationFactory()
            .GetClient();

        string result = await apiClient.TestAnonymous();

        Assert.AreEqual("Anonymous - OK", result);
    }

    [TestMethod]
    public async Task ApiTestAuthorize()
    {
        IApiClient apiClient = new ApplicationFactory()
            .InitDb()
            .Login("user", "654321")
            .GetClient();

        string result = await apiClient.TestAuthorize();

        Assert.AreEqual("Authorize - OK", result);
    }

    [TestMethod]
    public async Task ApiTestAdmin()
    {
        IApiClient apiClient = new ApplicationFactory()
            .InitDb()
            .Login("admin", "123456")
            .GetClient();

        string result = await apiClient.TestAdmin();

        Assert.AreEqual("Admin - OK", result);
    }

    [TestMethod]
    public async Task AdminApiMethodsImpossibleUseWithoutAuth()
    {
        IApiClient apiClient = new ApplicationFactory()
            .GetClient();

        await AssertFailWithStatusCode(() => apiClient.TestAdmin(), HttpStatusCode.Unauthorized);
    }

    [TestMethod]
    public async Task AdminApiMethodsImpossibleUseWithUserRole()
    {
        IApiClient apiClient = new ApplicationFactory()
            .InitDb()
            .Login("user", "654321")
            .GetClient();

        await AssertFailWithStatusCode(() => apiClient.TestAdmin(), HttpStatusCode.Forbidden);
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