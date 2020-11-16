using BattleshipStateTracker.Services.Enums;
using BattleshipStateTracker.Services.Models.Enums;

namespace BattleshipStateTracker.Services.Models
{
    public class BattleResult
    {
        /// <summary>
        /// Gets or Sets AttackedCellStatus
        /// </summary>
        public Occupation AttackedCellStatus { get; set; }

        /// <summary>
        /// Gets or Sets AllShipsSunk
        /// </summary>
        public bool AllShipsSunk { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        public BattleStatus Status { get; set; }
    }
}