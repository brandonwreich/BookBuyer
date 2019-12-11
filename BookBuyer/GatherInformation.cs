using BookBuyer.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BookBuyer
{
    class GatherInformation
    {
        public List<Listing> GrabBookInfo(IWebDriver driver)
        {
            //Init variables
            Regex regex = new Regex(@"\[{.*}\]");
            string bookInformationXpath = "//script[contains(.,'window.renderSearchSection')]";

            //Find information
            driver.WaitToBeReady(By.XPath(bookInformationXpath), 100);
            IWebElement bookInformation = driver.FindElement(By.XPath(bookInformationXpath), 100);

            //Grab information
            var text = bookInformation.GetAttribute("innerText");

            //Grab the wanted string
            var info = regex.Match(text).ToString();

            //Convert to Json object
            var listings = JsonConvert.DeserializeObject<List<Listing>>(info);

            return listings;
        }
    }
}