using OpenQA.Selenium;
using System;

namespace BookBuyer
{
    class Navigation
    {
        bool success = false;
        IWebElement nextButton = null;

        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver) => driver.Navigate().GoToUrl("https://classifieds.ksl.com/s/Books+and+Media/Books:+Education+and+College?perPage=96");

        //Navigates to the next page of search results
        public void NextKslPage(IWebDriver driver, int pageCount)
        {
            while (nextButton == null)
            {
                try
                {
                    nextButton = driver.FindElement(By.XPath("//a[starts-with(@href, '/search/index?page=" + pageCount + "')]"));
                }
                catch (WebDriverException) { }
            }

            while (success == false)
            {
                try
                {
                    nextButton.Click();
                    success = true;
                }
                catch (StaleElementReferenceException) { }
            }
        }

        //Maximizes the browser
        public void MaximizeBrower(IWebDriver driver) => driver.Manage().Window.Maximize();

        //Closes Google Chrome
        public void ExitBrowser(IWebDriver driver) => driver.Quit();
    }
}