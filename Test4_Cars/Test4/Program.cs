using System;
using Test4.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace Test4
{
    class Program
    {
        static void Main(string[] args) {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Host=localhost;Database=races;Username=lorenzobasoc;Password=ciao")
                .Options;
            var db = new AppDbContext(options);

            var listCars = db.Cars
                .Select(c => c)
                .ToList();
            var carThreads = listCars
                .Select(c => new Thread(c.Run))
                .ToList();
            foreach (var t in carThreads) {
                t.Start();
            }

        }

        
    }
}
