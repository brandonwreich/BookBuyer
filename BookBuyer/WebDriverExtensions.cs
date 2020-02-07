using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace BookBuyer
{
    static class WebDriverExtensions
    {
        //Waits for element to be visible or unitl timeout has expired
        public static void WaitTillVisible(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            //Init
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            //Set ignored exceptions
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException),
                    typeof(WebDriverException));

            //Wait and find
            wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        //Waits for element to be ready or until timout has expired
        public static void WaitToBeReady(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            //Init
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            //Set ignored exceptions
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException),
                    typeof(WebDriverException));

            //Wait
            wait.Until(ExpectedConditions.ElementExists(by));
        }

        //Waits for element to be clickable or until timeout has expired
        public static void WaitToBeClickable(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            //Init
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

            //Set ignored exceptions
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException),
                    typeof(WebDriverException));

            //Wait
            wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        //Checks to see if the Next Button is enabled
        public static bool IsNextButtonEnabled(this IWebDriver driver, int pageCount, string xPath)
        {
            try
            {
                //Find button
                IWebElement nextButton = driver.FindElement(By.XPath("//a[starts-with(@href, '" + xPath + pageCount + "')]"));
                driver.WaitTillVisible(By.XPath("//a[starts-with(@href, '" + xPath + pageCount + "')]"), 5);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}