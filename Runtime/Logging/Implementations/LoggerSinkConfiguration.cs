using System;

namespace EasyToolkit.Core.Logging.Implementations
{
    /// <summary>
    /// Implementation of <see cref="ILoggerSinkConfiguration"/> for adding log event sinks.
    /// </summary>
    /// <remarks>
    /// This class provides the sink configuration functionality for the fluent builder pattern.
    /// It maintains a reference to the parent configuration and uses a delegate to add sinks
    /// to the configuration's internal collection.
    /// </remarks>
    internal sealed class LoggerSinkConfiguration : ILoggerSinkConfiguration
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly Action<ILogEventSink> _addSink;

        /// <summary>
        /// Initializes a new instance with the specified parent configuration and sink addition delegate.
        /// </summary>
        /// <param name="loggerConfiguration">The parent logger configuration for fluent chaining.</param>
        /// <param name="addSink">The delegate that adds sinks to the configuration's internal collection.</param>
        public LoggerSinkConfiguration(LoggerConfiguration loggerConfiguration, Action<ILogEventSink> addSink)
        {
            _loggerConfiguration = loggerConfiguration;
            _addSink = addSink;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sink"/> is null.</exception>
        public ILoggerConfiguration Sink(ILogEventSink sink)
        {
            if (sink == null)
                throw new ArgumentNullException(nameof(sink));

            _addSink(sink);
            return _loggerConfiguration;
        }
    }
}
