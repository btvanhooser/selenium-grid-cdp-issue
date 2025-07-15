using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace SeleniumGridCDP;

public class SeleniumGridCdpTests
{
    private const BrowserLocation Browser = BrowserLocation.DockerGrid;

    private IWebDriver _driver;
    private WikipediaHomePage _homePage;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
            UseWebSocketUrl = true
        };
        options.AddArgument("--remote-debugging-port=9222");

        _driver = Browser switch
        {
            BrowserLocation.LocalInstall => new ChromeDriver(options),
            BrowserLocation.DockerGrid => new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options),
            BrowserLocation.KubernetesGrid => new RemoteWebDriver(new Uri("http://localhost:50000/wd/hub"), options),
            _ => throw new ArgumentOutOfRangeException()
        };

        _homePage = new WikipediaHomePage(_driver);
    }

    [Test]
    public void AttemptToJustRunSomethingBasic()
    {
        _homePage.GoTo();
        Assert.Multiple(() =>
        {
            Assert.That(_homePage.SearchFieldIsDisplayed, Is.True);
            Assert.That(_homePage.MainLogoIsCorrect, Is.True);
        });
    }

    [Test]
    public async Task AttemptToDoSomethingWithTheNetwork()
    {
        var network = _driver.Manage().Network;
        var handler = new NetworkRequestHandler
        {
            RequestMatcher = request =>
            {
                Console.WriteLine(request.Url);
                return request.Url!.Contains("Wikipedia-logo-v2.png");
            },
            ResponseSupplier = _ => new HttpResponseData { StatusCode = 404 }
        };
        network.AddRequestHandler(handler);
        await network.StartMonitoring();
        _homePage.GoTo();
        Assert.Multiple(() =>
        {
            Assert.That(_homePage.SearchFieldIsDisplayed, Is.True);
            Assert.That(_homePage.MainLogoIsCorrect, Is.False);
        });
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}