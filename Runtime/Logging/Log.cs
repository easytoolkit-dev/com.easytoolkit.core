using System;

namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides static access to logging functionality.
    /// </summary>
    /// <remarks>
    /// This class provides a convenient static accessor for logging throughout the application.
    /// Set the <see cref="Logger"/> property with a configured logger instance before use.
    /// All methods delegate to the configured logger.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Initialize during application startup
    /// Log.Logger = LoggerFactory.Configure()
    ///     .MinimumLevel.Info()
    ///     .WriteTo.UnityConsole()
    ///     .CreateLogger();
    ///
    /// // Use anywhere in the application
    /// Log.Info("Application started");
    /// Log.Error(ex, "An error occurred");
    /// </code>
    /// </example>
    public static class Log
    {
        /// <summary>
        /// Gets or sets the logger instance used for all static logging methods.
        /// </summary>
        /// <remarks>
        /// This property must be set before calling any logging methods. Typically configured
        /// during application initialization.
        /// </remarks>
        public static ILogger Logger { get; set; }

        /// <summary>
        /// Logs a debug-level message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Info(string message)
        {
            Logger.Info(message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Error(string message)
        {
            Logger.Error(message);
        }

        /// <summary>
        /// Logs an error message with an associated exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional message providing additional context about the error.</param>
        public static void Error(Exception exception, string message = null)
        {
            Logger.Error(exception, message);
        }

        /// <summary>
        /// Logs a fatal error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        /// <summary>
        /// Logs a fatal error message with an associated exception.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">An optional message providing additional context about the fatal error.</param>
        public static void Fatal(Exception exception, string message = null)
        {
            Logger.Fatal(exception, message);
        }
    }
}
