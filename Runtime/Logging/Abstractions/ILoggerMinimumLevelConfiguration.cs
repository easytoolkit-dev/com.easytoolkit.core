namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides a fluent interface for configuring the minimum log level.
    /// </summary>
    /// <remarks>
    /// This interface is used to set the minimum severity level at which log events
    /// should be processed. Events below this level will be filtered out and not
    /// passed to any sinks.
    /// </remarks>
    public interface ILoggerMinimumLevelConfiguration
    {
        /// <summary>
        /// Sets the minimum log level to the specified value.
        /// </summary>
        /// <param name="minimumLevel">The minimum level at which log events should be processed.</param>
        /// <returns>The logger configuration for chaining additional configuration calls.</returns>
        /// <remarks>
        /// Only log events at or above the specified level will be processed.
        /// For example, setting to <see cref="LogEventLevel.Warn"/> will filter out
        /// Debug and Info messages but process Warn, Error, and Fatal messages.
        /// </remarks>
        ILoggerConfiguration Is(LogEventLevel minimumLevel);
    }
}
