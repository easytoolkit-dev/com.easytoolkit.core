using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Extension methods for <see cref="ChainableState"/> and <see cref="ChainableState{T}"/>
    /// that provide a fluent chain API for configuring state event handlers.
    /// </summary>
    /// <remarks>
    /// These extension methods enable a fluent, chainable API for attaching event handlers
    /// to chainable states. Each method returns the state instance, allowing multiple
    /// configurations to be chained together in a single expression.
    /// </remarks>
    /// <example>
    /// The following example demonstrates the fluent API:
    /// <code>
    /// var state = new ChainableState&lt;PlayerState&gt;()
    ///     .WithEnter((owner) => Debug.Log("Enter"))
    ///     .WithTick((owner, deltaTime) => HandleUpdate())
    ///     .WithExit((owner) => Debug.Log("Exit"));
    /// </code>
    /// </example>
    public static class ChainableStateExtensions
    {
        #region Non-Generic Extensions

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState.Entered"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState.Entered"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithEnter((owner) => Debug.Log("State entered"));
        /// </code>
        /// </example>
        public static ChainableState WithEnter(this ChainableState chainableState, Action<StateMachine> callback)
        {
            chainableState.Entered += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState.Exited"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState.Exited"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithExit((owner) => Debug.Log("State exited"));
        /// </code>
        /// </example>
        public static ChainableState WithExit(this ChainableState chainableState, Action<StateMachine> callback)
        {
            chainableState.Exited += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState.Ticked"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState.Ticked"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithTick((owner, deltaTime) => Debug.Log("State updated"));
        /// </code>
        /// </example>
        public static ChainableState WithTick(this ChainableState chainableState, Action<StateMachine, float> callback)
        {
            chainableState.Ticked += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState.FixedTicked"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState.FixedTicked"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithFixedTick((owner, deltaTime) => Debug.Log("State fixed updated"));
        /// </code>
        /// </example>
        public static ChainableState WithFixedTick(this ChainableState chainableState, Action<StateMachine, float> callback)
        {
            chainableState.FixedTicked += callback;
            return chainableState;
        }

        #endregion

        #region Generic Extensions

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState{T}.Entered"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState{T}.Entered"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithEnter((owner) => Debug.Log("State entered"));
        /// </code>
        /// </example>
        public static ChainableState<T> WithEnter<T>(this ChainableState<T> chainableState, Action<StateMachine<T>> callback)
            where T : struct, Enum
        {
            chainableState.Entered += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState{T}.Exited"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState{T}.Exited"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithExit((owner) => Debug.Log("State exited"));
        /// </code>
        /// </example>
        public static ChainableState<T> WithExit<T>(this ChainableState<T> chainableState, Action<StateMachine<T>> callback)
            where T : struct, Enum
        {
            chainableState.Exited += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState{T}.Ticked"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState{T}.Ticked"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithTick((owner, deltaTime) => Debug.Log("State updated"));
        /// </code>
        /// </example>
        public static ChainableState<T> WithTick<T>(this ChainableState<T> chainableState, Action<StateMachine<T>, float> callback)
            where T : struct, Enum
        {
            chainableState.Ticked += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="ChainableState{T}.FixedTicked"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="ChainableState{T}.FixedTicked"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithFixedTick((owner, deltaTime) => Debug.Log("State fixed updated"));
        /// </code>
        /// </example>
        public static ChainableState<T> WithFixedTick<T>(this ChainableState<T> chainableState, Action<StateMachine<T>, float> callback)
            where T : struct, Enum
        {
            chainableState.FixedTicked += callback;
            return chainableState;
        }

        #endregion
    }
}
