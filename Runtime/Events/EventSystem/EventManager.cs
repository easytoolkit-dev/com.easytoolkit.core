using System;
using EasyToolKit.Core.Patterns;

namespace EasyToolKit.Core.Events
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

        /// <summary>
        /// Initializes the singleton instance.
        /// </summary>
        protected override void OnSingletonInit()
        {
            _eventBus = new EventBus();
        }

        /// <summary>
        /// Subscribes a handler to the specified event type with normal priority.
        /// </summary>
        public IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler)
        {
            return _eventBus.Subscribe(handler);
        }

        /// <summary>
        /// Subscribes a handler to the specified event type with explicit priority.
        /// </summary>
        public IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler, EventPriority priority)
        {
            return _eventBus.Subscribe(handler, priority);
        }

        /// <summary>
        /// Subscribes a handler interface to the specified event type.
        /// </summary>
        public IEventSubscription Subscribe<TEventArgs>(IEventHandler<TEventArgs> handler)
        {
            return _eventBus.Subscribe(handler);
        }

        /// <summary>
        /// Dispatches an event to all active subscribers.
        /// </summary>
        public int Dispatch<TEventArgs>(TEventArgs eventArgs)
        {
            return _eventBus.Dispatch(eventArgs);
        }

        /// <summary>
        /// Removes all subscriptions for the specified event type.
        /// </summary>
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
