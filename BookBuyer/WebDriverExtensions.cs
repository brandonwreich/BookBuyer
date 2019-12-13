using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace BookBuyer
{
    static class WebDriverExtensions
    {
        //Searches for element until found or timeout expires
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            //If time period is set
            if (timeoutInSeconds > 0)
            {
                //Init
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                //Set ignored exceptions
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException),
                    typeof(WebDriverException));

                //Wait and find
                try
                {
                    return wait.Until(drv => drv.FindElement(by));
                }
                catch (WebDriverTimeoutException)
                {
                    return driver.FindElement(by);
                }
                catch (NoSuchElementException)
                {
                    return driver.FindElement(by);
                }
            }

            //Find element
            return driver.FindElement(by);
        }

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
    }
}