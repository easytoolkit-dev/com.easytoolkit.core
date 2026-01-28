using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Interface for a state in the State Machine.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public interface IState<T> where T : Enum
    {
        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        void OnExit();

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// Called every fixed framerate frame while the state is active.
        /// </summary>
        void OnFixedUpdate();
    }
}
