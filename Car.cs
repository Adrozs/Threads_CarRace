namespace Threading_CarRace
{
    internal class Car
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        public double DistanceTravelled { get; set; } = 0;
        public bool ReachedGoal { get; set; }
        public TimeSpan RaceTime { get; set; }
    }
}
