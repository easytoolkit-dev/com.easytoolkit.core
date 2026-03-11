namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides a factory method for creating configured logger instances.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Configure"/> to obtain a logger configuration builder, then chain
    /// configuration methods to set minimum level, sinks, and other options.
    /// </remarks>
    /// <example>
    /// <code>
    /// var logger = LoggerFactory.Configure()
    ///     .MinimumLevel.Debug()
    ///     .WriteTo.UnityConsole()
    ///     .CreateLogger();
    /// </code>
    /// </example>
    public static class LoggerFactory
    {
        /// <summary>
        /// Creates a new logger configuration for building a customized logger.
        /// </summary>
        /// <returns>A new <see cref="ILoggerConfiguration"/> for configuring logger settings.</returns>
        /// <remarks>
        /// Use the fluent configuration interface to set minimum log level, add sinks,
        /// and configure other logging options before calling <see cref="ILoggerConfiguration.CreateLogger"/>.
        /// </remarks>
        public static ILoggerConfiguration Configure()
        {
            return new Implementations.LoggerConfiguration();
        }
    }
}
