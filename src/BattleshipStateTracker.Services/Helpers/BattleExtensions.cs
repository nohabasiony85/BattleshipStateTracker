using BattleshipStateTracker.Services.Enums;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;
using BattleshipStateTracker.Services.Models.ShipTypes;

namespace BattleshipStateTracker.Services.Helpers
{
    public static class BattleExtensions
    {
        /// <summary>
        /// The AddShip
        /// </summary>
        /// <param name="battle">The battle<see cref="Battle"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <param name="direction">The direction<see cref="ShipDirection"/>.</param>
        /// <param name="ship">The ship<see cref="ShipBase"/>.</param>
        /// <returns>The <see cref="ShipBase"/>.</returns>
        public static ShipBase AddShip(this Battle battle, Coordinate coordinate, ShipDirection direction, ShipBase ship)
        {
            if (direction == ShipDirection.Horizontal)
            {
                for (int i = coordinate.Column; i < coordinate.Column + ship.Length; i++)
                {
                    battle.Grid.Cells[i, coordinate.Row].Status = Occupation.Ship;
                    ship.Cells.Add(battle.Grid.Cells[i, coordinate.Row]);
                }

                battle.Grid.Ships.Add(ship);
            }
            else
            {
                for (int j = coordinate.Row; j < coordinate.Row + battle.Grid.ShipLength; j++)
                {
                    battle.Grid.Cells[coordinate.Column, j].Status = Occupation.Ship;
                    ship.Cells.Add(battle.Grid.Cells[coordinate.Column, j]);
                }
                battle.Grid.Ships.Add(ship);
            }
            return ship;
        }

        /// <summary>
        /// The IsValidCoordinate
        /// </summary>
        /// <param name="battle">The battle<see cref="Battle"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool IsValidCoordinate(this Battle battle, Coordinate coordinate) =>
            coordinate.Column >= 0
            && coordinate.Column < battle.Grid.Dimension
            && coordinate.Row >= 0
            && coordinate.Row < battle.Grid.Dimension;

        /// <summary>
        /// The CanCreateShip
        /// </summary>
        /// <param name="battle">The battle<see cref="Battle"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <param name="direction">The direction<see cref="ShipDirection"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool CanCreateShip(this Battle battle, Coordinate coordinate, ShipDirection direction)
        {
            return IsInsideGrid(battle.Grid, coordinate, direction) && CanAllocateCells(battle.Grid, coordinate, direction);
        }

        /// <summary>
        /// The CanAllocateCells
        /// </summary>
        /// <param name="grid">The grid<see cref="Grid"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <param name="direction">The direction<see cref="ShipDirection"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool CanAllocateCells(Grid grid, Coordinate coordinate, ShipDirection direction)
        {
            if (direction == ShipDirection.Horizontal)
            {
                for (var i = coordinate.Column; i < coordinate.Column + grid.ShipLength; i++)
                    if (grid.Cells[i, coordinate.Row].Status != Occupation.Empty)
                        return false;
            }
            else
            {
                for (var j = coordinate.Row; j < coordinate.Row + grid.ShipLength; j++)
                    if (grid.Cells[coordinate.Column, j].Status != Occupation.Empty)
                        return false;
            }
            return true;
        }

        /// <summary>
        /// The IsInsideGrid
        /// </summary>
        /// <param name="grid">The grid<see cref="Grid"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <param name="direction">The direction<see cref="ShipDirection"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool IsInsideGrid(Grid grid, Coordinate coordinate, ShipDirection direction)
        {
            if (direction == ShipDirection.Horizontal)
            {
                if ((coordinate.Column + grid.ShipLength) < grid.Dimension)
                    return true;
            }
            else
            {
                if ((coordinate.Row + grid.ShipLength) < grid.Dimension)
                    return true;
            }
            return false;
        }
    }
}