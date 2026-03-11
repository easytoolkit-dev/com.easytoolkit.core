using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Logging.Implementations
{
    /// <summary>
    /// Default implementation of <see cref="ILoggerConfiguration"/> for building configured loggers.
    /// </summary>
    /// <remarks>
    /// This class collects sink configurations and minimum level settings through a fluent interface,
    /// then creates a <see cref="Logger"/> instance with those settings. The sinks are aggregated
    /// into a single composite sink that broadcasts events to all registered sinks.
    /// </remarks>
    internal sealed class LoggerConfiguration : ILoggerConfiguration
    {
        private readonly List<ILogEventSink> _sinks = new();
        private bool _loggerCreated;
        private LogEventLevel _minimumLevel = LogEventLevel.Info;

        /// <inheritdoc/>
        public ILoggerSinkConfiguration WriteTo { get; }

        /// <inheritdoc/>
        public ILoggerMinimumLevelConfiguration MinimumLevel { get; }

        /// <summary>
        /// Initializes a new instance with default configuration.
        /// </summary>
        /// <remarks>
        /// Initializes the <see cref="WriteTo"/> and <see cref="MinimumLevel"/> configuration objects
        /// with delegates that modify this configuration instance. The default minimum level is Info.
        /// </remarks>
        public LoggerConfiguration()
        {
            WriteTo = new LoggerSinkConfiguration(this, sink => _sinks.Add(sink));
            MinimumLevel = new LoggerMinimumLevelConfiguration(this, level => _minimumLevel = level);
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="CreateLogger"/> has been called previously on this instance.
        /// </exception>
        public ILogger CreateLogger()
        {
            if (_loggerCreated)
                throw new InvalidOperationException("CreateLogger() was previously called and can only be called once.");
            _loggerCreated = true;

            var sink = new AggregateLogEventSink(_sinks);
            return new Logger(_minimumLevel, sink);
        }
    }
}
