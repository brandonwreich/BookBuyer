using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using BookBuyer.Model;
using Newtonsoft.Json.Linq;

namespace BookBuyer
{
    public class Identifier
    {
        static readonly string key = "3pYHYcs0rIF2KDFDvEq1oQ";

        public static async Task GetBookDetails(List<Listing> listings)
        {
            //Init lists
            List<string> cityList = new List<string> { "Sandy", "Draper", "Millcreek", "Highland", "slc", "South Jordan",
            "Saratoga Springs", "Midvale", "S Jordan", "Alpine", "Cedar Hills", "Bluffdale", "West Jordan", "Salt Lake City",
            "Riverton", "Orem", "Provo", "American Fork" };

            //Init varibles
            decimal totalProfit = 0;
            decimal totalListings = 0;
            decimal totalUnfoundCount = 0;
            decimal totalFoundProfit = 0;
            decimal totalFound = 0;
            decimal totalProblem = 0;
            decimal totalTried = 0;

            //Loop through listings
           foreach(var listing in listings)
            {
                try
                {
                    //Loop through cities
                    foreach(string city in cityList)
                    {
                        var town = listing.City ?? "Unknown";

                        //If book is in a surrounding city
                        if(town.Equals(city, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var httpClient = new HttpClient();
                            var encoded = HttpUtility.UrlEncode(listing.Title);
                            var response = await httpClient.GetAsync($"https://www.goodreads.com/search?q={encoded}&key={key}");
                            var id = "";

                            listing.HighestOffer = 0;

                            if(response.IsSuccessStatusCode)
                            {
                                var raw = await response.Content.ReadAsStringAsync();
                                Regex regex = new Regex(@"(?<=type=\""Book\"">\n    <id type=\""integer\"">)\d*(?=</id>)");
                                id = regex.Match(raw).ToString();
                            }

                            if(id != null && id != "")
                            {
                                var response2 = await httpClient.GetAsync($"https://www.goodreads.com/book/show.xml?id={id}&key={key}");

                                if (response2.IsSuccessStatusCode)
                                {
                                    var raw2 = await response2.Content.ReadAsStringAsync();
                                    Regex titleReg = new Regex(@"(?<=<title>).*?(?=</title)");
                                    Regex isbnReg = new Regex(@"(?<=<isbn><!\[CDATA\[).*?(?=\]\]></isbn)");
                                    Regex isbn13Reg = new Regex(@"(?<=<isbn13><!\[CDATA\[).*?(?=\]\]></isbn13)");
                                    listing.FoundBookTitle = titleReg.Match(raw2).ToString();
                                    listing.Isbn = isbnReg.Match(raw2).ToString();
                                    listing.Isbn13 = isbn13Reg.Match(raw2).ToString();
                                }
                            }

                            var isbn = listing.Isbn ?? listing.Isbn13;

                            if(isbn != null && isbn != "")
                            {
                                isbn = isbn.Replace("-", "");
                                httpClient.DefaultRequestHeaders.Add("authority", "www.bookfinder.com");
                                var response3 = await httpClient.GetAsync($"https://www.bookfinder.com/buyback/affiliate/{isbn}.mhtml");
                                var raw3 = await response3.Content.ReadAsStringAsync();

                                if(response3.IsSuccessStatusCode)
                                {
                                    var jobject = JObject.Parse(raw3);
                                    listing.OfferBookTitle = jobject.GetValue("title").ToString();
                                    var offers = jobject.GetValue("offers");
                                    var test1 = offers.Children();

                                    foreach(var child in offers.Children().ToList())
                                    {
                                        var test = child.Children().FirstOrDefault();

                                        if (test.Value<decimal>("buyback") == 1 && test.Value<decimal>("offer") > listing.HighestOffer)
                                        {
                                            listing.HighestOffer = test.Value<decimal>("offer");
                                            listing.HighestOfferName = child.Path;
                                        }
                                    }
                                }
                            }

                            //Build output
                            string result = $"Listing: {listing.Title}, " +
                                $"Isbn: {listing.Isbn}, " +
                                $"ID: {listing.Id}, " +
                                $"City: {listing.City}, " +
                                $"Price: {listing.Price}, " +
                                $"Offer: {listing.HighestOffer}, " +
                                $"Profit: {listing.HighestOffer - listing.Price}";

                            //If listing is not found
                            if(listing.FoundBookTitle == null)
                            {
                                //Increment unfound count
                                totalUnfoundCount++;
                            }
                            else
                            {
                                //Increment total found
                                totalFound++;
                            }

                            //If listing makes profit
                            if(listing.HighestOffer - listing.Price > 0)
                            {
                                //Write listing
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(result);

                                //Increment total profit and count
                                totalProfit += listing.HighestOffer - listing.Price;
                                totalFoundProfit++;
                            }

                            //Increment total tried
                            totalTried++;
                        }
                    }
                }
                catch(OperationCanceledException x)
                {
                    //If the operation was cancelled
                    if(x.CancellationToken.IsCancellationRequested)
                    {
                        //Build result
                        string result = $"Listing: {listing.Title}, " +
                        $"ID: {listing.Id}, " +
                        $"City: {listing.City}, " +
                        $"Price: {listing.Price}, " +
                        $"Isbn: {listing.Isbn}, " +
                        $"Isbn13: {listing.Isbn13}";

                        //Print result
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("");
                        Console.WriteLine(result);
                        Console.WriteLine("Cancellation Token was called");
                    }
                    else
                    {
                        //Build result
                        string result = $"Listing: {listing.Title}, " +
                        $"ID: {listing.Id}, " +
                        $"City: {listing.City}, " +
                        $"Price: {listing.Price}, " +
                        $"Isbn: {listing.Isbn}, " +
                        $"Isbn13: {listing.Isbn13}";

                        //Print result
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("");
                        Console.WriteLine(result);
                        Console.WriteLine("Timeout error");
                    }
                }
                catch(Exception x)
                {
                    //Build result
                    string result = $"Listing: {listing.Title}, " +
                    $"ID: {listing.Id}, " +
                    $"City: {listing.City}, " +
                    $"Price: {listing.Price}, " +
                    $"Isbn: {listing.Isbn}, " +
                    $"Isbn13: {listing.Isbn13}";

                    //Print result
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("");
                    Console.WriteLine(result);
                    Console.WriteLine(x);
                    Console.WriteLine("");

                    //Increment total problem count
                    totalProblem++;
                }

                //Increment total listings
                totalListings++;
            }

            //Caclculate percentages
            decimal totalFoundProfitPercentage = Math.Round((totalFoundProfit / totalFound) * 100, 2);
            decimal totalProblemPercentage = Math.Round((totalProblem / totalFound) * 100, 2);
            decimal totalFoundPercentage = Math.Round((totalFound / totalTried) * 100, 2);
            decimal totalUnfoundPercentage = Math.Round((totalUnfoundCount / totalTried) * 100, 2);
            decimal totalTriedPercentage = Math.Round((totalTried / totalListings) * 100, 2);

            //Write total profit
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("Total profit: $" + totalProfit);
            Console.WriteLine("");

            //Write analytics
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Total number of profitable listings: " + totalFoundProfit + "(" + totalFoundProfitPercentage + "%)");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Total number of problem listings: " + totalProblem + "(" + totalProblemPercentage + "%)");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Total number of listings found: " + totalFound + "(" + totalFoundPercentage + "%)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Total number of books not found: " + totalUnfoundCount + "(" + totalUnfoundPercentage + "%)");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Total number of listings tried: " + totalTried + "(" + totalTriedPercentage + "%)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Total number of listings collected: " + totalListings);
        }
    }
}