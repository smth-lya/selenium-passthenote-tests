using OpenQA.Selenium;
using PassTheNote.Tests.Models;

namespace PassTheNote.Tests.Tests;

[TestFixture]
public class PassTheNote_ECommerceTests : TestBase
{
    private const string TestEmail = "tester@passthenote.com";
    private const string TestPassword = "Tester@123";
    private const string CartClientErrorText = "Cannot read properties of null (reading 'cart')";
    private static readonly By ProductCardSelector = By.CssSelector("[data-testid='ptn-product-card']");
    private static readonly By CartItemSelector = By.CssSelector("[data-testid='ptn-cart-item']");
    private static readonly By CartItemNameSelector = By.CssSelector("[data-testid='ptn-cart-item-name']");
    private static readonly By CartItemRemoveSelector = By.CssSelector("[data-testid='ptn-cart-item-remove']");
    private static readonly By CartEmptyStateSelector = By.CssSelector("[data-testid='ptn-cart-empty-continue-shopping']");
    private static readonly By ProductNameSelector = By.XPath(".//h3");
    private static readonly By AddToCartButtonSelector = By.XPath(".//button[normalize-space()='Add to Cart']");

    [Test]
    public void AddProduct_ToCart_ShouldSucceed()
    {
        var user = new AccountData(TestEmail, TestPassword);
        app.Auth.Login(user);
        EnsureCartIsEmpty();

        app.Navigation.NavigateTo("/app/products");

        var productCard = FindFirstReadyProductCard();

        var expectedProductName = productCard.FindElement(ProductNameSelector).Text.Trim();

        var addToCartButton = productCard.FindElement(AddToCartButtonSelector);
        ClickElement(addToCartButton);
        WaitForProductAddedToCart(expectedProductName);

        app.Navigation.NavigateTo("/app/cart");

        var cartItem = WaitForCartItem(expectedProductName);

        var actualProductName = cartItem.FindElement(CartItemNameSelector).Text.Trim();

        Assert.That(cartItem.Displayed, Is.True, "Товар не был добавлен в корзину.");
        Assert.That(actualProductName, Is.EqualTo(expectedProductName),
            "Название товара в корзине не совпадает с добавленным.");
    }

    [Test]
    public void RemoveProduct_FromCart_ShouldSucceed()
    {
        var user = new AccountData(TestEmail, TestPassword);
        app.Auth.Login(user);
        EnsureCartIsEmpty();

        app.Navigation.NavigateTo("/app/products");

        var productCard = FindFirstReadyProductCard();
        var expectedProductName = productCard.FindElement(ProductNameSelector).Text.Trim();
        var addToCartButton = productCard.FindElement(AddToCartButtonSelector);

        ClickElement(addToCartButton);
        WaitForProductAddedToCart(expectedProductName);

        app.Navigation.NavigateTo("/app/cart");

        var cartItem = WaitForCartItem(expectedProductName);
        var removeButton = cartItem.FindElement(CartItemRemoveSelector);

        ClickElement(removeButton);
        WaitForCartMutation(1);
        app.Navigation.NavigateTo("/app/cart");
        WaitForCartToBeEmpty();

        var cartItems = app.Driver.FindElements(CartItemSelector);

        Assert.That(cartItems.Count, Is.EqualTo(0), "Корзина не пуста после удаления товара.");
        Assert.That(app.Driver.FindElements(CartEmptyStateSelector).Count, Is.GreaterThan(0),
            "Не отобразилось пустое состояние корзины после удаления товара.");
    }

    private IWebElement FindFirstReadyProductCard()
    {
        return app.Wait.Until(driver =>
        {
            foreach (var card in driver.FindElements(ProductCardSelector))
            {
                try
                {
                    if (!card.Displayed)
                    {
                        continue;
                    }

                    if (card.FindElements(ProductNameSelector).Count == 0)
                    {
                        continue;
                    }

                    if (card.FindElements(AddToCartButtonSelector).Count == 0)
                    {
                        continue;
                    }

                    return card;
                }
                catch (StaleElementReferenceException)
                { }
            }

            return null!;
        });
    }

    private IWebElement WaitForCartItem(string productName)
    {
        return app.Wait.Until(driver =>
        {
            foreach (var cartItem in driver.FindElements(CartItemSelector))
            {
                try
                {
                    if (!cartItem.Displayed)
                    {
                        continue;
                    }

                    var itemNames = cartItem.FindElements(CartItemNameSelector);
                    if (itemNames.Count == 0)
                    {
                        continue;
                    }

                    if (string.Equals(itemNames[0].Text.Trim(), productName, StringComparison.Ordinal))
                    {
                        return cartItem;
                    }
                }
                catch (StaleElementReferenceException)
                { }
            }

            return null!;
        });
    }

    private void EnsureCartIsEmpty()
    {
        app.Navigation.NavigateTo("/app/cart");
        WaitForCartState();

        while (app.Driver.FindElements(CartItemSelector).Count > 0)
        {
            var currentItemCount = app.Driver.FindElements(CartItemSelector).Count;
            var removeButton = app.Wait.Until(driver =>
            {
                foreach (var button in driver.FindElements(CartItemRemoveSelector))
                {
                    try
                    {
                        if (button.Displayed && button.Enabled)
                        {
                            return button;
                        }
                    }
                    catch (StaleElementReferenceException)
                    { }
                }

                return null!;
            });

            ClickElement(removeButton);

            WaitForCartMutation(currentItemCount);

            if (HasCartClientError(app.Driver))
            {
                app.Navigation.NavigateTo("/app/cart");
                WaitForCartState();
            }
        }

        app.Navigation.NavigateTo("/app/cart");
        WaitForCartToBeEmpty();
    }

    private void WaitForCartState()
    {
        app.Wait.Until(driver =>
            driver.FindElements(CartItemSelector).Count > 0
            || driver.FindElements(CartEmptyStateSelector).Count > 0);
    }

    private void WaitForProductAddedToCart(string productName)
    {
        app.Wait.Until(driver =>
        {
            try
            {
                var pageText = driver.FindElement(By.TagName("body")).Text;
                return pageText.Contains($"{productName} added to cart!", StringComparison.Ordinal);
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
    }

    private void WaitForCartMutation(int previousItemCount)
    {
        app.Wait.Until(driver =>
            driver.FindElements(CartItemSelector).Count < previousItemCount
            || driver.FindElements(CartEmptyStateSelector).Count > 0
            || HasCartClientError(driver));
    }

    private void WaitForCartToBeEmpty()
    {
        app.Wait.Until(driver =>
            driver.FindElements(CartItemSelector).Count == 0
            && driver.FindElements(CartEmptyStateSelector).Count > 0);
    }

    private bool HasCartClientError(IWebDriver driver)
    {
        try
        {
            return driver.FindElement(By.TagName("body")).Text.Contains(CartClientErrorText, StringComparison.Ordinal);
        }
        catch (StaleElementReferenceException)
        {
            return false;
        }
    }

    private void ClickElement(IWebElement element)
    {
        ((IJavaScriptExecutor)app.Driver).ExecuteScript("arguments[0].click();", element);
    }
}

