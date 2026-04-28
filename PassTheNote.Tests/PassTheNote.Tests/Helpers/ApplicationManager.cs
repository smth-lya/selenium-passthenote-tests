using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PassTheNote.Tests.Helpers;

public class ApplicationManager
{
    private static ThreadLocal<ApplicationManager> _instance = new();

    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly string _baseUrl;
    private bool _disposed;

    private readonly NavigationHelper _navigation;
    private readonly LoginHelper _auth;

    private ApplicationManager()
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

    public static ApplicationManager GetInstance()
    {
        if (!_instance.IsValueCreated || _instance.Value is null)
        {
            var newInstance = new ApplicationManager();
            newInstance.Navigation.NavigateTo("/");
            _instance.Value = newInstance;
        }

        return _instance.Value!;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        try
        {
            _driver.Quit();
        }
        catch (Exception)
        {
            // ignore
        }
        finally
        {
            _driver.Dispose();
            _disposed = true;
            GC.SuppressFinalize(this);

            if (_instance.IsValueCreated && ReferenceEquals(_instance.Value, this))
                _instance.Value = null!;
        }
    }

    public static void Stop()
    {
        if (!_instance.IsValueCreated || _instance.Value is null)
            return;

        _instance.Value.Dispose();
        _instance = new ThreadLocal<ApplicationManager>();
    }

    ~ApplicationManager()
    {
        Dispose();
    }
}