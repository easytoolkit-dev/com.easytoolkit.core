using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Factory for creating state machine instances.
    /// </summary>
    public static class StateMachineFactory
    {
        /// <summary>
        /// Creates a strict state machine that requires all states to be registered.
        /// </summary>
        /// <returns>A new strict state machine instance.</returns>
        public static IStateMachine Create()
        {
            return new Implementations.StateMachine();
        }

        /// <summary>
        /// Creates a lenient state machine that allows transitions to non-existent states.
        /// </summary>
        /// <returns>A new lenient state machine instance.</returns>
        public static IStateMachine CreateLenient()
        {
            return new Implementations.LenientStateMachine();
        }

        /// <summary>
        /// Creates a strict generic state machine that requires all states to be registered.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <returns>A new strict generic state machine instance.</returns>
        public static IStateMachine<T> Create<T>() where T : struct, Enum
        {
            return new Implementations.StateMachine<T>();
        }

        /// <summary>
        /// Creates a lenient generic state machine that allows transitions to non-existent states.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <returns>A new lenient generic state machine instance.</returns>
        public static IStateMachine<T> CreateLenient<T>() where T : struct, Enum
        {
            return new Implementations.LenientStateMachine<T>();
        }
    }
}
