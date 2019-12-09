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
            file.AutoFlush = true;
            
            foreach(var listing in listings)
            {
                var httpClient = new HttpClient();
                var encoded = HttpUtility.UrlEncode(listing.Title);
                var response = await httpClient.GetAsync($"https://www.goodreads.com/search?q={encoded}&key={key}");
                var id = "";
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
                
                var isbn = listing.Isbn == null? listing.Isbn13: listing.Isbn;
                listing.HighestOffer = 0;
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
                var result = $"Listing: {listing.Title}, Found: {listing.FoundBookTitle?? "NOT FOUND"}, OfferTitle:{listing.OfferBookTitle}, Price: {listing.Price}, Offer: {listing.HighestOffer}, Profit:{listing.HighestOffer-listing.Price}";
                Console.WriteLine(result);
                file.WriteLine(result);

                
            }
        }
    }
}