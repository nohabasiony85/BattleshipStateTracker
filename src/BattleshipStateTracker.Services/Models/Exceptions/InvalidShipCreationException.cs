using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class InvalidShipCreationException : Exception
    {
        /// <summary>
        /// Defines the <see cref="InvalidShipCreationException" />
        /// </summary>
        public InvalidShipCreationException(string message)
            : base(message)
        {            
        }
    }
}