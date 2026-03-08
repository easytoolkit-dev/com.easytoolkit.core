using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Extension methods for <see cref="IChainableState"/> and <see cref="IChainableState{T}"/>
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
    ///     .WithUpdate((owner) => HandleUpdate())
    ///     .WithExit((owner) => Debug.Log("Exit"));
    /// </code>
    /// </example>
    public static class ChainableStateExtensions
    {
        #region Non-Generic Extensions

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState.Entered"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState.Entered"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithEnter((owner) => Debug.Log("State entered"));
        /// </code>
        /// </example>
        public static IChainableState WithEnter(this IChainableState chainableState, Action<IStateMachine> callback)
        {
            chainableState.Entered += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState.Exited"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState.Exited"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithExit((owner) => Debug.Log("State exited"));
        /// </code>
        /// </example>
        public static IChainableState WithExit(this IChainableState chainableState, Action<IStateMachine> callback)
        {
            chainableState.Exited += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState.Updated"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState.Updated"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithUpdate((owner) => Debug.Log("State updated"));
        /// </code>
        /// </example>
        public static IChainableState WithUpdate(this IChainableState chainableState, Action<IStateMachine> callback)
        {
            chainableState.Updated += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState.FixedUpdated"/> event.
        /// </summary>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState.FixedUpdated"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState()
        ///     .WithFixedUpdate((owner) => Debug.Log("State fixed updated"));
        /// </code>
        /// </example>
        public static IChainableState WithFixedUpdate(this IChainableState chainableState, Action<IStateMachine> callback)
        {
            chainableState.FixedUpdated += callback;
            return chainableState;
        }

        #endregion

        #region Generic Extensions

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState{T}.Entered"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState{T}.Entered"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithEnter((owner) => Debug.Log("State entered"));
        /// </code>
        /// </example>
        public static IChainableState<T> WithEnter<T>(this IChainableState<T> chainableState, Action<IStateMachine<T>> callback)
            where T : struct, Enum
        {
            chainableState.Entered += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState{T}.Exited"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState{T}.Exited"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithExit((owner) => Debug.Log("State exited"));
        /// </code>
        /// </example>
        public static IChainableState<T> WithExit<T>(this IChainableState<T> chainableState, Action<IStateMachine<T>> callback)
            where T : struct, Enum
        {
            chainableState.Exited += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState{T}.Updated"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState{T}.Updated"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithUpdate((owner) => Debug.Log("State updated"));
        /// </code>
        /// </example>
        public static IChainableState<T> WithUpdate<T>(this IChainableState<T> chainableState, Action<IStateMachine<T>> callback)
            where T : struct, Enum
        {
            chainableState.Updated += callback;
            return chainableState;
        }

        /// <summary>
        /// Attaches an event handler to the <see cref="IChainableState{T}.FixedUpdated"/> event.
        /// </summary>
        /// <typeparam name="T">The enum type identifying the state.</typeparam>
        /// <param name="chainableState">The chainable state to configure.</param>
        /// <param name="callback">The event handler to attach.</param>
        /// <returns>The same state instance for method chaining.</returns>
        /// <remarks>
        /// This method provides a fluent API for attaching handlers to the <see cref="IChainableState{T}.FixedUpdated"/> event.
        /// Multiple handlers can be attached by calling this method multiple times.
        /// </remarks>
        /// <example>
        /// <code>
        /// var state = new ChainableState&lt;PlayerState&gt;()
        ///     .WithFixedUpdate((owner) => Debug.Log("State fixed updated"));
        /// </code>
        /// </example>
        public static IChainableState<T> WithFixedUpdate<T>(this IChainableState<T> chainableState, Action<IStateMachine<T>> callback)
            where T : struct, Enum
        {
            chainableState.FixedUpdated += callback;
            return chainableState;
        }

        #endregion
    }
}
