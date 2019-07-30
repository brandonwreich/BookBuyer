using OpenQA.Selenium;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL Classifed Book Section
        public void NavigateToKslBooksPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://classifieds.ksl.com/s/Books+and+Media/Books:+Education+and+College");
        }

        public void NavigateToBookFinderPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.bookfinder.com/buyback/");
        }

        //Closes Google Chrome
        public void ExitBrowser(IWebDriver driver)
        {
            driver.Close();
        }
    }
}