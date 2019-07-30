using OpenQA.Selenium;
using System;

namespace BookBuyer
{
    class GatherInformation
    {
        public void GrabInfo(IWebDriver driver)
        {
            var information = driver.FindElement(By.XPath("//*[@id='search-results']/type"));

            Console.WriteLine(information.ToString());
        }
    }
}