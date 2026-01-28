using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Thread-safe synchronous event bus with priority-based handler execution.
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        private sealed class HandlerRegistration
        {
            public Action<object> Handler;
            public object OriginalHandler;
            public EventPriority Priority;
            public bool IsActive;
        }

        private readonly Dictionary<Type, List<HandlerRegistration>> _handlersByEventType;
        private readonly object _lock;

        /// <summary>
        /// Initializes a new instance of the EventBus class.
        /// </summary>
        public EventBus()
        {
            _handlersByEventType = new Dictionary<Type, List<HandlerRegistration>>();
            _lock = new object();
        }

        /// <summary>
        /// Gets the number of distinct event types that have active subscriptions.
        /// </summary>
        public int EventTypeCount
        {
            get
            {
                lock (_lock)
                {
                    CleanupInactiveRegistrations();
                    return _handlersByEventType.Count;
                }
            }
        }

        /// <summary>
        /// Gets the total number of active subscriptions across all event types.
        /// </summary>
        public int TotalSubscriptionCount
        {
            get
            {
                lock (_lock)
                {
                    return _handlersByEventType.Values.Sum(handlers => handlers.Count(r => r.IsActive));
                }
            }
        }

        /// <summary>
        /// Subscribes a handler to the specified event type with normal priority.
        /// </summary>
        public IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler)
        {
            return Subscribe(handler, EventPriority.Normal);
        }

        /// <summary>
        /// Subscribes a handler to the specified event type with explicit priority.
        /// </summary>
        public IEventSubscription Subscribe<TEventArgs>(Action<TEventArgs> handler, EventPriority priority)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEventArgs);
            var registration = new HandlerRegistration
            {
                Handler = eventArgs => handler((TEventArgs)eventArgs),
                OriginalHandler = handler,
                Priority = priority,
                IsActive = true
            };

            lock (_lock)
            {
                if (!_handlersByEventType.TryGetValue(eventType, out var handlers))
                {
                    handlers = new List<HandlerRegistration>();
                    _handlersByEventType[eventType] = handlers;
                }

                handlers.Add(registration);
            }

            return new EventSubscription(eventType, () => UnsubscribeHandler(handler, eventType));
        }

        /// <summary>
        /// Subscribes a handler interface to the specified event type.
        /// </summary>
        public IEventSubscription Subscribe<TEventArgs>(IEventHandler<TEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var priority = handler is IPriorityHandler priorityHandler
                ? priorityHandler.Priority
                : EventPriority.Normal;

            return Subscribe(handler, priority);
        }

        /// <summary>
        /// Subscribes a handler interface to the specified event type with explicit priority.
        /// </summary>
        public IEventSubscription Subscribe<TEventArgs>(IEventHandler<TEventArgs> handler, EventPriority priority)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var eventType = typeof(TEventArgs);
            var registration = new HandlerRegistration
            {
                Handler = eventArgs => handler.OnEvent((TEventArgs)eventArgs),
                OriginalHandler = handler,
                Priority = priority,
                IsActive = true
            };

            lock (_lock)
            {
                if (!_handlersByEventType.TryGetValue(eventType, out var handlers))
                {
                    handlers = new List<HandlerRegistration>();
                    _handlersByEventType[eventType] = handlers;
                }

                handlers.Add(registration);
            }

            return new EventSubscription(eventType, () => UnsubscribeHandler(handler, eventType));
        }

        /// <summary>
        /// Unsubscribes all subscriptions associated with the specified action handler.
        /// </summary>
        public void Unsubscribe<TEventArgs>(Action<TEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            UnsubscribeHandler(handler, typeof(TEventArgs));
        }

        /// <summary>
        /// Unsubscribes all subscriptions associated with the specified event handler.
        /// </summary>
        public void Unsubscribe<TEventArgs>(IEventHandler<TEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            UnsubscribeHandler(handler, typeof(TEventArgs));
        }

        /// <summary>
        /// Dispatches an event to all active subscribers.
        /// </summary>
        public int Dispatch<TEventArgs>(TEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var eventType = typeof(TEventArgs);
            List<HandlerRegistration> activeHandlers;

            lock (_lock)
            {
                if (!_handlersByEventType.TryGetValue(eventType, out var handlers))
                    return 0;

                activeHandlers = handlers.Where(r => r.IsActive).OrderByDescending(r => r.Priority).ToList();
            }

            var invokedCount = 0;

            foreach (var registration in activeHandlers)
            {
                try
                {
                    registration.Handler(eventArgs);
                    invokedCount++;
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError(
                        $"[EventBus] HandlerFailed: Event type '{eventType.Name}' threw exception: {ex.Message}\n{ex.StackTrace}");
                }
            }

            return invokedCount;
        }

        /// <summary>
        /// Removes all subscriptions for the specified event type.
        /// </summary>
        public void UnsubscribeAll<TEventArgs>()
        {
            var eventType = typeof(TEventArgs);

            lock (_lock)
            {
                if (_handlersByEventType.TryGetValue(eventType, out var handlers))
                {
                    foreach (var registration in handlers)
                    {
                        registration.IsActive = false;
                    }

                    _handlersByEventType.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// Removes all subscriptions from the event bus.
        /// </summary>
        public void UnsubscribeAll()
        {
            lock (_lock)
            {
                foreach (var handlers in _handlersByEventType.Values)
                {
                    foreach (var registration in handlers)
                    {
                        registration.IsActive = false;
                    }
                }

                _handlersByEventType.Clear();
            }
        }

        private void UnsubscribeHandler(object handler, Type eventType)
        {
            lock (_lock)
            {
                if (!_handlersByEventType.TryGetValue(eventType, out var handlers))
                    return;

                foreach (var registration in handlers)
                {
                    if (ReferenceEquals(registration.OriginalHandler, handler) && registration.IsActive)
                    {
                        registration.IsActive = false;
                    }
                }
            }
        }

        private void CleanupInactiveRegistrations()
        {
            var emptyEventTypes = new List<Type>();

            foreach (var kvp in _handlersByEventType)
            {
                kvp.Value.RemoveAll(r => !r.IsActive);

                if (kvp.Value.Count == 0)
                {
                    emptyEventTypes.Add(kvp.Key);
                }
            }

            foreach (var eventType in emptyEventTypes)
            {
                _handlersByEventType.Remove(eventType);
            }
        }
    }
}
