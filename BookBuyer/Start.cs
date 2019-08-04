using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
            string[] bookTitles = new string[] { "Mindware: An introduction to the philosophy of Cognitive Science",
            "Studio Companion Series Design Basics: 1st Edition" };
            string[] bookIsbn = new string[1000];
            int count = 0;

            //Init pages
            Navigation navigationPage = new Navigation();
            GatherInformation infoGrabbingPage = new GatherInformation();

            //Navigate to KSL page
            navigationPage.NavigateToKslBooksPage(driver);
            navigationPage.MaximizeBrower(driver);

            //Navigate to ISBN search page
            navigationPage.NewTab(driver);
            navigationPage.NavigateToIsbnSearchPage(driver);

            //Find ISBN **BROKEN**
            while (count < bookTitles.Length)
            {
                bookIsbn[count] = infoGrabbingPage.FindIsbn(driver, bookTitles[count], "");


                count++;
            }

            //Grab the string of all the books **BROKEN**
            //       infoGrabbingPage.GrabBookInfo(driver);

            //Navigate to Book Finder page
            navigationPage.NewTab(driver);
            navigationPage.NavigateToBookFinderPage(driver);

            while (count <= bookIsbn.Length)
            {
                bookIsbn[count].Replace(bookIsbn[count], null);
            }

            //Exit Browser
            navigationPage.ExitBrowser(driver);
        }
    }
}