namespace BookBuyer.Model
{
    public class Listing
    {
        //Grabs the title of the ad
        public string Title { get; set; }
      
        public decimal Price { get; set; }
      
        public string City { get; set; }

        //Grabs the name of the vendor
        public string Name { get; set; }

        //Grabs the home phone number of the vendor
        public string HomeNumber { get; set; }

        //Grabs the cell phone number of the vendor
        public string CellNumber { get; set; }

        //Grabs the email of the vendor
        public string EmailCanonical { get; set; }
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