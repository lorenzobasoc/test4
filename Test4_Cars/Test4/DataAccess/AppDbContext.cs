using Microsoft.EntityFrameworkCore;
using Test4.Models;

namespace Test4.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<RoadPoint> RoadPoints { get; set; }
    }
}
