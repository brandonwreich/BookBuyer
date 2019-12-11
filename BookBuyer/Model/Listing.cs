namespace BookBuyer.Model
{
    public class Listing
    {
        //Title
        public string Title { get; set; }
      
        //Price
        public decimal Price { get; set; }
      
        //City
        public string City { get; set; }

        //Name of Vendor
        public string Name { get; set; }

        public string FoundBookTitle { get; set; }
        public string OfferBookTitle {get; set;}
        public string Author { get; set; }
        public string Isbn { get; set; }
        public string Isbn13 { get; set; }
        int Confidence { get; set; }
        public decimal HighestOffer { get; set;}
        public string HighestOfferName {get; set;}
    }
}