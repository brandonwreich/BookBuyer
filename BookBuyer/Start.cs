using BookBuyer.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace BookBuyer
{
    class Start
    {
        //Declare variables
        static IWebDriver driver;

        static void Main(string[] args)
        {
            //Init varibales
            driver = new ChromeDriver("C:\\Users\\knigh\\source\\repos\\BookBuyer");
            List<List<Listing>> pageListings = new List<List<Listing>>();
            int pageCount = 1;
            int count = 0;

            //Init pages
            Navigation navigationPage = new Navigation();
            GatherInformation infoGrabbingPage = new GatherInformation();

            //Navigate to KSL page
            navigationPage.NavigateToKslBooksPage(driver);
            navigationPage.MaximizeBrower(driver);

            //Find next page link
            IWebElement nextLink = navigationPage.FindNextLink(driver, pageCount);

            //Grab the string of all the books on all pages
            try
            {
                //While there is stil a next page
                while (nextLink.Enabled)
                {
                    //Grab book information
                    pageListings.Insert(count, infoGrabbingPage.GrabBookInfo(driver));

                    //Go to next page and increase pageCount
                    navigationPage.NextKslPage(driver, pageCount);
                    pageCount++;

                    //Refind nextLink
                    nextLink = navigationPage.FindNextLink(driver, pageCount);

                    //Increase count
                    count++;
                }
            }
            catch (NoSuchElementException) { }

            //Exit Browser
            navigationPage.ExitBrowser(driver);
        }
    }
}