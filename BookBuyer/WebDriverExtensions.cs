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
                return wait.Until(drv => drv.FindElement(by));
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
    }
}
