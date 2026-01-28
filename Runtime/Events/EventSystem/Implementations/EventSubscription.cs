using System;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Represents an active event subscription with lifecycle management capabilities.
    /// </summary>
    public sealed class EventSubscription : Registration, IEventSubscription
    {
        /// <summary>
        /// Initializes a new instance of the EventSubscription class.
        /// </summary>
        /// <param name="eventType">The type of event this subscription is for.</param>
        /// <param name="unregisterAction">The action to execute when unregistering the subscription.</param>
        /// <exception cref="ArgumentNullException">Thrown when eventType or unregisterAction is null.</exception>
        public EventSubscription(Type eventType, Action unregisterAction)
            : base(unregisterAction)
        {
            EventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
        }

        /// <inheritdoc />
        public Type EventType { get; }
    }
}
