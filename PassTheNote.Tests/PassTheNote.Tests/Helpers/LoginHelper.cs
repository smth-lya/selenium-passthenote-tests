using OpenQA.Selenium;
using PassTheNote.Tests.Models;

namespace PassTheNote.Tests.Helpers;

public class LoginHelper : HelperBase
{
    public LoginHelper(ApplicationManager manager)
        : base(manager)
    {
    }

    public void Login(AccountData user)
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
