using System.Xml.Serialization;
using OpenQA.Selenium;
using PassTheNote.Tests.Helpers;
using PassTheNote.Tests.Models;
using PassTheNote.Tests.Settings;

namespace PassTheNote.Tests.Tests;


[TestFixture]
public class PassTheNote_LoginTests : TestBase
{
    public static IEnumerable<AccountData> AccountDataFromXmlFile()
    {
        var xmlPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "TestData",
            "accounts.xml");

        var serializer = new XmlSerializer(typeof(List<AccountData>));
        using var reader = new StreamReader(xmlPath);
        return (List<AccountData>)serializer.Deserialize(reader)!;
    }

    [Test]
    public void Login_WithValidCredentials_ShouldSucceed()
    {
        app.Auth.Logout();

        app.Auth.Login(AppSettings.DefaultAccount);

        Assert.That(app.Auth.IsLoggedIn(), Is.True,
            "Авторизация не удалась: пользователь не авторизован после входа с валидными данными.");
        Assert.That(app.Auth.IsLoggedIn(AppSettings.DefaultEmail), Is.True,
            $"Авторизован другой пользователь, ожидался: {AppSettings.DefaultEmail}");
    }

    [Test]
    public void Login_WithInvalidCredentials_ShouldFail()
    {
        app.Auth.Logout();

        var invalidUser = new AccountData("invalid_user_xyz@test.com", "WrongPassword999!");
        app.Auth.AttemptLogin(invalidUser);

        Thread.Sleep(2000);

        Assert.That(app.Auth.IsLoggedIn(), Is.False,
            "Пользователь не должен быть авторизован с невалидными данными.");
    }

    [Test, TestCaseSource(nameof(AccountDataFromXmlFile))]
    public void Login_WithXmlData_ShouldSucceed(AccountData user)
    {
        app.Auth.Logout();
        app.Auth.Login(user);

        Assert.That(app.Auth.IsLoggedIn(), Is.True,
            $"Авторизация не удалась для пользователя {user.Email}");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        ApplicationManager.Stop();
    }
}
