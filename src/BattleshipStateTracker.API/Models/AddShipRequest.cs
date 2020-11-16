namespace BattleshipStateTracker.API.Models
{
    public class AddShipRequest
    {
        /// <summary>
        /// Gets or Sets Column number
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Gets or Sets Row number
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or Sets Ship Direction
        /// </summary>
        public string ShipDirection { get; set; }

        /// <summary>
        /// Gets or Sets Ship Type
        /// </summary>
        public string ShipType { get; set; }
    }
}