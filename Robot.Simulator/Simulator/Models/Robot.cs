namespace Simulator.Models
{
    public class Robot
    {
        public string Direction { get; set; }
        public bool IsPlacedOnBoard { get; set; }
        public Coordinates Position { get; set; }
    }
}
