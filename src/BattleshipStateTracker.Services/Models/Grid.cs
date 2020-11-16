using System.Collections.Generic;
using BattleshipStateTracker.Services.Models.ShipTypes;
using Newtonsoft.Json;

namespace BattleshipStateTracker.Services.Models
{
    public class Grid
    {
        /// <summary>
        /// Gets Cells
        /// </summary>
        [JsonIgnore]
        public GridCell[,] Cells { get; }

        /// <summary>
        /// Gets Dimension
        /// </summary>
        public int Dimension { get; }

        /// <summary>
        /// Gets or Sets NumberOfShips
        /// </summary>
        public int NumberOfShips { get; set; }

        /// <summary>
        /// Gets or Sets ShipLength
        /// </summary>
        public int ShipLength { get; }

        /// <summary>
        /// Gets or Sets Ships
        /// </summary>
        public IList<ShipBase> Ships { get; set; }

        public Grid(int dimension, int numberOfShips, int shipLength)
        {
            Dimension = dimension;
            NumberOfShips = numberOfShips;
            ShipLength = shipLength;
            Ships = new List<ShipBase>();
            
            Cells = new GridCell[dimension,dimension];
            
            for (var i = 0; i < dimension; i++)
            for (var j = 0; j < dimension; j++)
            {
                Cells[i, j] = new GridCell(i, j);
            }
        }
    }
}