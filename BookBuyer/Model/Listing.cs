namespace BookBuyer.Model
{
    public class Listing
    {
        //Title
        public string Title { get; set; }

        //ID
        public int Id { get; set; }
      
        //Price
        public decimal Price { get; set; }
      
        //City
        public string City { get; set; }

        //Name of Vendor
        public string Name { get; set; }

        //Email of Vendor
        public string EmailCanonical { get; set; }

        //Title of found book
        public string FoundBookTitle { get; set; }

        //Offer for book
        public string OfferBookTitle {get; set;}

        //Author of book
        public string Author { get; set; }

        //Isbn of book
        public string Isbn { get; set; }

        //Isbn13 of book
        public string Isbn13 { get; set; }

        //Highest offer for book
        public decimal HighestOffer { get; set;}

        //Vendor giving highest offer
        public string HighestOfferName {get; set;}
    }
}