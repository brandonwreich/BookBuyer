using OpenQA.Selenium;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://classifieds.ksl.com/s/Books+and+Media/Books:+Education+and+College");
        }

        //Navigates to the Book Finder page
        public void NavigateToBookFinderPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.bookfinder.com/buyback/");
        }

        //Opens a new tab
        public void NewTab(IWebDriver driver, int tabNumber)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");

            var newTab = driver.WindowHandles[tabNumber];

            driver.SwitchTo().Window(newTab);
        }

        //Maximizes the browser
        public void MaximizeBrower(IWebDriver driver)
        {
            driver.Manage().Window.Maximize();
        }

        //Closes Google Chrome
        public void ExitBrowser(IWebDriver driver)
        {
           // while(driver.WindowHandles.Count != 0)
           // {
                driver.Quit();
          //  }
        }
    }
}