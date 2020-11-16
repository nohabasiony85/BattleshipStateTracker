using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class BattleIsNotExistException : Exception
    {
        /// <summary>
        /// Defines the <see cref="BattleIsNotExistException" />
        /// </summary>
        public BattleIsNotExistException(string message) 
            : base(message)
        {            
        }
    }
}