using BookBuyer.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BookBuyer
{
    class GatherInformation
    {
        public List<Listing> GrabBookInfo(string text)
        {
            //Init variables
            Regex regex = new Regex(@"\[{.*}\]");

            //Grab the wanted string
            var info = regex.Match(text).ToString();

            //Convert to Json object
            var listings = JsonConvert.DeserializeObject<List<Listing>>(info);

            return listings;
        }
    }
}