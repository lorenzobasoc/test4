using System;
using Test4.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using Test4.Models;
using Test4.ViewModel;
using System.Reflection;

namespace Test4
{
    class Program
    {
        public static string privacy;

        public static void Main() {
            Console.WriteLine("GARA DI CORSE DEI PERSONAGGI DI CARS!!");
            Again:
            Console.WriteLine("Sei in possesso dei permessi per vedere i dati delle auto coperti da privacy?\n - si\n - no");
            privacy = Console.ReadLine();
            if (privacy != "si" || privacy != "no") {
                Console.WriteLine("Inserisci un valore corretto.");
                goto Again;
            }
            var db = GenerateDb();
            Car.getLastRoadPoint += PrintRoadPoint;
            Car.won += Winner;
            var listCars = db.Cars
                .ToList();
            var carThreads = listCars
                .Select(c => new Thread(c.Run))
                .ToList();
            foreach (var t in carThreads) {
                t.Start();
            }

        }


        public static void PrintRoadPoint() {
            var lastRoadPoint = GenerateDb().RoadPoints
                .OrderBy(x => x.Time)
                .Select(r => new RoadPointCar { Name = r.Car.Name, Brand = r.Car.Brand, Model = r.Car.Model, Cc = r.Car.Cc, Location = r.Location, Time = r.Time })
                .Last();
            PrintValue(lastRoadPoint);
        }

        private static void PrintValue(RoadPointCar lastRoadPoint) {
            if (privacy == "si") {
                Console.WriteLine($"Last Road Point: {lastRoadPoint.Name}, {lastRoadPoint.Brand}, {lastRoadPoint.Model}, {lastRoadPoint.Cc}, {lastRoadPoint.Location}, {lastRoadPoint.Time},");
            } else {
                var type = lastRoadPoint.GetType();
                var list = type
                    .GetProperties()
                    .Where(pi => pi.GetCustomAttribute<PrivacyAttribute>() == null)
                    .Select(pi => pi.GetValue(lastRoadPoint))
                    .ToList();
                Console.WriteLine(string.Join(", ", list));
            }
        }

        public static AppDbContext GenerateDb() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Host=localhost;Database=races;Username=lorenzobasoc;Password=ciao")
                .Options;
            return new AppDbContext(options);
        }

        private static void Winner(Car c) {
            Console.WriteLine($"LA GARA E' TERMINATA, {c.Name} HA VINTO!!!");
            Car.tokenSource.Cancel();
        }
    }
}
