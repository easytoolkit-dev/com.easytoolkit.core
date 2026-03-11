namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides a fluent interface for configuring logger settings.
    /// </summary>
    /// <remarks>
    /// This interface is the entry point for configuring a logger through a fluent builder pattern.
    /// Use <see cref="WriteTo"/> to add log sinks and <see cref="MinimumLevel"/> to set the
    /// minimum log level threshold. Call <see cref="CreateLogger"/> to build the configured logger.
    /// </remarks>
    public interface ILoggerConfiguration
    {
        /// <summary>
        /// Gets the sink configuration for adding log event sinks.
        /// </summary>
        /// <remarks>
        /// Use this property to add sinks that will receive log events. Multiple sinks can be
        /// chained to output to different destinations simultaneously.
        /// </remarks>
        ILoggerSinkConfiguration WriteTo { get; }

        /// <summary>
        /// Gets the minimum level configuration for setting the log level threshold.
        /// </summary>
        /// <remarks>
        /// Use this property to set the minimum level at which log events should be processed.
        /// Events below this level will be filtered out.
        /// </remarks>
        ILoggerMinimumLevelConfiguration MinimumLevel { get; }

        /// <summary>
        /// Creates a new logger instance with the configured settings.
        /// </summary>
        /// <returns>A new <see cref="ILogger"/> instance.</returns>
        /// <remarks>
        /// This method can only be called once per configuration instance. Subsequent calls
        /// will throw an exception.
        /// </remarks>
        ILogger CreateLogger();
    }
}
