using OpenQA.Selenium;
using System;

namespace BookBuyer
{
    class GatherInformation
    {
        public void GrabBookInfo(IWebDriver driver)
        {
            IWebElement bookInformation = driver.FindElement(By.XPath("//script[contains(.,'window.renderSearchSection')]"));
            var text = bookInformation.GetAttribute("innerText");
            Console.WriteLine(text);


            // The text is a giant string, we only want the actual json part of it. Find a way to substring the text so that we have 
            // just the data in the array --> listings: [...], we want just the [...]

            // With the new string variable that is just an array of json objects, convert this into c# objects:
            // 1. create a c# class that is a model of "listing". It should have relevant fields like title, description, price, etc.
            //    Put this class in a folder titled "Models"
            // 2. use newtonsoft nuget package to convert the string json array to a c# array of type "listing" that you just created. 
            //    Google will help you
        }
    }
}