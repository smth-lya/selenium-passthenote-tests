using PassTheNote.Tests.Helpers;

namespace PassTheNote.Tests.Tests;

[TestFixture]
public abstract class TestBase
{
    protected ApplicationManager app = null!;

    [SetUp]
    public void SetUp()
    {
        app = ApplicationManager.GetInstance();
    }

    [TearDown]
    public void TearDown()
    {
        app.Dispose();
        app = null!;
    }
}
