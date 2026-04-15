using System;
using EasyToolkit.Core.Patterns;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Global singleton facade for the event bus system.
    /// Provides convenient static access to event subscription and dispatching.
    /// </summary>
    public sealed class EventManager : Singleton<EventManager>
    {
        private IEventBus _eventBus;

        private EventManager()
        {
        }

        protected override void OnSingletonInitialize()
        {
            _eventBus = new EventBus();
        }

        /// <summary>
        /// Subscribes a handler to the specified event type with normal priority.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="handler">The action to invoke when the event is dispatched.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        public IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler)
        {
            return _eventBus.Subscribe(handler);
        }

        /// <summary>
        /// Subscribes a handler to the specified event type with explicit priority.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="handler">The action to invoke when the event is dispatched.</param>
        /// <param name="priority">The priority level for handler execution.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        public IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler, EventPriority priority)
        {
            return _eventBus.Subscribe(handler, priority);
        }

        /// <summary>
        /// Subscribes a handler interface to the specified event type.
        /// The priority is determined by the handler's IPriorityHandler implementation if available.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="handler">The handler to register for the event.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        public IEventSubscription Subscribe<TEventArgs>(IEventHandler<TEventArgs> handler)
        {
            return _eventBus.Subscribe(handler);
        }

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
        public int Dispatch<TEventArgs>(TEventArgs eventArgs)
        {
            return _eventBus.Dispatch(eventArgs);
        }

        /// <summary>
        /// Removes all subscriptions for the specified event type.
        /// </summary>
        /// <typeparam name="TEventArgs">The event type whose subscriptions should be cleared.</typeparam>
        public void UnsubscribeAll<TEventArgs>()
        {
            _eventBus.UnsubscribeAll<TEventArgs>();
        }

        /// <summary>
        /// Removes all subscriptions from the event bus.
        /// </summary>
        public void UnsubscribeAll()
        {
            _eventBus.UnsubscribeAll();
        }
    }
}
