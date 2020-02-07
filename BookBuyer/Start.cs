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
            //Init driver
            try
            {
                driver = new ChromeDriver(Directory.GetCurrentDirectory());
            }
            catch (InvalidOperationException)
            {
                driver = new ChromeDriver(@"C:\Users\knigh\source\repos\BookBuyer");
            }

            //Init lists
            List<Listing> pageListings = new List<Listing>();
            List<string> Websites = new List<string> {"https://classifieds.ksl.com/s/Books+and+Media/Books:+Education+and+College", 
                "https://classifieds.ksl.com/s/Books+and+Media/Books:+Religious", 
                "https://classifieds.ksl.com/s/Books+and+Media/Books:+Non-fiction", 
                "https://classifieds.ksl.com/s/Books+and+Media/Books:+Fiction", 
                "https://classifieds.ksl.com/s/Books+and+Media/Books:+Children"};

            //Init pages
            Navigation navigationPage = new Navigation();
            GatherInformation infoGrabbingPage = new GatherInformation();

            //Init variables
            int caseSwitch = 1;

            //Loop through websites
            foreach (var website in Websites)
            {
                //Init variables
                int pageCount = 1;
                string xPath = "";

                switch(caseSwitch)
                {
                    case 1:
                       xPath = "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Education+and+College&page=";
                        break;
                    case 2:
                        xPath = "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Religious&page=";
                        break;
                    case 3:
                        xPath = "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Non-fiction&page=";
                        break;
                    case 4:
                        xPath = "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Fiction&page=";
                        break;
                    case 5:
                        xPath = "/search?category%5B0%5D=Books+and+Media&subCategory%5B0%5D=Books%3A+Children&page=";
                        break;
                }

                //Navigate to KSL page
                navigationPage.NavigateToKslBooksPage(driver, website);
                navigationPage.MaximizeBrower(driver);

                //Wait 5 seconds
                try
                {
                    driver.WaitToBeReady(By.XPath(""), 5);
                }
                catch (Exception) { }

                //While there is stil a next page
                while (driver.IsNextButtonEnabled(pageCount, xPath))
                {
                    //Grab book information
                    pageListings.AddRange(infoGrabbingPage.GrabBookInfo(driver));

                    //Go to next page and increase pageCount
                    navigationPage.NextKslPage(driver, pageCount, xPath);
                    pageCount++;

                    //Wait for 3 Seconds
                    try
                    {
                        driver.WaitToBeReady(By.XPath(""), 3);
                    }
                    catch (Exception) { }
                }

                caseSwitch++;
            }

            //Compare prices
            Console.Clear();
            Task.WaitAll(Identifier.GetBookDetails(pageListings));

            //Exit Browser
            navigationPage.ExitBrowser(driver);
        }
    }
}