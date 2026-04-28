using OpenQA.Selenium;
using PassTheNote.Tests.Helpers;
using PassTheNote.Tests.Models;

namespace PassTheNote.Tests.Tests;

[TestFixture]
public class PassTheNote_LoginTests : TestBase
{
    private const string TestEmail = "tester@passthenote.com";
    private const string TestPassword = "Tester@123";

    [Test]
    public void Login_WithValidCredentials_ShouldSucceed()
    {
        var user = new AccountData(TestEmail, TestPassword);

        Assert.DoesNotThrow(() => app.Auth.Login(user),
            "Авторизация не удалась: не найден элемент профиля/выхода после попытки логина.");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        ApplicationManager.Stop();
    }
}