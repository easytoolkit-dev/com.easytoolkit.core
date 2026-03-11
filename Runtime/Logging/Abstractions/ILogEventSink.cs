namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Defines a sink that receives and processes log events.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are responsible for emitting log events
    /// to various outputs such as console, file, or external services.
    /// Each sink receives <see cref="LogEvent"/> instances through the <see cref="Emit"/> method.
    /// Sinks are typically held by <see cref="ILogger"/> instances for dispatching log events.
    /// </remarks>
    public interface ILogEventSink
    {
        /// <summary>
        /// Emits the provided log event to the sink's output destination.
        /// </summary>
        /// <param name="logEvent">The log event containing timestamp, level, message, and optional exception.</param>
        void Emit(LogEvent logEvent);
    }
}
