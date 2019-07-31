using OpenQA.Selenium;
using System;

namespace BookBuyer
{
    class GatherInformation
    {
        public void GrabBookInfo(IWebDriver driver)
        {
            IWebElement bookInformation = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[2]/div[2]/script/text()"));
            Console.WriteLine(bookInformation);
        }
    }
}