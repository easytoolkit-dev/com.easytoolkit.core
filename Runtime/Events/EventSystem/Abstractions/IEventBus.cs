using System;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Defines a synchronous event bus for publishing and subscribing to events.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Gets the number of distinct event types that have active subscriptions.
        /// </summary>
        int EventTypeCount { get; }

        /// <summary>
        /// Gets the total number of active subscriptions across all event types.
        /// </summary>
        int TotalSubscriptionCount { get; }

        /// <summary>
        /// Subscribes a handler to the specified event type with normal priority.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="handler">The action to invoke when the event is dispatched.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler);

        /// <summary>
        /// Subscribes a handler to the specified event type with explicit priority.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="handler">The action to invoke when the event is dispatched.</param>
        /// <param name="priority">The priority level for handler execution.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler, EventPriority priority);

        /// <summary>
        /// Subscribes a handler interface to the specified event type.
        /// The priority is determined by the handler's IPriorityHandler implementation if available.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="handler">The handler to register for the event.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        IEventSubscription Subscribe<TEventArgs>(IEventHandler<TEventArgs> handler);

        /// <summary>
        /// Subscribes a handler interface to the specified event type with explicit priority.
        /// The explicit priority takes precedence over any IPriorityHandler implementation.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="handler">The handler to register for the event.</param>
        /// <param name="priority">The priority level for handler execution.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        IEventSubscription Subscribe<TEventArgs>(IEventHandler<TEventArgs> handler, EventPriority priority);

        /// <summary>
        /// Unsubscribes all subscriptions associated with the specified action handler.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event.</typeparam>
        /// <param name="handler">The action handler to unsubscribe.</param>
        /// <remarks>
        /// This method removes all subscriptions for the given handler reference,
        /// regardless of priority level. If the handler was subscribed multiple times,
        /// all instances will be removed.
        /// </remarks>
        void Unsubscribe<TEventArgs>(Action<TEventArgs> handler);

        /// <summary>
        /// Unsubscribes all subscriptions associated with the specified event handler.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event.</typeparam>
        /// <param name="handler">The event handler to unsubscribe.</param>
        /// <remarks>
        /// This method removes all subscriptions for the given handler instance,
        /// regardless of priority level. If the handler was subscribed multiple times,
        /// all instances will be removed.
        /// </remarks>
        void Unsubscribe<TEventArgs>(IEventHandler<TEventArgs> handler);

        /// <summary>
        /// Dispatches an event to all active subscribers, executing handlers in priority order.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to dispatch.</typeparam>
        /// <param name="eventArgs">The event to dispatch.</param>
        /// <returns>The number of handlers that were invoked.</returns>
        /// <remarks>
        /// Handlers are executed in priority order (High → Normal → Low).
        /// Exceptions thrown by individual handlers are caught and logged without stopping execution.
        /// </remarks>
        int Dispatch<TEventArgs>(TEventArgs eventArgs);

        /// <summary>
        /// Removes all subscriptions for the specified event type.
        /// </summary>
        /// <typeparam name="TEventArgs">The event type whose subscriptions should be cleared.</typeparam>
        void UnsubscribeAll<TEventArgs>();

        /// <summary>
        /// Removes all subscriptions from the event bus.
        /// </summary>
        void UnsubscribeAll();
    }
}
