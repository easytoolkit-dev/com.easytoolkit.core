using System;
using NUnit.Framework;

namespace EasyToolkit.Core.Logging.Tests
{
    /// <summary>
    /// Unit tests for <see cref="LogEvent"/> model.
    /// </summary>
    public class TestLoggingLogEvent
    {
        #region Constructor Tests

        /// <summary>
        /// Verifies that constructor initializes all properties correctly.
        /// </summary>
        [Test]
        public void Constructor_ValidParameters_InitializesAllProperties()
        {
            // Arrange
            var timestamp = DateTime.Now;
            var level = LogEventLevel.Info;
            var message = "Test message";
            var exception = new InvalidOperationException("Test exception");
            var stackTrace = new System.Diagnostics.StackTrace(true);

            // Act
            var logEvent = new LogEvent(timestamp, level, exception, message, stackTrace);

            // Assert
            Assert.AreEqual(timestamp, logEvent.Timestamp);
            Assert.AreEqual(level, logEvent.Level);
            Assert.AreEqual(exception, logEvent.Exception);
            Assert.AreEqual(message, logEvent.Message);
            Assert.AreEqual(stackTrace, logEvent.StackTrace);
        }

        /// <summary>
        /// Verifies that constructor accepts null exception.
        /// </summary>
        [Test]
        public void Constructor_NullException_SetsExceptionToNull()
        {
            // Arrange
            var timestamp = DateTime.Now;
            var level = LogEventLevel.Debug;
            var message = "Test without exception";
            var stackTrace = new System.Diagnostics.StackTrace(true);

            // Act
            var logEvent = new LogEvent(timestamp, level, null, message, stackTrace);

            // Assert
            Assert.IsNull(logEvent.Exception);
            Assert.AreEqual(message, logEvent.Message);
        }

        #endregion

        #region Property Tests

        /// <summary>
        /// Verifies that Timestamp property returns the value passed to constructor.
        /// </summary>
        [Test]
        public void Timestamp_ConstructedValue_ReturnsSameValue()
        {
            // Arrange
            var expectedTimestamp = DateTime.Now;
            var logEvent = new LogEvent(expectedTimestamp, LogEventLevel.Info, null, "message", new System.Diagnostics.StackTrace(true));

            // Act
            var actualTimestamp = logEvent.Timestamp;

            // Assert
            Assert.AreEqual(expectedTimestamp, actualTimestamp);
        }

        /// <summary>
        /// Verifies that Level property returns the value passed to constructor.
        /// </summary>
        [Test]
        public void Level_ConstructedValue_ReturnsSameValue()
        {
            // Arrange
            var expectedLevel = LogEventLevel.Warn;
            var logEvent = new LogEvent(DateTime.Now, expectedLevel, null, "message", new System.Diagnostics.StackTrace(true));

            // Act
            var actualLevel = logEvent.Level;

            // Assert
            Assert.AreEqual(expectedLevel, actualLevel);
        }

        /// <summary>
        /// Verifies that Message property returns the value passed to constructor.
        /// </summary>
        [Test]
        public void Message_ConstructedValue_ReturnsSameValue()
        {
            // Arrange
            var expectedMessage = "Expected log message";
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Info, null, expectedMessage, new System.Diagnostics.StackTrace(true));

            // Act
            var actualMessage = logEvent.Message;

            // Assert
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        /// <summary>
        /// Verifies that Exception property returns the exception passed to constructor.
        /// </summary>
        [Test]
        public void Exception_ConstructedValue_ReturnsSameException()
        {
            // Arrange
            var expectedException = new ArgumentException("Invalid argument");
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Error, expectedException, "message", new System.Diagnostics.StackTrace(true));

            // Act
            var actualException = logEvent.Exception;

            // Assert
            Assert.AreSame(expectedException, actualException);
        }

        /// <summary>
        /// Verifies that StackTrace property returns the value passed to constructor.
        /// </summary>
        [Test]
        public void StackTrace_ConstructedValue_ReturnsSameStackTrace()
        {
            // Arrange
            var expectedStackTrace = new System.Diagnostics.StackTrace(true);
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Info, null, "message", expectedStackTrace);

            // Act
            var actualStackTrace = logEvent.StackTrace;

            // Assert
            Assert.AreSame(expectedStackTrace, actualStackTrace);
        }

        #endregion

        #region All Log Levels Tests

        /// <summary>
        /// Verifies that LogEvent can be created with Debug level.
        /// </summary>
        [Test]
        public void Constructor_DebugLevel_CreatesEventWithDebugLevel()
        {
            // Arrange & Act
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Debug, null, "debug message", new System.Diagnostics.StackTrace(true));

            // Assert
            Assert.AreEqual(LogEventLevel.Debug, logEvent.Level);
        }

        /// <summary>
        /// Verifies that LogEvent can be created with Info level.
        /// </summary>
        [Test]
        public void Constructor_InfoLevel_CreatesEventWithInfoLevel()
        {
            // Arrange & Act
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Info, null, "info message", new System.Diagnostics.StackTrace(true));

            // Assert
            Assert.AreEqual(LogEventLevel.Info, logEvent.Level);
        }

        /// <summary>
        /// Verifies that LogEvent can be created with Warn level.
        /// </summary>
        [Test]
        public void Constructor_WarnLevel_CreatesEventWithWarnLevel()
        {
            // Arrange & Act
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Warn, null, "warn message", new System.Diagnostics.StackTrace(true));

            // Assert
            Assert.AreEqual(LogEventLevel.Warn, logEvent.Level);
        }

        /// <summary>
        /// Verifies that LogEvent can be created with Error level.
        /// </summary>
        [Test]
        public void Constructor_ErrorLevel_CreatesEventWithErrorLevel()
        {
            // Arrange & Act
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Error, null, "error message", new System.Diagnostics.StackTrace(true));

            // Assert
            Assert.AreEqual(LogEventLevel.Error, logEvent.Level);
        }

        /// <summary>
        /// Verifies that LogEvent can be created with Fatal level.
        /// </summary>
        [Test]
        public void Constructor_FatalLevel_CreatesEventWithFatalLevel()
        {
            // Arrange & Act
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Fatal, null, "fatal message", new System.Diagnostics.StackTrace(true));

            // Assert
            Assert.AreEqual(LogEventLevel.Fatal, logEvent.Level);
        }

        #endregion
    }
}
