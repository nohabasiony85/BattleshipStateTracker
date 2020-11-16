using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipStateTracker.Services.Enums;
using BattleshipStateTracker.Services.Helpers;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;
using BattleshipStateTracker.Services.Models.Exceptions;
using BattleshipStateTracker.Services.Models.ShipTypes;
using Microsoft.Extensions.Logging;

namespace BattleshipStateTracker.Services
{
    public class BattleService : IBattleService
    {
        private readonly ILogger<BattleService> _logger;
        private static IList<Battle> _battles;

        public BattleService(ILogger<BattleService> logger)
        {
            _logger = logger;
            _battles = new List<Battle>();
        }

        /// <summary>
        /// The InitiateBattle
        /// </summary>
        /// <param name="gridDimension">The gridDimension<see cref="int"/>.</param>
        /// <param name="numberOfShips">The battle<see cref="int"/>.</param>
        /// <param name="shipLength">The shipLength<see cref="int"/>.</param>
        /// <returns></returns>
        public Battle InitiateBattle(int gridDimension, int numberOfShips, int shipLength)
        {
            if (shipLength > gridDimension)
                throw new InvalidBattleInitiationException("Failed to initiate battle: ship length is greater than grid dimension.");

            var battle = new Battle(gridDimension, numberOfShips, shipLength);
            _battles.Add(battle);

            _logger.LogInformation($"battle initiated : {battle}.");

            return battle;
        }

        /// <summary>
        /// The GetBattleStatus
        /// </summary>
        /// <param name="battleId">The battleId<see cref="string"/>.</param>
        /// <returns>The <see cref="BattleStatus"/>.</returns>
        public BattleStatus GetBattleStatus(string battleId)
        {
            var battle = GetBattle(battleId);

            return battle.Status;
        }

        /// <summary>
        /// The AddShip
        /// </summary>
        /// <param name="battleId">The battleId<see cref="string"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <param name="direction">The direction<see cref="ShipDirection"/>.</param>
        /// <param name="shipType">The shipType<see cref="ShipType"/>.</param>
        /// <returns>The <see cref="ShipBase"/>.</returns>
        public ShipBase AddShip(string battleId, Coordinate coordinate, ShipDirection direction, ShipType shipType)
        {
            var battle = GetBattle(battleId);

            var shipInfo = GetShipByType(shipType);

            ValidateShipCreation(battle, coordinate, direction);

            var ship = battle.AddShip(coordinate, direction, shipInfo);

            return ship;
        }

        /// <summary>
        /// The GetShipByType
        /// </summary>
        /// <param name="shipType">The shipType<see cref="ShipType"/>.</param>
        /// <returns>The <see cref="ShipBase"/>.</returns>
        private ShipBase GetShipByType(ShipType shipType)
        {
            return shipType switch
            {
                ShipType.CarrierShip => new CarrierShip(),
                ShipType.BattleShip => new Battleship(),
                ShipType.DestroyerShip => new DestroyerShip(),
                ShipType.SubmarineShip => new SubmarineShip(),
                ShipType.CruiserShip => new CruiserShip(),
                _ => throw new Exception("Invalid Ship Type."),
            };
        }

        /// <summary>
        /// The Attack
        /// </summary>
        /// <param name="battleId">The battleId<see cref="string"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <returns>The <see cref="BattleResult"/>.</returns>
        public BattleResult Attack(string battleId, Coordinate coordinate)
        {
            var battle = GetBattle(battleId);

            ValidateAttack(coordinate, battle);

            if (battle.Status == BattleStatus.Initialized) battle.Status = BattleStatus.InPlay;

            var attackedCell = battle.Grid.Cells[coordinate.Column, coordinate.Row];

            attackedCell.Status = attackedCell.Status switch
            {
                Occupation.Empty => Occupation.Miss,
                Occupation.Ship => Occupation.Hit,
                _ => attackedCell.Status
            };

            _logger.LogInformation(
                $"Cell ({attackedCell.Coordinate.Column},{attackedCell.Coordinate.Row}) was attacked and the result is {Enum.GetName(typeof(Occupation), attackedCell.Status)}");

            var allShipsSunk = battle.Grid.Ships.All(s => s.IsSunk);

            if (allShipsSunk)
            {
                battle.Status = BattleStatus.GameOver;
                _logger.LogInformation("Game Over! All ships has been sunk!");
            }

            return new BattleResult
            {
                AttackedCellStatus = attackedCell.Status,
                AllShipsSunk = allShipsSunk,
                Status = battle.Status
            };
        }

        /// <summary>
        /// The ValidateAttack
        /// </summary>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <param name="battle">The battle<see cref="Battle"/>.</param>
        private void ValidateAttack(Coordinate coordinate, Battle battle)
        {
            if (battle.Status == BattleStatus.GameOver)
                throw new AttackFailedException("This battle is over. Can't process attack.");

            if (battle.Grid.Ships.Count != battle.Grid.NumberOfShips)
                throw new AttackFailedException(
                    $"You have to create a total of {battle.Grid.NumberOfShips} ships to start the battle");

            if (!battle.IsValidCoordinate(coordinate))
                throw new AttackFailedException(
                    $"The attacked cell is invalid column and row coordinates have to be from 0 to {battle.Grid.Dimension}");
        }

        /// <summary>
        /// The ValidateShipCreation
        /// </summary>
        /// <param name="battle">The battle<see cref="Battle"/>.</param>
        /// <param name="coordinate">The coordinate<see cref="Coordinate"/>.</param>
        /// <param name="direction">The direction<see cref="ShipDirection"/>.</param>
        private void ValidateShipCreation(Battle battle, Coordinate coordinate, ShipDirection direction)
        {
            if (battle.Status == BattleStatus.GameOver)
                throw new InvalidShipCreationException("This battle is over. Ship can't be created.");

            if (!battle.IsValidCoordinate(coordinate) || !battle.CanCreateShip(coordinate, direction))
                throw new InvalidShipCreationException("Ship can't be created.");

            if (battle.Grid.Ships.Count >= battle.Grid.NumberOfShips)
                    throw new InvalidShipCreationException("Can't fit more ships");
        }

        /// <summary>
        /// The GetBattle
        /// </summary>
        /// <param name="battleId">The battleId<see cref="string"/>.</param>
        /// <returns></returns>
        private Battle GetBattle(string battleId)
        {
            if (!Guid.TryParse(battleId, out var id))
                throw new InvalidBattleIdException("Invalid battle id.");

            var battle = _battles.FirstOrDefault(b => b.Id == id);

            if (battle == null) throw new BattleIsNotExistException("The battle is not exist.");
            return battle;
        }
    }
}