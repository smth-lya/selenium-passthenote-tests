using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PassTheNote.Tests;

public class ApplicationManager
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly string _baseUrl;

    private readonly NavigationHelper _navigation;
    private readonly LoginHelper _auth;

    public ApplicationManager()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddArgument("--disable-features=PasswordLeakDetection,PasswordCheck");
        options.AddArgument("--disable-notifications");

        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        _baseUrl = "https://www.passthenote.com";

        _navigation = new NavigationHelper(this, _baseUrl);
        _auth = new LoginHelper(this);
    }

    public IWebDriver Driver => _driver;

    public WebDriverWait Wait => _wait;

    public NavigationHelper Navigation => _navigation;

    public LoginHelper Auth => _auth;

    public void Stop()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
