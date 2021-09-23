using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Test4.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Test4.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Cc { get; set; }
        public int MaxSpeed { get; set; }

        private DateTime _currentTime = DateTime.Now;
        private int _totalPosition;       

        public static CancellationTokenSource tokenSource;
        public static event Action getLastRoadPoint;
        public static event Action<Car> won;

        public List<RoadPoint> RoadPoints { get; set; }

        public async void Run() {
            tokenSource = new CancellationTokenSource();
            var db = GenerateDb();
            while (true) {
                if (tokenSource.IsCancellationRequested) {
                    break;
                }
                var delay = GenerateRandom(1, 1000);
                var casual = GenerateRandomDouble();
                await Task.Delay(delay);
                var newPosition = (int)casual * MaxSpeed;

                RoadPoints.Add(new RoadPoint { Location = newPosition + _totalPosition, Time = _currentTime.AddMilliseconds(delay) });
                getLastRoadPoint.Invoke();
                db.SaveChanges();
                if (_totalPosition >= 5000) {
                    won.Invoke(this);
                    break;
                }
            }    
        }

        public static int GenerateRandom(int startIndex, int stopIndex) {
            return new Random().Next(startIndex, stopIndex + 1);
        }

        public static double GenerateRandomDouble() {
            var random = new Random().NextDouble();
            while (random < 0.5) {
                random = new Random().NextDouble();
            }
            return random;
        }

        public static AppDbContext GenerateDb() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Host=localhost;Database=races;Username=lorenzobasoc;Password=ciao")
                .Options;
            return new AppDbContext(options);
        } 
    }
}
