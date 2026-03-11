using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Logging.Implementations
{
    /// <summary>
    /// Default implementation of <see cref="ILogger"/> that filters and dispatches log events to sinks.
    /// </summary>
    /// <remarks>
    /// This implementation filters log events based on a minimum level threshold and dispatches
    /// enabled events to a configured <see cref="ILogEventSink"/>. The sink can be a simple
    /// implementation or an aggregate that broadcasts to multiple sinks.
    /// </remarks>
    internal sealed class Logger : ILogger
    {
        private readonly LogEventLevel _minimumLevel;
        private readonly ILogEventSink _sink;

        /// <summary>
        /// Initializes a new instance with the specified minimum level and sink.
        /// </summary>
        /// <param name="minimumLevel">The minimum level at which log events should be processed.</param>
        /// <param name="sink">The sink to which log events are dispatched.</param>
        public Logger(LogEventLevel minimumLevel, ILogEventSink sink)
        {
            _minimumLevel = minimumLevel;
            _sink = sink;
        }

        /// <summary>
        /// Determines whether the specified log level is enabled for logging.
        /// </summary>
        /// <param name="level">The log level to check.</param>
        /// <returns>True if the level is greater than or equal to the minimum level; otherwise, false.</returns>
        private bool IsEnabled(LogEventLevel level)
        {
            return level >= _minimumLevel;
        }

        /// <summary>
        /// Dispatches the log event to the configured sink.
        /// </summary>
        /// <param name="logEvent">The log event to dispatch.</param>
        private void Dispatch(LogEvent logEvent)
        {
            _sink.Emit(logEvent);
        }

        /// <summary>
        /// Writes a log event if the level is enabled.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="exception">The optional exception associated with the log event.</param>
        /// <param name="message">The log message.</param>
        private void Write(LogEventLevel level, [CanBeNull] Exception exception, string message)
        {
            if (!IsEnabled(level))
                return;
            var logEvent = new LogEvent(DateTime.Now, level, exception, message, new StackTrace(true));
            Dispatch(logEvent);
        }

        /// <inheritdoc/>
        public void Debug(string message)
        {
            Write(LogEventLevel.Debug, null, message);
        }

        /// <inheritdoc/>
        public void Info(string message)
        {
            Write(LogEventLevel.Info, null, message);
        }

        /// <inheritdoc/>
        public void Warn(string message)
        {
            Write(LogEventLevel.Warn, null, message);
        }

        /// <inheritdoc/>
        public void Error(string message)
        {
            Write(LogEventLevel.Error, null, message);
        }

        /// <inheritdoc/>
        public void Error(Exception exception, string message = null)
        {
            Write(LogEventLevel.Error, exception, message);
        }

        /// <inheritdoc/>
        public void Fatal(string message)
        {
            Write(LogEventLevel.Fatal, null, message);
        }

        /// <inheritdoc/>
        public void Fatal(Exception exception, string message = null)
        {
            Write(LogEventLevel.Fatal, exception, message);
        }
    }
}
