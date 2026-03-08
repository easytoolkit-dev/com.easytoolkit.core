using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Delegate for handling state changes.
    /// </summary>
    /// <param name="previousState">The key of the state being exited. Null when starting the state machine.</param>
    /// <param name="newState">The key of the state being entered.</param>
    public delegate void StateChangeHandler(string previousState, string newState);
}
