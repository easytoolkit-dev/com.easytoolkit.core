using System;

namespace EasyToolkit.Core.Logging.Implementations
{
    /// <summary>
    /// Implementation of <see cref="ILoggerMinimumLevelConfiguration"/> for setting the minimum log level.
    /// </summary>
    /// <remarks>
    /// This class provides the minimum level configuration functionality for the fluent builder pattern.
    /// It maintains a reference to the parent configuration and uses a delegate to set the
    /// minimum level in the configuration.
    /// </remarks>
    internal sealed class LoggerMinimumLevelConfiguration : ILoggerMinimumLevelConfiguration
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly Action<LogEventLevel> _setMinimum;

        /// <summary>
        /// Initializes a new instance with the specified parent configuration and level setting delegate.
        /// </summary>
        /// <param name="loggerConfiguration">The parent logger configuration for fluent chaining.</param>
        /// <param name="setMinimum">The delegate that sets the minimum level in the configuration.</param>
        public LoggerMinimumLevelConfiguration(LoggerConfiguration loggerConfiguration, Action<LogEventLevel> setMinimum)
        {
            _loggerConfiguration = loggerConfiguration;
            _setMinimum = setMinimum;
        }

        /// <inheritdoc/>
        public ILoggerConfiguration Is(LogEventLevel minimumLevel)
        {
            _setMinimum(minimumLevel);
            return _loggerConfiguration;
        }
    }
}
