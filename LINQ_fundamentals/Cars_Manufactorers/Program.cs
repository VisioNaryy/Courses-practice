using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars_Manufactorers
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufactorers("manufacturers.csv");

            var query = 
                from car in cars
                join manufactorer in manufacturers
                    on car.Manufacturer equals manufactorer.Name
                orderby car.Combined descending, car.Name ascending
                select new
                {
                    manufactorer.Headquarters,
                    car.Name,
                    car.Combined
                };

            //it's more efficient to use manufactorers as an inner sequence, because it's more cars than manufactorers
            var query2 =
                cars.Join(manufacturers,
                            c => c.Manufacturer,
                            m => m.Name,
                            (c, m) => new
                            {
                                m.Headquarters,
                                c.Name,
                                c.Combined
                                //Car = c,
                                //Manufactorer = m
                            })
                .OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name);

            var query3 =
                cars.Join(manufacturers,
                            c => c.Manufacturer,
                            m => m.Name,
                            (c, m) => new
                            {
                                Car = c,
                                Manufactorer = m
                            })
                .OrderByDescending(c => c.Car.Combined)
                .ThenBy(c => c.Car.Name)
                .Select(c => new
                {
                    c.Manufactorer.Headquarters,
                    c.Car.Name,
                    c.Car.Combined
                });
            //joining more than 1 column
            var query4 =
                from car in cars
                join manufactorer in manufacturers
                    on new { car.Manufacturer, car.Year }
                    equals
                    new { Manufacturer = manufactorer.Name, manufactorer.Year }
                orderby car.Combined descending, car.Name ascending
                select new
                {
                    manufactorer.Headquarters,
                    car.Name,
                    car.Combined
                };
            //for each manufactorer cw 2 of the most efficient 
            var query6 =
                from car in cars
                group car by car.Manufacturer.ToUpper() into manufacturer
                orderby manufacturer.Key
                select manufacturer;
            var query7 =
                cars.GroupBy(c => c.Manufacturer.ToUpper())
                .OrderBy(g => g.Key);


            foreach (var group in query6)
            {
                Console.WriteLine(group.Key);
                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
            //grouping and joining cars and manufactorers to cw country name
            var query8 =
                from manufacturer in manufacturers
                join car in cars on manufacturer.Name equals car.Manufacturer
                    into carGroup
                select new
                {
                    Manufacturer = manufacturer,
                    Cars = carGroup
                };

            var query9 =
                manufacturers
                .GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
                        new
                        {
                            Manufacturer = m,
                            Cars = g
                        })
                .OrderBy(m => m.Manufacturer.Name);

            //foreach (var group in query8)
            //{
            //    Console.WriteLine($"{group.Manufacturer.Name} : {group.Manufacturer.Headquarters}");
            //    foreach (var car in group.Cars.OrderByDescending(c => c.Combined).Take(2))
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }
            //}

            //top 3 fuel-efficient cars by country
            var query10 =
                from manufacturer in manufacturers
                join car in cars on manufacturer.Name equals car.Manufacturer
                    into carGroup
                select new
                {
                    Manufacturer = manufacturer,
                    Cars = carGroup
                } into result
                group result by result.Manufacturer.Headquarters;
            var query11 =
                manufacturers
                .GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
                        new
                        {
                            Manufacturer = m,
                            Cars = g
                        })
                .GroupBy(m => m.Manufacturer.Headquarters);


            //foreach (var group in query10)
            //{
            //    Console.WriteLine($"{group.Key}");
            //    foreach (var car in group.SelectMany(g=>g.Cars)
            //                             .OrderByDescending(c => c.Combined)
            //                             .Take(2))
            //    {
            //        Console.WriteLine($"\t{car.Name} : {car.Combined}");
            //    }
            //}

            //
            var query12 =
                from car in cars
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Avg = carGroup.Average(c => c.Combined)
                } into result
                orderby result.Max descending
                select result;
            //we can also loop once through the dataset to perform all 3 calculations by Aggregate() method and custom class 
            var query13 =
                cars.GroupBy(c => c.Manufacturer)
                .Select(g =>
               {
                   var results = g.Aggregate(new CarStatistics(),
                       (acc, c) => acc.Accumulate(c),
                       acc => acc.Compute());
                   return new
                   {
                       Name = g.Key,
                       Avg = results.Average,
                       Min = results.Min,
                       Max = results.Max
                   };
               })
                .OrderByDescending(r => r.Max);



            foreach (var result in query13)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\t Max: {result.Max}");
                Console.WriteLine($"\t Min: {result.Min}");
                Console.WriteLine($"\t Avg: {result.Avg}");
            }

            









        }


        //custom Aggregation class
        public class CarStatistics
        {
            public int Max { get; set; }
            public int Min { get; set; }
            public int Total { get; set; }
            public int Count { get; set; }
            public double Average { get; set; }
            public CarStatistics()
            {
                Max = Int32.MinValue;
                Min = Int32.MaxValue;
            }
            public CarStatistics Accumulate(Car car)
            {
                Count = +1;
                Total += car.Combined;
                Max = Math.Max(Max, car.Combined);
                Min = Math.Min(Max, car.Combined);

                return this;
            }
            public CarStatistics Compute()
            {
                Average = Total / Count;
                return this;
            }

        }



        private static List<Manufacturer> ProcessManufactorers(string path)
        {
            var query =
                File.ReadAllLines(path)
                .Where(l => l.Length > 1)
                .Select(l =>
                {
                    var columns = l.Split(',');
                    return new Manufacturer
                    {
                        Name = columns[0],
                        Headquarters = columns[1],
                        Year = Int32.Parse(columns[2])
                    };
                });
            return query.ToList();
        }

        private static List<Car> ProcessCars(string path)
        {
            var query =
            File.ReadAllLines(path)
                .Skip(1)
                .Where(line => line.Length > 1)
                .ToCar();

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
