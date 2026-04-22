using NUnit.Framework;

namespace PassTheNote.Tests;

[TestFixture]
public class TestBase
{
    protected ApplicationManager app = null!;

    [SetUp]
    public void SetUp()
    {
        app = new ApplicationManager();
    }

    [TearDown]
    public void TearDown()
    {
        app.Stop();
    }
}
