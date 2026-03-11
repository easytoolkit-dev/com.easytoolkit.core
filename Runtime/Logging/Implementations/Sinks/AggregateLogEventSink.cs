using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyToolkit.Core.Logging.Implementations
{
    /// <summary>
    /// Aggregates multiple <see cref="ILogEventSink"/> instances and broadcasts log events to all of them.
    /// </summary>
    /// <remarks>
    /// This sink is used internally by <see cref="LoggerConfiguration"/> to combine multiple sink configurations
    /// into a single composite sink that is passed to the <see cref="Logger"/>. When emitting a log event,
    /// it iterates through all registered sinks and calls their <see cref="ILogEventSink.Emit"/> methods.
    /// Exceptions thrown by individual sinks are caught and logged to Unity console without affecting other sinks.
    /// </remarks>
    internal class AggregateLogEventSink : ILogEventSink
    {
        readonly ILogEventSink[] _sinks;

        /// <summary>
        /// Initializes a new instance with the specified collection of sinks.
        /// </summary>
        /// <param name="sinks">The collection of sinks to aggregate. Converted to an array for efficient iteration.</param>
        public AggregateLogEventSink(IEnumerable<ILogEventSink> sinks)
        {
            _sinks = sinks.ToArray();
        }

        /// <summary>
        /// Emits the log event to all aggregated sinks.
        /// </summary>
        /// <param name="logEvent">The log event to emit to all sinks.</param>
        /// <remarks>
        /// If any sink throws an exception during emission, the exception is caught and logged to Unity console.
        /// Processing continues with remaining sinks to ensure fault isolation.
        /// </remarks>
        public void Emit(LogEvent logEvent)
        {
            foreach (var sink in _sinks)
            {
                try
                {
                    sink.Emit(logEvent);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Caught exception while emitting to sink {sink}: {ex}");
                }
            }
        }
    }
}
