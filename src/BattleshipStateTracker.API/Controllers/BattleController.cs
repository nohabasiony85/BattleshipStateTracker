using System;
using BattleshipStateTracker.API.Models;
using BattleshipStateTracker.Services;
using BattleshipStateTracker.Services.Models;
using BattleshipStateTracker.Services.Models.Enums;
using BattleshipStateTracker.Services.Models.Exceptions;
using BattleshipStateTracker.Services.Models.ShipTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BattleshipStateTracker.API.Controllers
{
    [ApiController]
    [Route("battle")]
    public class BattleController : ControllerBase
    {
        private readonly ILogger<BattleController> _logger;
        private readonly IBattleService _battleService;

        public BattleController(ILogger<BattleController> logger, IBattleService gameService)
        {
            _logger = logger;
            _battleService = gameService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Battle), 201)]
        [ProducesResponseType(typeof(FailedBattleInitiationException), 400)]
        [ProducesResponseType(typeof(InvalidBattleInitiationException), 409)]
        public IActionResult InitiateBattle([FromBody] InitiateBattleRequest request)
        {
            try
            {
                var battle = _battleService.InitiateBattle(
                    request.Dimension,
                    request.NoOfShips,
                    request.ShipLength);

                return Created(Request.Path, battle);
            }
            catch (InvalidBattleInitiationException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(ex);
            }
            catch (FailedBattleInitiationException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e);
            }
        }

        [HttpGet("{battleId}")]
        [ProducesResponseType(typeof(BattleStatus), 200)]
        [ProducesResponseType(typeof(InvalidBattleIdException), 400)]
        [ProducesResponseType(typeof(BattleIsNotExistException), 404)]
        public IActionResult GetBattleStatus(string battleId)
        {
            var status = _battleService.GetBattleStatus(battleId);

            return Ok(((BattleStatus)status).ToString());
        }


        [Route("{battleId}/ship")]
        [HttpPost]
        [ProducesResponseType(typeof(ShipBase), 201)]
        [ProducesResponseType(typeof(InvalidShipCreationException), 409)]
        [ProducesResponseType(typeof(FailedShipCreationException), 400)]
        public IActionResult AddShip(string battleId,
            [FromBody] AddShipRequest addShipRequest)
        {
            try
            {
                var direction = (ShipDirection)Enum.Parse(typeof(ShipDirection), addShipRequest.ShipDirection, true);
                var shipType = (ShipType)Enum.Parse(typeof(ShipType), addShipRequest.ShipType, true);

                var ship = _battleService.AddShip(
                    battleId,
                    new Coordinate(addShipRequest.Column, addShipRequest.Row),
                    direction, shipType
                );
                return Created(Request.Path, ship);
            }
            catch (InvalidShipCreationException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(ex.Message);
            }
            catch (FailedShipCreationException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("{battleId}/attacks")]
        [HttpPost]
        [ProducesResponseType(typeof(BattleResult), 200)]
        [ProducesResponseType(typeof(AttackFailedException), 400)]
        [ProducesResponseType(typeof(AttackFailedException), 409)]
        public IActionResult Attack(string battleId,
            [FromBody] Coordinate attackCoordinate)
        {
            try
            {
                var result = _battleService.Attack(battleId, attackCoordinate);

                return Ok(result);
            }
            catch (AttackFailedException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}