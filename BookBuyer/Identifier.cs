using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
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
            //Init lists
            List<String> cityList = new List<string> { "Sandy", "Draper", "Millcreek", "Highland", "slc", "South Jordan",
            "Saratoga Springs", "Midvale", "S Jordan", "Alpine", "Cedar Hills", "Bluffdale", "West Jordan", "Salt Lake City",
            "Riverton", "Orem", "Provo", "American Fork" };
            List<Listing> notFoundList = new List<Listing>();

            //Init varibles
            decimal totalProfit = 0;
            int unfoundCount = 0;
            int listingCount = 0;
            file.AutoFlush = true;

            foreach (var listing in listings)
            {
                //Loop through cities
                foreach (string city in cityList)
                {
                    //If book is in a surrounding city
                    if (listing.City.Equals(city, StringComparison.InvariantCultureIgnoreCase))
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

                        var isbn = listing.Isbn ?? listing.Isbn13;

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
                            $"ID: {listing.Id}, " +
                            $"City: {listing.City}, " +
                            $"Price: {listing.Price}, " +
                            $"Offer: {listing.HighestOffer}, " +
                            $"Profit:{listing.HighestOffer - listing.Price}";

                        //If listing is not found
                        if (listing.FoundBookTitle == null)
                        {
                            //If price is low
                            if (listing.Price <= 20)
                            {
                                //Add to list
                                notFoundList.Add(listing);
                            }

                            //Increment count
                            unfoundCount++;
                        }

                        //If listing makes profit
                        if (listing.HighestOffer - listing.Price > 0)
                        {
                    
                            //Write listing
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine(result);
                            file.WriteLine(result);

                            //If profit is higher than $50
                            if (listing.HighestOffer - listing.Price > 50)
                            {
                                //Compose and send email
                                SendEmail(listing.EmailCanonical, listing.Name, listing);
                            }

                            //Increment total profit
                            totalProfit += listing.HighestOffer - listing.Price;
                        }

                        listingCount++;
                    }
                }
            }

            //Write total profit
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine(totalProfit);
            Console.WriteLine("");

            //Loop through unfound listings
            foreach (var listing in notFoundList)
            {
                string notFound = $"Listing: {listing.Title}, " +
                    $"ID: {listing.Id}, " +
                    $"Isbn: {listing.Isbn}, " +
                    $"City: {listing.City}, " +
                    $"Price: {listing.Price}";

                //Write unfound listings
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(notFound);
            }

            //Write the total number of unfound listings
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine(unfoundCount);

            //Write total number of listings
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(listingCount);
        }

        //Sends an email to the vendor
        public static void SendEmail(string emailAddress, string name, Listing listing)
        {
            //Init varibles
            var fromAddress = new MailAddress("reichb53@gmail.com", "Brandon Reich");
            var toAddress = new MailAddress(emailAddress, name);
            const string fromPassword = "Buyb00ks";
            const string subject = "KSL Ad";
            string body;

            //If the listing is free
            if (listing.Price == 0)
            {
                body = "I saw your ad on KSL for '" + listing.Title + "' and was interested in it. My only " +
                    "question is if it is really free or not. Let me know if you had a price in mind. You can reach me at (801) 357-9780.";
            }
            else
            {
                body = "I saw your ad on KSL for '" + listing.Title + "' and was interested in buying it. " +
                    "I have $" + listing.Price + " in cash and can meet anytime. You can reach me on my mobile phone at " +
                    "(801) 357-9780 to arrange a meeting place.";
            }

            //Init Smtp
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                //Set subject and body
                Subject = subject,
                Body = body
            })
            {
                // Send email
                smtp.Send(message);
                Console.WriteLine("Email successfully sent to " + listing.Name);
            }
        }
    }
}