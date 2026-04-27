namespace PassTheNote.Tests.Helpers;

public class NavigationHelper : HelperBase
{
    private readonly string _baseUrl;

    public NavigationHelper(ApplicationManager manager, string baseUrl)
        : base(manager)
    {
        _baseUrl = baseUrl;
    }

    public void NavigateTo(string path)
    {
        driver.Navigate().GoToUrl(_baseUrl + path);
    }
}
