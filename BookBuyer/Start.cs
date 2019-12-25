using BookBuyer.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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
            //Init varibles
            try
            {
                driver = new ChromeDriver(Directory.GetCurrentDirectory());
            }
            catch(InvalidOperationException)
            {
                driver = new ChromeDriver(@"C:\Users\knigh\source\repos\BookBuyer");
            }

            List<Listing> pageListings = new List<Listing>();
            int pageCount = 1;

            //Init pages
            Navigation navigationPage = new Navigation();
            GatherInformation infoGrabbingPage = new GatherInformation();

            //Navigate to KSL page
            navigationPage.NavigateToKslBooksPage(driver);
            navigationPage.MaximizeBrower(driver);

            //While there is stil a next page
            while (pageCount < 83)
            {
                //Grab book information
                pageListings.AddRange(infoGrabbingPage.GrabBookInfo(driver));

                //Go to next page and increase pageCount
                navigationPage.NextKslPage(driver, pageCount);
                pageCount++;

                //Wait for 5 Seconds
                try
                {
                    driver.WaitToBeReady(By.XPath(""), 5);
                }
                catch (Exception e) { }
            }

            Task.WaitAll(Identifier.GetBookDetails(pageListings));
        
            //Exit Browser
            navigationPage.ExitBrowser(driver);
        }
    }
}