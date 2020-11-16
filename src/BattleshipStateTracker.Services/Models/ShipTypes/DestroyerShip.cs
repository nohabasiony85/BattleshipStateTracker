using BattleshipStateTracker.Services.Enums;

namespace BattleshipStateTracker.Services.Models.ShipTypes
{
    public class DestroyerShip : ShipBase
    {
        public DestroyerShip()
        {
            Name = "Destroyer";
            Length = 2;
        }
    }
}
