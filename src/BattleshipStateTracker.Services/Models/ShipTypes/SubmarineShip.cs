using BattleshipStateTracker.Services.Enums;

namespace BattleshipStateTracker.Services.Models.ShipTypes
{
    public class SubmarineShip : ShipBase
    {
        public SubmarineShip()
        {
            Name = "Submarine";
            Length = 3;
        }
    }
}
