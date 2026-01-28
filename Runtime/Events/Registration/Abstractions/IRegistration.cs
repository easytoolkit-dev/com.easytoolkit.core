
namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Represents an event registration that can be unsubscribed.
    /// </summary>
    public interface IRegistration
    {
        /// <summary>
        /// Gets a value indicating whether this subscription is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Unsubscribes the event handler and marks the registration as inactive.
        /// </summary>
        /// <remarks>
        /// Calling this method on an already unregistered registration has no effect.
        /// </remarks>
        void Unregister();
    }
}
