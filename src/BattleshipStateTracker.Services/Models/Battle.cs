using System;
using System.Text.Json.Serialization;
using BattleshipStateTracker.Services.Models.Enums;

namespace BattleshipStateTracker.Services.Models
{
    public class Battle
    {
        /// <summary>
        /// Gets Id
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets Grid
        /// </summary>
        public Grid Grid { get;  } 
        
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BattleStatus Status { get; set; }

        public Battle(int gridDimension, int numberOfShips, int shipLength)
        {
            Id = Guid.NewGuid();
            Grid = new Grid(gridDimension,numberOfShips, shipLength);
            Status = BattleStatus.Initialized;
        }

        public override string ToString() => $"Id: {Id}, Status: {Status}";
    }
}