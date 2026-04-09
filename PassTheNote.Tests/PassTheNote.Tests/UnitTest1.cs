using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PassTheNote.Tests;

[TestFixture]
public class PassTheNote_LoginTests
{
    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;

    private const string LoginUrl = "https://www.passthenote.com/auth/login";

    private const string TestEmail = "tester@passthenote.com";
    private const string TestPassword = "Tester@123";

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        // options.AddArgument("--incognito"); 

        _driver = new ChromeDriver(options);

        Console.WriteLine(null == null);
        Console.WriteLine(double.NaN == double.NaN);
        
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Test]
    public void Login_WithValidCredentials_ShouldSucceed()
    {
        _driver.Navigate().GoToUrl(LoginUrl);

        var emailInput = _wait.Until(driver =>
        {
            var element = driver.FindElement(By.CssSelector("input[type='email'], input[name='email'], input[name='username']"));
            return element.Displayed ? element : null!; 
        });
        
        emailInput.Clear();
        emailInput.SendKeys(TestEmail);

        var passwordInput = _driver.FindElement(By.CssSelector("input[type='password'], input[name='password']"));
        passwordInput.Clear();
        passwordInput.SendKeys(TestPassword);

        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
        submitButton.Click();

        var isLoginSuccessful = _wait.Until(driver =>
        {
            try
            {
                var indicator = driver.FindElement(By.CssSelector("a[href*='logout'], button[type='submit'], [data-testid='notes'], .dashboard"));
                return indicator.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        });

        Assert.IsTrue(isLoginSuccessful, "Авторизация не удалась: не найден элемент профиля/выхода после попытки логина.");
    }
}