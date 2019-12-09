using BookBuyer.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BookBuyer
{
    class Start
    {
        //Declare variables
        static IWebDriver driver;

        static void Main(string[] args)
        {
            //Init varibales
            driver = new ChromeDriver(Directory.GetCurrentDirectory());
            List<Listing> pageListings = new List<Listing>();
        
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
                while (pageCount<11)
                {
                    //Grab book information
                    pageListings.AddRange(infoGrabbingPage.GrabBookInfo(driver));

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

            
            Task.WaitAll(Identifier.GetBookDetails(pageListings));
            //Exit Browser
            navigationPage.ExitBrowser(driver);
        }
    }
}