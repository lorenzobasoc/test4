using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test4.ViewModel
{
    public class RoadPointCar
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        [Privacy]
        public int Cc { get; set; }
        public int Location { get; set; }
        public DateTime Time { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrivacyAttribute : Attribute
    {

    }

}
