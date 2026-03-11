namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Defines the severity levels for log events.
    /// </summary>
    /// <remarks>
    /// Log levels are ordered from least severe (Debug) to most severe (Fatal).
    /// When a minimum level is configured, only events at or above that level will be processed.
    /// </remarks>
    public enum LogEventLevel
    {
        /// <summary>
        /// Detailed debugging information, typically only of interest during development.
        /// </summary>
        Debug = 0,

        /// <summary>
        /// Informational messages that track the general flow of the application.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Warning messages indicating potentially harmful situations or important events.
        /// </summary>
        Warn = 2,

        /// <summary>
        /// Error events that might still allow the application to continue running.
        /// </summary>
        Error = 3,

        /// <summary>
        /// Critical errors that may cause the application to terminate.
        /// </summary>
        Fatal = 4
    }
}
