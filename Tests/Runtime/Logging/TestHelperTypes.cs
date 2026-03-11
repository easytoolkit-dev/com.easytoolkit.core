using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Logging.Tests
{
    /// <summary>
    /// Test helper types for logging tests.
    /// </summary>
    public static class LoggingTestHelperTypes
    {
        /// <summary>
        /// A test sink that captures log events for inspection in unit tests.
        /// </summary>
        public class TestLogEventSink : ILogEventSink
        {
            private readonly List<LogEvent> _logEvents = new();

            /// <summary>
            /// Gets all log events captured by this sink.
            /// </summary>
            public IReadOnlyList<LogEvent> LogEvents => _logEvents;

            /// <summary>
            /// Gets the number of log events captured.
            /// </summary>
            public int Count => _logEvents.Count;

            /// <summary>
            /// Gets the most recent log event.
            /// </summary>
            public LogEvent LastEvent => _logEvents.Count > 0 ? _logEvents[_logEvents.Count - 1] : null;

            /// <summary>
            /// Emits and captures the log event.
            /// </summary>
            public void Emit(LogEvent logEvent)
            {
                _logEvents.Add(logEvent);
            }

            /// <summary>
            /// Clears all captured log events.
            /// </summary>
            public void Clear()
            {
                _logEvents.Clear();
            }

            /// <summary>
            /// Checks if any log event matches the specified level.
            /// </summary>
            public bool HasEventAtLevel(LogEventLevel level)
            {
                foreach (var logEvent in _logEvents)
                {
                    if (logEvent.Level == level)
                        return true;
                }
                return false;
            }

            /// <summary>
            /// Checks if any log event contains the specified message.
            /// </summary>
            public bool HasEventWithMessage(string message)
            {
                foreach (var logEvent in _logEvents)
                {
                    if (logEvent.Message == message)
                        return true;
                }
                return false;
            }

            /// <summary>
            /// Gets the count of log events at the specified level.
            /// </summary>
            public int GetCountAtLevel(LogEventLevel level)
            {
                var count = 0;
                foreach (var logEvent in _logEvents)
                {
                    if (logEvent.Level == level)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Exception type for testing exception logging.
        /// </summary>
        public class TestException : Exception
        {
            public TestException(string message) : base(message)
            {
            }

            public TestException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }
}
