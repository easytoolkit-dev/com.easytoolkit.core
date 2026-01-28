namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Defines the priority level for event handlers.
    /// Higher priority handlers execute before lower priority handlers.
    /// </summary>
    public enum EventPriority
    {
        /// <summary>
        /// Low priority handlers execute after normal and high priority handlers.
        /// </summary>
        Low = 0,

        /// <summary>
        /// Normal priority handlers execute after high priority handlers and before low priority handlers.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// High priority handlers execute first.
        /// </summary>
        High = 2
    }
}
