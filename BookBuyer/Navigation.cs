using OpenQA.Selenium;
using System;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver, string website) => driver.Navigate().GoToUrl(website);

        //Navigates to the next page of search results
        public void NextKslPage(IWebDriver driver, int pageCount, string xPath)
        {
            //Init varibles
            string nextButtonXpath = "//a[starts-with(@href, '" + xPath + pageCount + "')]";
            IWebElement nextButton;

            try
            {
                //Find nextButton
                driver.WaitTillVisible(By.XPath(nextButtonXpath), 100);
                nextButton = driver.FindElement(By.XPath(nextButtonXpath));

                //Click
                driver.WaitToBeClickable(By.XPath(nextButtonXpath), 100);
                nextButton.Click();
            }
            catch(Exception)
            {
                //Refind nextButton
                driver.WaitTillVisible(By.XPath(nextButtonXpath), 100);
                nextButton = driver.FindElement(By.XPath(nextButtonXpath));

                //Click
                driver.WaitToBeClickable(By.XPath(nextButtonXpath), 100);
                nextButton.Click();
            }
        }

        //Maximizes the browser
        public void MaximizeBrower(IWebDriver driver) => driver.Manage().Window.Maximize();

        //Closes Google Chrome
        public void ExitBrowser(IWebDriver driver) => driver.Quit();
    }
}