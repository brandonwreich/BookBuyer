using System;
using System.Collections.Generic;
using System.IO;
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
        static StreamWriter file = File.CreateText("./results.txt");
        static string key = "3pYHYcs0rIF2KDFDvEq1oQ";

        public static async Task GetBookDetails(List<Listing> listings)
        {
            List<String> cityList = new List<string> { "Sandy", "Draper", "Millcreek", "Highland", "slc", "South Jordan",
            "Saratoga Springs", "Midvale", "S Jordan", "Alpine", "Cedar Hills", "Bluffdale", "West Jordan", "Salt Lake City",
            "Riverton", "Orem", "Provo", "American Fork" };
            List<Listing> notFoundList = new List<Listing>();

            decimal totalProfit = 0;
            file.AutoFlush = true;

            foreach (var listing in listings)
            {
                var httpClient = new HttpClient();
                var encoded = HttpUtility.UrlEncode(listing.Title);
                var response = await httpClient.GetAsync($"https://www.goodreads.com/search?q={encoded}&key={key}");
                var id = "";

                listing.HighestOffer = 0;

                if (response.IsSuccessStatusCode)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    Regex regex = new Regex(@"(?<=type=\""Book\"">\n    <id type=\""integer\"">)\d*(?=</id>)");
                    id = regex.Match(raw).ToString();
                }

                if (id != null && id != "")
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

                var isbn = listing.Isbn == null ? listing.Isbn13 : listing.Isbn;

                if (isbn != null && isbn != "")
                {
                    isbn = isbn.Replace("-", "");
                    httpClient.DefaultRequestHeaders.Add("authority", "www.bookfinder.com");
                    var response3 = await httpClient.GetAsync($"https://www.bookfinder.com/buyback/affiliate/{isbn}.mhtml");
                    var raw3 = await response3.Content.ReadAsStringAsync();

                    if (response3.IsSuccessStatusCode)
                    {
                        var jobject = JObject.Parse(raw3);
                        listing.OfferBookTitle = jobject.GetValue("title").ToString();
                        var offers = jobject.GetValue("offers");
                        var test1 = offers.Children();

                        foreach (var child in offers.Children().ToList())
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

                string result = $"Listing: {listing.Title}, " +
                    $"Isbn: {listing.Isbn}, " +
                    $"City: {listing.City}, " +
                    $"Price: {listing.Price}, " +
                    $"Offer: {listing.HighestOffer}, " +
                    $"Profit:{listing.HighestOffer - listing.Price}";

                //If listing is not found
                if (listing.FoundBookTitle == null)
                {
                    //Add to list
                    notFoundList.Add(listing);
                }

                //If listing makes profit
                if (listing.HighestOffer - listing.Price > 0)
                {
                    foreach (string city in cityList)
                    {
                        //If book is in a surrounding city
                        if (listing.City.Equals(city, StringComparison.InvariantCultureIgnoreCase))
                        {
                            //Write listing
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine(result);
                            file.WriteLine(result);

                            totalProfit += listing.HighestOffer - listing.Price;
                        }
                    }
                }
            }

            //Write total profit
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine(totalProfit);
            Console.WriteLine("");

            //Write the listings that weren't found
            foreach (var listing in notFoundList)
            {
                string notFound = $"Listing: {listing.Title}, " +
                    $"Isbn: {listing.Isbn}, " +
                    $"City: {listing.City}, " +
                    $"Price: {listing.Price}";

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(notFound);
            }

            //Write the total number of listings
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine(notFoundList.Count);
        }
    }
}