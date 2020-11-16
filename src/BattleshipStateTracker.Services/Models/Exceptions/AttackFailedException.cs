using System;

namespace BattleshipStateTracker.Services.Models.Exceptions
{
    public class AttackFailedException : Exception
    {
        /// <summary>
        /// Defines the <see cref="AttackFailedException" />
        /// </summary>
        public AttackFailedException(string message)
        : base(message)
        {            
        }
    }
}