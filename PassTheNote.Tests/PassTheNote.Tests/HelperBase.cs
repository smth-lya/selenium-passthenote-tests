using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PassTheNote.Tests;

public class HelperBase
{
    protected ApplicationManager manager;
    protected IWebDriver driver;
    protected WebDriverWait wait;

    public HelperBase(ApplicationManager manager)
    {
        this.manager = manager;
        this.driver = manager.Driver;
        this.wait = manager.Wait;
    }
}
