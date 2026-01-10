namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Defines a handler with explicit priority specification.
    /// Implement this interface to override the default priority for event handlers.
    /// </summary>
    public interface IPriorityHandler
    {
        /// <summary>
        /// Gets the priority level for this handler.
        /// Higher priority handlers execute before lower priority handlers.
        /// </summary>
        EventPriority Priority { get; }
    }
}
