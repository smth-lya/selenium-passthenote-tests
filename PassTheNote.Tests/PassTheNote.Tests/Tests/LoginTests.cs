using System.Xml.Serialization;
using OpenQA.Selenium;
using PassTheNote.Tests.Helpers;
using PassTheNote.Tests.Models;

namespace PassTheNote.Tests.Tests;

[TestFixture]
public class PassTheNote_LoginTests : TestBase
{
    private const string TestEmail = "tester@passthenote.com";
    private const string TestPassword = "Tester@123";

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
        var user = new AccountData(TestEmail, TestPassword);

        Assert.DoesNotThrow(() => app.Auth.Login(user),
            "Авторизация не удалась: не найден элемент профиля/выхода после попытки логина.");
    }

    [Test, TestCaseSource(nameof(AccountDataFromXmlFile))]
    public void Login_WithXmlData_ShouldSucceed(AccountData user)
    {
        Assert.DoesNotThrow(() => app.Auth.Login(user),
            $"Авторизация не удалась для пользователя {user.Email}");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        ApplicationManager.Stop();
    }
}