namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides extension methods for configuring the minimum log level.
    /// </summary>
    public static class LoggerMinimumLevelExtensions
    {
        /// <summary>
        /// Sets the minimum log level to <see cref="LogEventLevel.Debug"/>.
        /// </summary>
        /// <param name="minimumLevelConfiguration">The minimum level configuration.</param>
        /// <returns>The logger configuration for chaining.</returns>
        public static ILoggerConfiguration Debug(this ILoggerMinimumLevelConfiguration minimumLevelConfiguration)
        {
            return minimumLevelConfiguration.Is(LogEventLevel.Debug);
        }

        /// <summary>
        /// Sets the minimum log level to <see cref="LogEventLevel.Info"/>.
        /// </summary>
        /// <param name="minimumLevelConfiguration">The minimum level configuration.</param>
        /// <returns>The logger configuration for chaining.</returns>
        public static ILoggerConfiguration Info(this ILoggerMinimumLevelConfiguration minimumLevelConfiguration)
        {
            return minimumLevelConfiguration.Is(LogEventLevel.Info);
        }

        /// <summary>
        /// Sets the minimum log level to <see cref="LogEventLevel.Warn"/>.
        /// </summary>
        /// <param name="minimumLevelConfiguration">The minimum level configuration.</param>
        /// <returns>The logger configuration for chaining.</returns>
        public static ILoggerConfiguration Warn(this ILoggerMinimumLevelConfiguration minimumLevelConfiguration)
        {
            return minimumLevelConfiguration.Is(LogEventLevel.Warn);
        }

        /// <summary>
        /// Sets the minimum log level to <see cref="LogEventLevel.Error"/>.
        /// </summary>
        /// <param name="minimumLevelConfiguration">The minimum level configuration.</param>
        /// <returns>The logger configuration for chaining.</returns>
        public static ILoggerConfiguration Error(this ILoggerMinimumLevelConfiguration minimumLevelConfiguration)
        {
            return minimumLevelConfiguration.Is(LogEventLevel.Error);
        }

        /// <summary>
        /// Sets the minimum log level to <see cref="LogEventLevel.Fatal"/>.
        /// </summary>
        /// <param name="minimumLevelConfiguration">The minimum level configuration.</param>
        /// <returns>The logger configuration for chaining.</returns>
        public static ILoggerConfiguration Fatal(this ILoggerMinimumLevelConfiguration minimumLevelConfiguration)
        {
            return minimumLevelConfiguration.Is(LogEventLevel.Fatal);
        }
    }
}
