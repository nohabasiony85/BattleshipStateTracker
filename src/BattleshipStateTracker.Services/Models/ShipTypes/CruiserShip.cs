using BattleshipStateTracker.Services.Enums;

namespace BattleshipStateTracker.Services.Models.ShipTypes
{
    public class CruiserShip : ShipBase
    {
        public CruiserShip()
        {
            Name = "Cruiser";
            Length = 3;
        }
    }
}
