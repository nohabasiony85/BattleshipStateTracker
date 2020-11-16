using System.Text.Json.Serialization;
using BattleshipStateTracker.Services.Enums;

namespace BattleshipStateTracker.Services.Models
{
    public class GridCell
    {
        /// <summary>
        /// Gets Coordinate
        /// </summary>
        public Coordinate Coordinate { get; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Occupation Status { get; set; }
        
        public GridCell(int row, int column)
        {
            Coordinate = new Coordinate(row, column);
            Status = Occupation.Empty;
        }
    }
}