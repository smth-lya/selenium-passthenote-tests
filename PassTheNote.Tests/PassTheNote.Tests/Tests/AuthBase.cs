using PassTheNote.Tests.Helpers;
using PassTheNote.Tests.Settings;

namespace PassTheNote.Tests.Tests;

[TestFixture]
public abstract class AuthBase : TestBase
{
    [SetUp]
    public void AuthSetUp()
    {
        app.Auth.Login(AppSettings.DefaultAccount);
    }
}
