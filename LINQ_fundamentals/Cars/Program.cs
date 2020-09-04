using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

            var top = cars
                .Where(c => c.Manufacturer == "asdads" && c.Year == 2016)
                .OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name)
                .Select(c => c)
                .FirstOrDefault();

            //Console.WriteLine(top?.Name);

            var result = cars.Any(c => c.Manufacturer == "Ford");
            //cars.All(c => c.Manufacturer == "Ford");
            //cars.Contains(...)


            var result2 = cars
                .SelectMany(c => c.Name)
                .OrderBy(c => c);
            foreach (var character in result2)
            {
                Console.WriteLine(character);
            }

            // QUERIES
            Console.WriteLine("\n*** QUERIES ***\n");
            //var query = cars.OrderByDescending(c => c.Combined)
            //                .ThenBy(c => c.Name);
            var query = from car in cars
                        where car.Manufacturer == "BMW" && car.Year == 2016
                        orderby car.Combined descending, car.Name ascending
                        select new
                        {
                            car.Manufacturer,
                            car.Name,
                            car.Combined
                        };


            var query2 = cars
                .Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                .OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name)
                .Select(c => c);
                //.Select(c => new
                //{
                //    c.Manufacturer,
                //    c.Name,
                //    c.Combined
                //});

            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            var query =
            File.ReadAllLines(path)
                .Skip(1)
                .Where(line => line.Length > 1)
                .ToCar();
            //.Select(line => Car.ParseFromCsv(line));


            //var query =
            //from line in File.ReadAllLines(path).Skip(1)
            //where line.Length > 1
            //select Car.ParseFromCsv(line);
            return query.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {

            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new Car
                {
                    Year = Int32.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = Double.Parse(columns[3]),
                    Cylinders = Int32.Parse(columns[4]),
                    City = Int32.Parse(columns[5]),
                    Highway = Int32.Parse(columns[6]),
                    Combined = Int32.Parse(columns[7])
                };
            }
        }
    }
}
