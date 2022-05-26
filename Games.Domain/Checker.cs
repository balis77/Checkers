namespace Games.Domain
{
    public class Checker
    {
        public string Color { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool Queen { get; set; }
        public MoveDirection Direction { get; set; }
    }
}
