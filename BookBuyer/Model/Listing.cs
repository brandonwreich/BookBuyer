namespace BookBuyer.Model
{
    class Listing
    {
        //Grabs the title of the ad
        public string Title { get; set; }

        //Grabs the price of the book(s)
        public decimal Price { get; set; }

        //Grabs the city location of the vendor
        public string City { get; set; }

        //Grabs the name of the vendor
        public string Name { get; set; }

        //Grabs the home phone number of the vendor
        public string HomeNumber { get; set; }

        //Grabs the cell phone number of the vendor
        public string CellNumber { get; set; }

        //Grabs the email of the vendor
        public string EmailCanonical { get; set; }
    }
}