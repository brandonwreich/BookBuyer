using OpenQA.Selenium;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver, string website) => driver.Navigate().GoToUrl(website);

        //Navigates to the next page of search results
        public void NextKslPage(IWebDriver driver, int pageCount)
        {
            //Init varibles
            string nextButtonXpath = "//a[starts-with(@href, '/search/index?page=" + pageCount + "')]";
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
            catch (StaleElementReferenceException)
            {
                //Refind element
                nextButton = driver.FindElement(By.XPath(nextButtonXpath));

                //Click
                driver.WaitToBeClickable(By.XPath(nextButtonXpath), 100);
                nextButton.Click();
            }
            catch (NoSuchElementException)
            {
                //Refind element
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