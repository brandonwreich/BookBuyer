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

            //Grab information
            IWebElement bookInformation = driver.FindElement(By.XPath("//script[contains(.,'window.renderSearchSection')]"));
            var text = bookInformation.GetAttribute("innerText");

            //Grab the wanted string
            var info = regex.Match(text).ToString();

            //Convert to Json object
            var wantedInfo = JsonConvert.DeserializeObject<List<Listing>>(info);

            return wantedInfo;
        }
    }
}