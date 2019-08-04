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

        public void findIsbn(IWebDriver driver, String title, String edition)
        {
            Navigation navigationPage = new Navigation();

            IWebElement searchBar = driver.FindElement(By.Id("search_query"));

            searchBar.SendKeys(title);  
            searchBar.SendKeys(Keys.Enter);

            IWebElement bookIsbn = driver.FindElement(By.XPath(
                "//*[@id='block - multipurpose - business - theme - content']/div[1]/div[3]/div/div[2]/dl/dt[3]/text()"));

            Console.WriteLine(bookIsbn.ToString());

            navigationPage.NavigateToIsbnSearchPage(driver);
        }
    }
}