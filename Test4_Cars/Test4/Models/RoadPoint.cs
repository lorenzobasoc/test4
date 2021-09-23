using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4.Models
{
    public class RoadPoint
    {
        public int Id { get; set; }
        public int Location { get; set; }
        public DateTime Time { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
