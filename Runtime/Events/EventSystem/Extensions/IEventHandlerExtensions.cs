using System;
using UnityEngine;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Extension methods for IEventHandler providing convenient registration capabilities.
    /// </summary>
    public static class IEventHandlerExtensions
    {
        /// <summary>
        /// Registers this event handler to the global EventManager for the specified event type.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to handle.</typeparam>
        /// <param name="handler">The event handler instance.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        public static IEventSubscription RegisterEventHandler<TEventArgs>(this IEventHandler<TEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return EventManager.Instance.Subscribe(handler);
        }

        /// <summary>
        /// Registers this event handler to the specified event bus for the specified event type.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to handle.</typeparam>
        /// <param name="handler">The event handler instance.</param>
        /// <param name="eventBus">The event bus to register with.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        public static IEventSubscription RegisterEventHandler<TEventArgs>(this IEventHandler<TEventArgs> handler,
            IEventBus eventBus)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (eventBus == null)
                throw new ArgumentNullException(nameof(eventBus));

            return eventBus.Subscribe(handler);
        }
    }
}
