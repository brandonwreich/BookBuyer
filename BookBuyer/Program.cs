using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BookBuyer
{
    class Program
    {
        //Declare variables
        static IWebDriver driver;

        static void Main(string[] args)
        {
            //Init varibales
            driver = new ChromeDriver("C:\\Users\\brand\\source\\repos\\BookBuyer");

            //Init pages
            Navigation navigationPage = new Navigation();
            GatherInformation infoGrabbingPage = new GatherInformation();

            //Navigate to KSL page
            navigationPage.NavigateToKslBooksPage(driver);
            navigationPage.MaximizeBrower(driver);

            //Grab the string of all the books
            infoGrabbingPage.GrabBookInfo(driver);

            //Navigate to Book Finder page
            navigationPage.NewTab(driver);
            navigationPage.NavigateToBookFinderPage(driver);

            //Exit Browser
            navigationPage.ExitBrowser(driver);
        }
    }
}