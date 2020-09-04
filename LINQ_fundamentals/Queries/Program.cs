using System;
using System.Collections.Generic;
using System.Linq;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10);
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }


            List<Movie> movies = new List<Movie>
            {
                new Movie{Title="The Dark Knight", Rating = 8.9f, Year=2008},
                new Movie{Title="The King's speech", Rating = 8.0f, Year=2010},
                new Movie{Title="Casablanca", Rating = 8.5f, Year=1942},
                new Movie{Title="Star Wars V", Rating = 8.7f, Year=1980}
            };

            var query = movies.Where(m => m.Year > 2000);
            //Create a new extension method called "Filter" 
            var query1 = movies.Filter(m => m.Year > 2000);

            foreach (var q in query1)
            {
                Console.WriteLine(q.Title);
            }

            var query2 = from movie in movies
                        where movie.Year > 2000
                        orderby movie.Rating descending
                        select movie;

            var enumerator = query2.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }

        }
    }
}
