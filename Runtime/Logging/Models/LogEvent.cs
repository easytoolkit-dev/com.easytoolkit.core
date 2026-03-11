using System;
using System.Diagnostics;

namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Represents a log event containing timestamp, level, message, and optional diagnostic information.
    /// </summary>
    /// <remarks>
    /// Log events are created by <see cref="ILogger"/> implementations and passed to
    /// <see cref="ILogEventSink"/> instances for output. They contain all the information
    /// needed to format and emit a log message, including stack trace for debugging.
    /// </remarks>
    public sealed class LogEvent
    {
        /// <summary>
        /// Initializes a new instance with the specified properties.
        /// </summary>
        /// <param name="timestamp">The timestamp when the log event occurred.</param>
        /// <param name="level">The severity level of the log event.</param>
        /// <param name="exception">The optional exception associated with the log event.</param>
        /// <param name="message">The log message.</param>
        /// <param name="stackTrace">The stack trace at the point where the log was created.</param>
        public LogEvent(DateTime timestamp, LogEventLevel level, Exception exception, string message, StackTrace stackTrace)
        {
            Timestamp = timestamp;
            Level = level;
            Exception = exception;
            Message = message;
            StackTrace = stackTrace;
        }

        /// <summary>
        /// Gets the timestamp when the log event occurred.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the severity level of the log event.
        /// </summary>
        public LogEventLevel Level { get; }

        /// <summary>
        /// Gets the optional exception associated with the log event.
        /// </summary>
        /// <remarks>
        /// This property is null when logging a simple message without an exception.
        /// </remarks>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the log message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the stack trace captured at the point where the log was created.
        /// </summary>
        /// <remarks>
        /// This can be used by sinks to provide clickable links to source code locations
        /// in the Unity console or other debugging tools.
        /// </remarks>
        public StackTrace StackTrace { get; }
    }
}
