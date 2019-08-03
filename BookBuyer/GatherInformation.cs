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