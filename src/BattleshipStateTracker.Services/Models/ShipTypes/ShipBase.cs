using BattleshipStateTracker.Services.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BattleshipStateTracker.Services.Models.ShipTypes
{
    public abstract class ShipBase
    { 
        public string Name { get; set; }

        public List<GridCell> Cells { get; }

        public int Length { get; set; }

        public ShipBase()
        {
            Cells = new List<GridCell>();
        }

        public bool IsSunk
        {
            get
            {
                return Cells.All(c => c.Status == Occupation.Hit);
            }
        }
    }
}
