﻿using OpenQA.Selenium;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://classifieds.ksl.com/s/Books+and+Media/Books:+Education+and+College?perPage=96");
        }

        //Navigates to the next page of search results
        public void NextKslPage(IWebDriver driver, int pageCount)
        {
            IWebElement nextButton = driver.FindElement(By.XPath("//a[starts-with(@href, '/search/index?page=" + pageCount + "')]"));

            nextButton.Click();
        }

        public IWebElement FindNextLink(IWebDriver driver, int pageCount)
        {
            IWebElement nextButton = driver.FindElement(By.XPath("//a[starts-with(@href, '/search/index?page=" + pageCount + "')]"));

            return nextButton;
        }

        //Navigates to the ISBN search page
        public void NavigateToIsbnSearchPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://isbndb.com/");
        }

        //Navigates to the Book Finder page
        public void NavigateToBookFinderPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.bookfinder.com/buyback/");
        }

        //Opens a new tab and switches the driver to that tab
        public void NewTab(IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");

            var newTab = driver.WindowHandles[driver.WindowHandles.Count - 1];

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
            driver.Quit();
        }
    }
}