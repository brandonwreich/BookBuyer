namespace BookBuyer.Model
{
    public class Listing
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string HomeNumber { get; set; }
        public string CellNumber { get; set; }
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