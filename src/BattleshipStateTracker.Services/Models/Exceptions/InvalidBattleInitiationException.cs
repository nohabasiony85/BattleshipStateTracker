using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class InvalidBattleInitiationException : Exception
    {
        /// <summary>
        /// Defines the <see cref="InvalidBattleInitiationException" />
        /// </summary>
        public InvalidBattleInitiationException(string message) 
            : base(message)
        {            
        }
    }
}