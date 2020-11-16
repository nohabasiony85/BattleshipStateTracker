using BattleshipStateTracker.Services.Enums;

namespace BattleshipStateTracker.Services.Models.ShipTypes
{
    public class Battleship : ShipBase
    {
        public Battleship()
        {
            Name = "Battleship";
            Length = 4;
        }
    }
}
