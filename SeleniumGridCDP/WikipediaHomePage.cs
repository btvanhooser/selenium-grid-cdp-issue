using OpenQA.Selenium;

namespace SeleniumGridCDP;

public class WikipediaHomePage(IWebDriver driver)
{
    public void GoTo() => driver.Navigate().GoToUrl("https://www.wikipedia.org/");
    public bool MainLogoIsCorrect => int.Parse(MainLogo.GetAttribute("naturalWidth")) > 0;
    public bool SearchFieldIsDisplayed => SearchField.Displayed;
    private IWebElement MainLogo => driver.FindElement(By.ClassName("central-featured-logo"));
    private IWebElement SearchField => driver.FindElement(By.Id("searchInput"));
}