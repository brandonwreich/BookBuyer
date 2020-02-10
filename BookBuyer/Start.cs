using ArbitrageBot;
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

            //Init pages
            GatherInformation infoGrabbingPage = new GatherInformation();
            HelperMethods helperMethods = new HelperMethods();

            //Init variables
            int collegeTextbookMaxPages = helperMethods.GetCollegeTextbookPageNumber();
            int religiousMaxPages = helperMethods.GetReligiousMaxPageNumber();
            int nonFictionMaxPages = helperMethods.GetNonfictionMaxPageNumnber();
            int fictionMaxPages = helperMethods.GetFictionMaxPageNumber();
            int childrenMaxPages = helperMethods.GetChildrenMaxPageNumber();

            int pageNumber = 0;

            while (pageNumber <= collegeTextbookMaxPages)
            {
                var url = "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Education+and+College&page={pageNumber}";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var value = doc.DocumentNode.SelectNodes("//script[contains(.,'window.renderSearchSection')]").First().GetDirectInnerText();
                pageListings.AddRange(infoGrabbingPage.GrabBookInfo(value));

                pageNumber++;
            }

            pageNumber = 0;

            while (pageNumber <= religiousMaxPages)
            {
                var url = "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Religious&page={pageNumber}";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var value = doc.DocumentNode.SelectNodes("//script[contains(.,'window.renderSearchSection')]").First().GetDirectInnerText();
                pageListings.AddRange(infoGrabbingPage.GrabBookInfo(value));

                pageNumber++;
            }

            pageNumber = 0;

            while (pageNumber <= nonFictionMaxPages)
            {
                var url = "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Non-fiction&page={pageNumber}";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var value = doc.DocumentNode.SelectNodes("//script[contains(.,'window.renderSearchSection')]").First().GetDirectInnerText();
                pageListings.AddRange(infoGrabbingPage.GrabBookInfo(value));

                pageNumber++;
            }

            pageNumber = 0;

            while (pageNumber <= fictionMaxPages)
            {
                var url = "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Fiction&page={pageNumber}";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var value = doc.DocumentNode.SelectNodes("//script[contains(.,'window.renderSearchSection')]").First().GetDirectInnerText();
                pageListings.AddRange(infoGrabbingPage.GrabBookInfo(value));

                pageNumber++;
            }

            pageNumber = 0;

            while (pageNumber <= childrenMaxPages)
            {
                var url = "https://classifieds.ksl.com/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Children&page={pageNumber}";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var value = doc.DocumentNode.SelectNodes("//script[contains(.,'window.renderSearchSection')]").First().GetDirectInnerText();
                pageListings.AddRange(infoGrabbingPage.GrabBookInfo(value));

                pageNumber++;
            }

            Console.WriteLine("");
            Task.WaitAll(Identifier.GetBookDetails(pageListings));
        }
    }
}