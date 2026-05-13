using OpenQA.Selenium;
using PassTheNote.Tests.Models;

namespace PassTheNote.Tests.Helpers;

public class LoginHelper : HelperBase
{
    public LoginHelper(ApplicationManager manager)
        : base(manager)
    {
    }

    public bool IsLoggedIn()
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
    }

    public bool IsLoggedIn(string email)
    {
        if (!IsLoggedIn())
            return false;

        try
        {
            var body = driver.FindElement(By.TagName("body")).Text;
            return body.Contains(email, StringComparison.OrdinalIgnoreCase);
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    public void Logout()
    {
        if (!IsLoggedIn())
            return;

        var userMenu = wait.Until(d =>
        {
            var el = d.FindElement(By.CssSelector("[data-testid='top-nav-user-menu']"));
            return el.Displayed && el.Enabled ? el : null!;
        });
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", userMenu);

        var logoutButton = wait.Until(d =>
        {
            var el = d.FindElement(By.XPath("//button[contains(., 'Logout')]"));
            return el.Displayed && el.Enabled ? el : null!;
        });
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", logoutButton);

        wait.Until(_ => !IsLoggedIn());
    }
    
    public void AttemptLogin(AccountData user)
    {
        manager.Navigation.NavigateTo("/auth/login");

        var emailInput = wait.Until(d =>
        {
            var element = d.FindElement(By.CssSelector("[data-testid='ptn-login-email-input']"));
            return element.Displayed ? element : null!;
        });

        emailInput.Clear();
        emailInput.SendKeys(user.Email);

        var passwordInput = driver.FindElement(By.CssSelector("[data-testid='ptn-login-password-input']"));
        passwordInput.Clear();
        passwordInput.SendKeys(user.Password);

        var submitButton = driver.FindElement(By.CssSelector("[data-testid='ptn-login-submit-button']"));
        submitButton.Click();
    }
    
    public void Login(AccountData user)
    {
        if (IsLoggedIn())
        {
            if (IsLoggedIn(user.Email))
                return;

            Logout();
        }

        manager.Navigation.NavigateTo("/auth/login");

        var emailInput = wait.Until(d =>
        {
            var element = d.FindElement(By.CssSelector("[data-testid='ptn-login-email-input']"));
            return element.Displayed ? element : null!;
        });

        emailInput.Clear();
        emailInput.SendKeys(user.Email);

        var passwordInput = driver.FindElement(By.CssSelector("[data-testid='ptn-login-password-input']"));
        passwordInput.Clear();
        passwordInput.SendKeys(user.Password);

        var submitButton = driver.FindElement(By.CssSelector("[data-testid='ptn-login-submit-button']"));
        submitButton.Click();

        wait.Until(d =>
        {
            try
            {
                var menu = d.FindElement(By.CssSelector("[data-testid='top-nav-user-menu']"));
                return menu.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        });
    }
}
