using NUnit.Framework;
using OpenQA.Selenium;

namespace PassTheNote.Tests;

[TestFixture]
public class PassTheNote_ECommerceTests : TestBase
{
    private const string TestEmail = "tester@passthenote.com";
    private const string TestPassword = "Tester@123";

    [Test]
    public void AddProduct_ToCart_ShouldSucceed()
    {
        var user = new AccountData(TestEmail, TestPassword);
        Login(user);

        NavigateTo("/app/products");

        var addToCartButton = _wait.Until(driver =>
        {
            var element = driver.FindElement(By.CssSelector("[data-testid='ptn-product-add-button']"));
            return element.Displayed ? element : null!;
        });

        addToCartButton.Click();

        NavigateTo("/app/cart");

        var isCartItemPresent = _wait.Until(driver =>
        {
            try
            {
                var item = driver.FindElement(By.CssSelector("[data-testid='ptn-cart-item']"));
                return item.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        });

        Assert.IsTrue(isCartItemPresent, "Товар не был добавлен в корзину.");
    }
}
