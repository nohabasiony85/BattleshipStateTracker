using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;
using BattleshipStateTracker.Services.Models.ShipTypes;

namespace BattleshipStateTracker.Services
{
    public interface IBattleService
    {
        Battle InitiateBattle(int gridDimension, int numberOfShips, int shipLength);
        BattleStatus GetBattleStatus(string battleId);
        ShipBase AddShip(string battleId, Coordinate coordinate, ShipDirection direction, ShipType shipType);
        BattleResult Attack(string battleId, Coordinate attackCoordinate);
    }
}