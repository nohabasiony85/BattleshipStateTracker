using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class FailedBattleInitiationException : Exception
    {
        /// <summary>
        /// Defines the <see cref="FailedBattleInitiationException" />
        /// </summary>
        public FailedBattleInitiationException(string message)
            : base(message)
        {            
        }
    }
}