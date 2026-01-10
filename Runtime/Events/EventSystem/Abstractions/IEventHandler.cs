namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Defines a handler for processing events of a specific type.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of event this handler processes.</typeparam>
    public interface IEventHandler<in TEventArgs>
    {
        /// <summary>
        /// Handles the specified event.
        /// </summary>
        /// <param name="eventArgs">The event to handle.</param>
        void OnEvent(TEventArgs eventArgs);
    }
}
