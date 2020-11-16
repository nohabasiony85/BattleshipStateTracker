namespace BattleshipStateTracker.Services.Models
{
    public class Coordinate
    {
        /// <summary>
        /// Gets or Sets Row
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or Sets Column
        /// </summary>
        public int Column { get; set; }

        public Coordinate() { }

        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}