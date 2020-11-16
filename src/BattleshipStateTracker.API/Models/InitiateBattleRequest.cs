namespace BattleshipStateTracker.API.Models
{
    public class InitiateBattleRequest
    {
        /// <summary>
        /// Gets or Sets Ship Length
        /// </summary>
        public int ShipLength { get; set; }

        /// <summary>
        /// Gets or Sets Dimension
        /// </summary>
        public int Dimension { get; set; }

        /// <summary>
        /// Gets or Sets Number Of Ships
        /// </summary>
        public int NoOfShips { get; set; }
    }
}