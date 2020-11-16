using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class InvalidBattleIdException : Exception
    {
        /// <summary>
        /// Defines the <see cref="InvalidBattleIdException" />
        /// </summary>
        public InvalidBattleIdException(string message) 
            : base(message)
        {            
        }
    }
}