using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace BookBuyer
{
    class Navigation
    {
        //Opens up Google Chrome and navigates the the KSL classifieds book section
        public void NavigateToKslBooksPage(IWebDriver driver, string website) => driver.Navigate().GoToUrl(website);

        //Navigates to the next page of search results
        public void NextKslPage(IWebDriver driver, int pageCount)
        {
            //Init list
            List<string> nextButtonList = new List<string> {"/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Education+and+College&page=",
                "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Non-fiction&page=",
                "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Religious&page=",
                "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Children&page=",
                "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Fiction&page=" };

            //Loop through list of XPaths
            foreach (var buttonXPath in nextButtonList)
            {
                try
                {
                    //Init varibles
                    string nextButtonXpath = "//a[starts-with(@href, '" + buttonXPath + pageCount + "')]";
                    IWebElement nextButton;

                    try
                    {
                        //Find nextButton
                        nextButton = driver.FindElement(By.XPath(nextButtonXpath));

                        //Click
                        driver.WaitToBeClickable(By.XPath(nextButtonXpath), 5);
                        nextButton.Click();
                    }
                    catch (StaleElementReferenceException)
                    {
                        //Refind element
                        nextButton = driver.FindElement(By.XPath(nextButtonXpath));

                        //Click
                        driver.WaitToBeClickable(By.XPath(nextButtonXpath), 5);
                        nextButton.Click();
                    }
                    catch (NoSuchElementException)
                    {
                        //Refind element
                        nextButton = driver.FindElement(By.XPath(nextButtonXpath));

                        //Click
                        driver.WaitToBeClickable(By.XPath(nextButtonXpath), 5);
                        nextButton.Click();
                    }
                }
                catch (Exception) { }
            }
        }

        //Maximizes the browser
        public void MaximizeBrower(IWebDriver driver) => driver.Manage().Window.Maximize();

        //Closes Google Chrome
        public void ExitBrowser(IWebDriver driver) => driver.Quit();
    }
}