namespace Roguelike.models
{
    public class Room
    {
        public Corridor NorthCorridor { get; set; }
        public Corridor EastCorridor { get; set; }
        public Corridor SouthCorridor { get; set; }
        public Corridor WestCorridor { get; set; }

        public Enemy Enemy { get; set; }

        public bool Visited { get; set; }
    }
}
