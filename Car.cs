using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading_CarRace
{
    internal class Car
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        public double DistanceTravelled { get; set; } = 0;
        public int seconds { get; set; }
        public bool reachedGoal { get; set; }
        public TimeSpan raceTime { get; set; }
    }
}
