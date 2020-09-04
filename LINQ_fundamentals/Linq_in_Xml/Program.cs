using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Linq_to_Xml
{
    class Program
    {
        static void Main(string[] args)
        {
            //var records = ProcessCars("fuel.csv");
            //var document = new XDocument();
            //var cars = new XElement("Cars");
            //foreach (var record in records)
            //{
            //    //1st method to add elements to a xml document
            //    //var car = new XElement("Car");
            //    //var name = new XElement("Name", record.Name);
            //    //var combined = new XElement("Combined", record.Combined);
            //    //cars.Add(car);
            //    //cars.Add(name);
            //    //cars.Add(combined);

            //    //2d method
            //    //var name = new XElement("Name", record.Name);
            //    //var combined = new XElement("Combined", record.Combined);
            //    //var car = new XElement("Car", name, combined);
            //    //cars.Add(car);

            //    //3d method
            //    var car = new XElement("Car",
            //                    new XAttribute("Name", record.Name),
            //                    new XAttribute("Combined", record.Combined),
            //                    new XAttribute("Manufacturer", record.Manufacturer)
            //                    );
            //    cars.Add(car);
            //}

            //4d method
            //CreateXml();
            //QueryXml();

            //Working with db

            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDbContext>()); //just using it in this project
            
            InsertData();
            QueryData();

        }

        private static void QueryData()
        {
            var db = new CarDbContext();

            var query = from car in db.Cars
                        orderby car.Combined descending, car.Name ascending
                        select car;
            //var query2 =
            //    db.Cars.OrderByDescending(c => c.Combined).ThenBy(c => c.Name).Take(10);

            //var query2 =
            //    db.Cars.Where(c => c.Manufacturer == "BMW")
            //    .OrderByDescending(c => c.Combined)
            //    .ThenBy(c => c.Name)
            //    .Take(10)
            //    .Select(c => new
            //    {
            //        Name = c.Name.ToUpper()
            //    })
            //    .ToList();
            //foreach (var item in query2)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //
            var query2 =
                db.Cars.GroupBy(c => c.Manufacturer)
                .Select(g => new
                {
                    //we grouped by a manufacturer, so the key will be manufactorer(m)
                    Name = g.Key,
                    Cars = g.OrderByDescending(c => c.Combined).Take(2)
                })
                .ToList();

            //var query2 =
            //    from car in db.Cars
            //    group car by car.Manufacturer into manufacturer
            //    select new
            //    {
            //        Name = manufacturer.Key,
            //        Cars = (from car in manufacturer
            //                orderby car.Combined descending
            //                select car).Take(2)
            //    };

            foreach (var group in query2)
            {
                Console.WriteLine(group.Name);
                foreach (var car in group.Cars)
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }

            //foreach (var car in query2)
            //{
            //    Console.WriteLine($"{car.Name} : {car.Combined}");
            //}

        }

        private static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            var db = new CarDbContext();

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }
        }

        private static void QueryXml()
        {
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";

            var document = XDocument.Load("fuel.xml");
            var query =
                from element in document.Element(ns + "Cars").Elements(ex +"Car") 
                                                            ?? Enumerable.Empty<XElement>()
                //document.Descendants - to prevent exception
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;
            foreach (var name in query)
            {
                Console.WriteLine($"{name}");
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");

            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = new XDocument();
            var cars = new XElement(ns + "Cars",
                from record in records
                select new XElement(ex + "Car",
                                new XAttribute("Name", record.Name),
                                new XAttribute("Combined", record.Combined),
                                new XAttribute("Manufacturer", record.Manufacturer))
                );

            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));
            document.Add(cars);
            document.Save("fuel.xml");
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

