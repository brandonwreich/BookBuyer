using BookBuyer.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
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

            //Find information
            IWebElement bookInformation = driver.FindElement(By.XPath("//script[contains(.,'window.renderSearchSection')]"));

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