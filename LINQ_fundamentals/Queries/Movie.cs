using System;
using System.Collections.Generic;
using System.Text;

namespace Queries
{
    public class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }

        private int _year;
        public int Year
        {
            get 
            {
                //every time this get is called, invoke cw
                Console.WriteLine($"Returning {_year} : {Title}");
                return _year;
            }
            set
            {
                _year = value;
            }
        }
    }
}
