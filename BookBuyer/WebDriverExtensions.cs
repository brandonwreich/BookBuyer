using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace BookBuyer
{
    static class WebDriverExtensions
    {
        /*
         * Method searches for the element until found for a set period of time
         */
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            //If time period is set
            if (timeoutInSeconds > 0)
            {
                //Init
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));

                return wait.Until(drv => drv.FindElement(by));
            }

            return driver.FindElement(by);
        }
    }
}
