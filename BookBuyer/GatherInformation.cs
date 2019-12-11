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
            IWebElement bookInformation = null;
            string text = "";
            int attempts = 0;

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);

            //Grab information
            while (bookInformation == null || attempts < 100)
            {
                try
                {
                    bookInformation = driver.FindElement(By.XPath("//script[contains(.,'window.renderSearchSection')]"));
                    attempts += 100;
                }
                catch (NoSuchElementException) { }

                attempts++;
            }

            attempts = 0;

            while (text.Equals("") || attempts < 100)
            {
                try
                {
                    text = bookInformation.GetAttribute("innerText");
                    attempts += 100;
                }
                catch (StaleElementReferenceException) { }

                attempts++;

                if (attempts == 100)
                {
                    text = "Skip";
                }
            }

            attempts = 0;

            //Grab the wanted string
            var info = regex.Match(text).ToString();

            //Convert to Json object
            var listings = JsonConvert.DeserializeObject<List<Listing>>(info);

            return listings;
        }
    }
}