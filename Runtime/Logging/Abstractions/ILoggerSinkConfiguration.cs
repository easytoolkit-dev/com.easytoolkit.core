namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides a fluent interface for configuring log event sinks.
    /// </summary>
    /// <remarks>
    /// This interface is used to add sinks that will receive and process log events.
    /// Sinks can output to various destinations such as console, files, or external services.
    /// Extension methods are typically provided to add specific sink types.
    /// </remarks>
    public interface ILoggerSinkConfiguration
    {
        /// <summary>
        /// Adds a custom log event sink to the logger configuration.
        /// </summary>
        /// <param name="sink">The sink to add for receiving log events.</param>
        /// <returns>The logger configuration for chaining additional configuration calls.</returns>
        /// <remarks>
        /// The sink will receive all log events that pass the minimum level filter.
        /// Multiple sinks can be added to broadcast log events to different destinations.
        /// </remarks>
        ILoggerConfiguration Sink(ILogEventSink sink);
    }
}
