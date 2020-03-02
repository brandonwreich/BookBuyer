using BookBuyer.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBuyer
{
    class Start
    {
        static void Main(string[] args)
        {
            //Init lists
            List<Listing> pageListings = new List<Listing>();
            List<string> websites = new List<string> { "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Education+and+College&page=",
                "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Religious&page=",
                "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Non-fiction&page=",
                "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Fiction&page=",
                "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Children&page=" };

            //Init pages
            GatherInformation infoGrabbingPage = new GatherInformation();

            //Loop through website list
            foreach(string website in websites)
            {
                //Update console
                Console.WriteLine("Grabbing data...");

                //Init variables
                int pageNumber = 0;

                var url = website + pageNumber;
                var web = new HtmlWeb();
                var doc = web.Load(url);

                int maxPages = FindMaxPageNumber(doc);

                //While there are still listings
                while(pageNumber <= maxPages)
                {
                    try
                    {
                        //Init variables
                        url = website + pageNumber;
                        web = new HtmlWeb();
                        doc = web.Load(url);

                        //Find listing information
                        var value = doc.DocumentNode.SelectNodes("//script[contains(.,'window.renderSearchSection')]").First().GetDirectInnerText();
                        value = value.Substring(value.IndexOf("listings: ") + 10);

                        pageListings.AddRange(infoGrabbingPage.GrabBookInfo(value));

                    }
                    catch (Exception x)
                    {
                        //Update console
                        Console.WriteLine("Failure to grab data on page " + pageNumber + ". " + x.Message);
                    }

                    //Increment page number
                    pageNumber++;
                }
            }

            //Compare book prices
            Console.WriteLine("");
            Console.WriteLine("Comparing pricing...");
            Console.WriteLine("");
            Task.WaitAll(Identifier.GetBookDetails(pageListings));
        }

        //Helper method to find max number of pages
        public static int FindMaxPageNumber(HtmlDocument doc)
        {
            //Find max number of pages
            var value = doc.DocumentNode.SelectNodes("//*[@title='Go to last page']").First().GetDirectInnerText();

            return Convert.ToInt32(value);
        }
    }
}