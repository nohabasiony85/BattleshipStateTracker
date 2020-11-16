using System;
using System.Collections.Generic;
using BattleshipStateTracker.Services;
using BattleshipStateTracker.Services.Enums;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;
using BattleshipStateTracker.Services.Models.Exceptions;
using BattleshipStateTracker.Services.Models.ShipTypes;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BattleStateTracker.Unit.Tests
{
    public class BattleServiceTest
    {
        private readonly BattleService _battleService;
        private Mock<ILogger<BattleService>> _logger;

        public BattleServiceTest()
        {
            _logger = new Mock<ILogger<BattleService>>();
            
            _battleService = new BattleService(_logger.Object);
        }

        [Fact] 
        public void ShouldInitiateBattle_Create_Battle_Successfully_When_Pass_ValidInputs()
        { 
            //Act
            var battle = _battleService.InitiateBattle(10, 6, 3);
            
            //Assert
            Assert.NotEqual(Guid.Empty, battle.Id);
            Assert.Equal(BattleStatus.Initialized, battle.Status);
            Assert.Equal(100, battle.Grid.Cells.Length);
            Assert.Equal(Occupation.Empty, battle.Grid.Cells[9, 9].Status);
            Assert.Equal(9, battle.Grid.Cells[9, 9].Coordinate.Column);
        }

        [Fact]
        public void ShouldInitiateBattle_Throw_Exception_When_Given_ShipLength_Greater_Than_Given_GridDimension()
        {
            Assert.Throws<InvalidBattleInitiationException>(() => 
                _battleService.InitiateBattle(5, 6, 6));
        }
        
        [Fact] 
        public void ShouldGetBattleStatus_Return_Battle_Status_Successfully_When_Pass_ValidBattleId()
        { 
            //Arrange
            var battle1 = _battleService.InitiateBattle(10, 6, 3);
            var battle2 = _battleService.InitiateBattle(5, 6, 3);
 
            //Act
            var battleStatus1 = _battleService.GetBattleStatus(battle1.Id.ToString());
            var battleStatus2 = _battleService.GetBattleStatus(battle2.Id.ToString());
             
            //Assert
            Assert.Equal(BattleStatus.Initialized, battleStatus1);
            Assert.Equal(BattleStatus.Initialized, battleStatus2); 
        }
        
        [Fact] 
        public void ShouldGetBattleStatus_Throw_Exception_When_Pass_InvalidBattleId()
        {  
            //Assert
            Assert.Throws<InvalidBattleIdException>(() =>
                _battleService.GetBattleStatus("invalid-guid"));  
        }
        
        [Fact] 
        public void ShouldGetBattleStatus_Throw_Exception_When_Battle_Is_Not_Exist()
        {  
            //Assert
            Assert.Throws<BattleIsNotExistException>(() =>
                _battleService.GetBattleStatus(Guid.NewGuid().ToString()));  
        }
        
        [Fact] 
        public void ShouldAddShip_Return_Ship_When_Created_Successfully()
        {  
            //Arrange
            var battle = _battleService.InitiateBattle(10, 6, 3);
            
            //Act
            var ship = _battleService.AddShip(battle.Id.ToString(), new Coordinate(0, 0),
                ShipDirection.Horizontal, ShipType.CarrierShip);
            
            //Arrange
            Assert.NotNull(ship);
            Assert.Contains(ship.Cells, (c) => c.Status == Occupation.Ship);
        }
        
        [Fact] 
        public void ShouldAddShip_Throw_Exception_When_Game_Is_Over()
        {  
            //Arrange
            var battle = _battleService.InitiateBattle(10, 6, 3);
            battle.Status = BattleStatus.GameOver;
            
            //Arrange
            Assert.Throws<InvalidShipCreationException>(() =>  _battleService.AddShip(battle.Id.ToString(), new Coordinate(0, 0),
                ShipDirection.Horizontal, ShipType.DestroyerShip));
        }
        
        [Fact] 
        public void ShouldAddShip_Throw_Exception_When_Given_Coordinates_Is_Invalid()
        {  
            //Arrange
            var battle = _battleService.InitiateBattle(10, 6, 3);
           
            //Assert
            Assert.Throws<InvalidShipCreationException>(() =>  _battleService.AddShip(battle.Id.ToString(), new Coordinate(11, 11),
                ShipDirection.Horizontal, ShipType.DestroyerShip));
        }
        
        [Fact] 
        public void ShouldAddShip_Throw_Exception_When_Given_No_Of_Ships_Exceeeded()
        {  
            //Arrange
            var battle = _battleService.InitiateBattle(10, 2, 3);
            battle.Grid.Ships = new List<ShipBase>()
            {
                new CarrierShip(),
                new DestroyerShip()
            };
            
            //Assert
            Assert.Throws<InvalidShipCreationException>(() =>  _battleService.AddShip(battle.Id.ToString(), new Coordinate(11, 11),
                ShipDirection.Horizontal, ShipType.CruiserShip));
        }
        
        [Fact] 
        public void ShouldAttack_Return_Battle_Result_When_Given_Valid_Coordinates_Successfully()
        {  
            //Arrange
            var battle = _battleService.InitiateBattle(10, 2, 3);

            var battleId = battle.Id.ToString();

            var shipCoordinate = new Coordinate(0, 0);
            
            _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal, ShipType.BattleShip);
            _battleService.AddShip(battleId, new Coordinate(1, 0), 
                ShipDirection.Horizontal, ShipType.DestroyerShip);

            //Act
            var result = _battleService.Attack(battleId, shipCoordinate);
            
            //Assert
            Assert.Equal(Occupation.Hit ,result.AttackedCellStatus);
            Assert.Equal(BattleStatus.InPlay ,result.Status);
            Assert.False(result.AllShipsSunk);
        }

        [Fact]
        public void ShouldAttack_Throw_Exception_When_Ship_Created_Is_Less_Than_NumberOfShips()
        {
            //Arrange
            var battle = _battleService.InitiateBattle(10, 2, 3);
            var battleId = battle.Id.ToString();
            var shipCoordinate = new Coordinate(0, 0);

            //Act
            _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal, ShipType.DestroyerShip);

            //Assert
            Assert.Throws<AttackFailedException>(
                () => _battleService.Attack(battleId, shipCoordinate));
        }
        
        [Fact] 
        public void ShouldAttack_Throw_Exception_When_Game_Is_Over()
        {  
            //Arrange
            var battle = _battleService.InitiateBattle(10, 6, 3);
            var battleId = battle.Id.ToString();
            var shipCoordinate = new Coordinate(0, 0);

            //Act
            _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal, ShipType.BattleShip);
            
            //Assert
            Assert.Throws<AttackFailedException>(
                () => _battleService.Attack(battleId, shipCoordinate));
        }
        
        [Fact]
        public void ShouldAttack_Throw_Exception_When_Given_Coordinate_Invalid()
        {
            //Arrange
            var battle = _battleService.InitiateBattle(10, 2, 3);
            var battleId = battle.Id.ToString();
            var shipCoordinate = new Coordinate(0, 0);
            var attackCoordinate = new Coordinate(10, 10);

            //Act
            _battleService.AddShip(battleId, shipCoordinate,
                ShipDirection.Horizontal, ShipType.SubmarineShip);
 
            //Assert
            Assert.Throws<AttackFailedException>(
                () => _battleService.Attack(battleId, attackCoordinate));
        }

        [Fact] public void ShouldAttack_Return_Battle_Status_GameOver_When_AllShipsSunk()
        {  
            //Arrange
            var battle = _battleService.InitiateBattle(10, 2, 2);
            var battleId = battle.Id.ToString();

            //Act
            _battleService.AddShip(battleId, new Coordinate(0, 0),
                ShipDirection.Horizontal, ShipType.DestroyerShip);
            _battleService.AddShip(battleId, new Coordinate(1, 0), 
                ShipDirection.Horizontal, ShipType.DestroyerShip);

            _battleService.Attack(battleId, new Coordinate(0, 0));
            _battleService.Attack(battleId, new Coordinate(0, 1)); 
            _battleService.Attack(battleId, new Coordinate(1, 0));
            var result = _battleService.Attack(battleId, new Coordinate(1, 1));

             //Assert
            Assert.True(result.AllShipsSunk);
        }
    }
}