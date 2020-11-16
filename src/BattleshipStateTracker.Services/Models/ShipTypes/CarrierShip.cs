using BattleshipStateTracker.Services.Enums;

namespace BattleshipStateTracker.Services.Models.ShipTypes
{
    public class CarrierShip : ShipBase
    {
        public CarrierShip()
        {
            Name = "Carrier";
            Length = 3;
        }
    }
}
