using OpenQA.Selenium;
using System;

namespace BookBuyer
{
    class GatherInformation
    {
        public void GrabBookInfo(IWebDriver driver)
        {
            var bookInformation = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[2]/div[2]/script"));

            Console.WriteLine(bookInformation.ToString());
        }
    }
}