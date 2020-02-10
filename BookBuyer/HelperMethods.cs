using System;
using System.Collections.Generic;
using System.Text;

namespace ArbitrageBot
{
    public class HelperMethods
    {
        public int GetCollegeTextbookPageNumber()
        {
            //Grab number of pages
            Console.WriteLine("How many pages of college text book listings are there?");
            string pageNumber = Console.ReadLine();

            return Convert.ToInt32(pageNumber);
        }

        public int GetReligiousMaxPageNumber()
        {
            //Grab number of pages
            Console.WriteLine("How many pages of religious book listings are there?");
            string pageNumber = Console.ReadLine();

            return Convert.ToInt32(pageNumber);
        }

        public int GetNonfictionMaxPageNumnber()
        {
            //Grab number of pages
            Console.WriteLine("How many pages of non-fiction book listings are there?");
            string pageNumber = Console.ReadLine();

            return Convert.ToInt32(pageNumber);
        }

        public int GetFictionMaxPageNumber()
        {
            //Grab number of pages
            Console.WriteLine("How many pages of fiction book listings are there?");
            string pageNumber = Console.ReadLine();

            return Convert.ToInt32(pageNumber);
        }

        public int GetChildrenMaxPageNumber()
        {
            //Grab number of pages
            Console.WriteLine("How many pages of children book listings are there?");
            string pageNumber = Console.ReadLine();

            return Convert.ToInt32(pageNumber);
        }
    }
}
