using System;

namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Defines a logger that records log events at various severity levels.
    /// </summary>
    /// <remarks>
    /// This interface provides methods for logging messages at different severity levels.
    /// Log events are dispatched to configured sinks for output to destinations such as console, files, or external services.
    /// The minimum log level can be configured to filter out messages below a certain severity.
    /// </remarks>
    public interface ILogger
    {
        /// <summary>
        /// Logs a debug-level message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <remarks>
        /// Debug messages are typically used for detailed information during development
        /// and troubleshooting. They are usually disabled in production builds.
        /// </remarks>
        void Debug(string message);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <remarks>
        /// Informational messages track the general flow of the application.
        /// </remarks>
        void Info(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <remarks>
        /// Warning messages indicate potentially harmful situations or important events
        /// that do not prevent the application from continuing.
        /// </remarks>
        void Warn(string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <remarks>
        /// Error messages indicate error events that might still allow the application to continue running.
        /// </remarks>
        void Error(string message);

        /// <summary>
        /// Logs an error message with an associated exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional message providing additional context about the error.</param>
        /// <remarks>
        /// Use this overload when logging caught exceptions to preserve the full stack trace
        /// and exception details for debugging.
        /// </remarks>
        void Error(Exception exception, string message = null);

        /// <summary>
        /// Logs a fatal error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <remarks>
        /// Fatal messages indicate critical errors that may cause the application to terminate.
        /// </remarks>
        void Fatal(string message);

        /// <summary>
        /// Logs a fatal error message with an associated exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional message providing additional context about the fatal error.</param>
        /// <remarks>
        /// Use this overload when logging exceptions that result in application termination
        /// or critical failures.
        /// </remarks>
        void Fatal(Exception exception, string message = null);
    }
}
