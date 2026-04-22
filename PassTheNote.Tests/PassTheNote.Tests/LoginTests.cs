using NUnit.Framework;
using OpenQA.Selenium;

namespace PassTheNote.Tests;

[TestFixture]
public class PassTheNote_LoginTests : TestBase
{
    private const string TestEmail = "tester@passthenote.com";
    private const string TestPassword = "Tester@123";

    [Test]
    public void Login_WithValidCredentials_ShouldSucceed()
    {
        var user = new AccountData(TestEmail, TestPassword);
        app.Auth.Login(user);

        var isLoginSuccessful = app.Wait.Until(driver =>
        {
            try
            {
                var menu = driver.FindElement(By.CssSelector("[data-testid='top-nav-user-menu']"));
                return menu.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        });

        Assert.IsTrue(isLoginSuccessful, "Авторизация не удалась: не найден элемент профиля/выхода после попытки логина.");
    }
}