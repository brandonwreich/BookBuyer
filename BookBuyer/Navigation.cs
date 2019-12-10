using OpenQA.Selenium;
using System;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver) => driver.Navigate().GoToUrl("https://classifieds.ksl.com/s/Books+and+Media/Books:+Education+and+College?perPage=96");

        //Navigates to the next page of search results
        public void NextKslPage(IWebDriver driver, int pageCount)
        {
            IWebElement nextButton = driver.FindElement(By.XPath("//a[starts-with(@href, '/search/index?page=" + pageCount + "')]"));

            try
            {
                nextButton.Click();
            }
            catch (StaleElementReferenceException)
            {
                nextButton.Click();
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        //Maximizes the browser
        public void MaximizeBrower(IWebDriver driver) => driver.Manage().Window.Maximize();

        //Closes Google Chrome
        public void ExitBrowser(IWebDriver driver) => driver.Quit();
    }
}