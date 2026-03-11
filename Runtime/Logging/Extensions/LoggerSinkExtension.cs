namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides extension methods for configuring log sinks.
    /// </summary>
    /// <remarks>
    /// Extension methods in this class provide convenient syntax for adding specific
    /// sink types to the logger configuration through the <see cref="ILoggerSinkConfiguration"/>
    /// fluent interface.
    /// </remarks>
    public static class LoggerSinkExtension
    {
        /// <summary>
        /// Adds a Unity console sink to the logger configuration.
        /// </summary>
        /// <param name="sinkConfiguration">The sink configuration object.</param>
        /// <returns>The logger configuration for chaining additional configuration calls.</returns>
        /// <remarks>
        /// This creates a <see cref="UnityConsoleLogEventSink"/> that outputs log events
        /// to the Unity console. Log levels are mapped to Unity's <see cref="LogType"/>:
        /// Debug and Info → Log, Warn → Warning, Error and Fatal → Error.
        /// </remarks>
        public static ILoggerConfiguration UnityConsole(this ILoggerSinkConfiguration sinkConfiguration)
        {
            return sinkConfiguration.Sink(new UnityConsoleLogEventSink(UnityEngine.Debug.unityLogger));
        }
    }
}
