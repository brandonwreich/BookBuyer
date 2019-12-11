using OpenQA.Selenium;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver) => driver.Navigate().GoToUrl("https://classifieds.ksl.com/s/Books+and+Media/Books:+Education+and+College");

        //Navigates to the next page of search results
        public void NextKslPage(IWebDriver driver, int pageCount)
        {
            string nextButtonXpath = "//a[starts-with(@href, '/search/index?page=" + pageCount + "')]";

            //Find nextButton
            driver.WaitTillVisible(By.XPath(nextButtonXpath), 100);
            IWebElement nextButton = driver.FindElement(By.XPath(nextButtonXpath));

            //Click
            nextButton.Click();
        }

        //Maximizes the browser
        public void MaximizeBrower(IWebDriver driver) => driver.Manage().Window.Maximize();

        //Closes Google Chrome
        public void ExitBrowser(IWebDriver driver) => driver.Quit();
    }
}