using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PassTheNote.Tests;

[TestFixture]
public class TestBase
{
    protected IWebDriver _driver = null!;
    protected WebDriverWait _wait = null!;

    private const string BaseUrl = "https://www.passthenote.com";

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddArgument("--disable-features=PasswordLeakDetection,PasswordCheck");
        options.AddArgument("--disable-notifications");

        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    protected void NavigateTo(string path)
    {
        _driver.Navigate().GoToUrl(BaseUrl + path);
    }

    protected void Login(AccountData user)
    {
        NavigateTo("/auth/login");

        var emailInput = _wait.Until(driver =>
        {
            var element = driver.FindElement(By.CssSelector("[data-testid='ptn-login-email-input']"));
            return element.Displayed ? element : null!;
        });

        emailInput.Clear();
        emailInput.SendKeys(user.Email);

        var passwordInput = _driver.FindElement(By.CssSelector("[data-testid='ptn-login-password-input']"));
        passwordInput.Clear();
        passwordInput.SendKeys(user.Password);

        var submitButton = _driver.FindElement(By.CssSelector("[data-testid='ptn-login-submit-button']"));
        submitButton.Click();

        _wait.Until(driver =>
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
    }
}
