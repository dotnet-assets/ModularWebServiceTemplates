using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyPetProject.Auth.Model;

namespace MyPetProject.Auth.Tests;

[TestClass]
public class AccountTest
{
    [DataTestMethod]
    [DataRow("123-qwe-!@#", "123-qwe-!@#", true)]
    [DataRow("   x   ", "   x   ", true)]
    [DataRow("   x   ", "  x  ", false)]
    [DataRow("asD111", "asd111", false)]
    [DataRow("asd111", "asD111", false)]
    [DataRow("asd111", "", false)]
    public void CheckPasswordIsCorrect(string password, string passwordToCheck, bool expected)
    {
        Account account = new Account("test-user", UserRole.User, password);
        bool result = account.CheckPassword(passwordToCheck);
        Assert.AreEqual(expected, result);
    }
}