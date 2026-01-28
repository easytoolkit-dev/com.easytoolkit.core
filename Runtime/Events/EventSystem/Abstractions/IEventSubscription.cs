using System;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Defines a subscription to an event with lifecycle management capabilities.
    /// </summary>
    public interface IEventSubscription : IRegistration
    {
        /// <summary>
        /// Gets the type of event this subscription is registered for.
        /// </summary>
        Type EventType { get; }
    }
}
