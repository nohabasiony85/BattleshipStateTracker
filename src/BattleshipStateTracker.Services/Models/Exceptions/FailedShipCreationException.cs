using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class FailedShipCreationException : Exception
    {
        /// <summary>
        /// Defines the <see cref="FailedShipCreationException" />
        /// </summary>
        public FailedShipCreationException(string message)
            : base(message)
        {            
        }
    }
}