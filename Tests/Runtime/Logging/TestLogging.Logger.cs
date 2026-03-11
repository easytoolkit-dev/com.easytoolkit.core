using System;
using NUnit.Framework;

namespace EasyToolkit.Core.Logging.Tests
{
    /// <summary>
    /// Unit tests for <see cref="ILogger"/> implementation.
    /// </summary>
    public class TestLoggingLogger
    {
        #region Minimum Level Filtering Tests

        /// <summary>
        /// Verifies that Debug messages are filtered out when minimum level is Info.
        /// </summary>
        [Test]
        public void Debug_MinimumLevelInfo_DoesNotEmitEvent()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Info()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Debug("Debug message");

            // Assert
            Assert.AreEqual(0, testSink.Count);
        }

        /// <summary>
        /// Verifies that Info messages are emitted when minimum level is Info.
        /// </summary>
        [Test]
        public void Info_MinimumLevelInfo_EmitsEvent()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Info()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Info("Info message");

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.IsTrue(testSink.HasEventAtLevel(LogEventLevel.Info));
        }

        /// <summary>
        /// Verifies that Warn messages are emitted when minimum level is Info.
        /// </summary>
        [Test]
        public void Warn_MinimumLevelInfo_EmitsEvent()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Info()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Warn("Warning message");

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.IsTrue(testSink.HasEventAtLevel(LogEventLevel.Warn));
        }

        /// <summary>
        /// Verifies that Error messages are emitted when minimum level is Info.
        /// </summary>
        [Test]
        public void Error_MinimumLevelInfo_EmitsEvent()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Info()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Error("Error message");

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.IsTrue(testSink.HasEventAtLevel(LogEventLevel.Error));
        }

        /// <summary>
        /// Verifies that Fatal messages are emitted when minimum level is Info.
        /// </summary>
        [Test]
        public void Fatal_MinimumLevelInfo_EmitsEvent()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Info()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Fatal("Fatal message");

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.IsTrue(testSink.HasEventAtLevel(LogEventLevel.Fatal));
        }

        #endregion

        #region Minimum Level Debug Tests

        /// <summary>
        /// Verifies that all messages are emitted when minimum level is Debug.
        /// </summary>
        [Test]
        public void AllLogLevels_MinimumLevelDebug_EmitsAllEvents()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Debug("Debug message");
            logger.Info("Info message");
            logger.Warn("Warning message");
            logger.Error("Error message");
            logger.Fatal("Fatal message");

            // Assert
            Assert.AreEqual(5, testSink.Count);
            Assert.AreEqual(1, testSink.GetCountAtLevel(LogEventLevel.Debug));
            Assert.AreEqual(1, testSink.GetCountAtLevel(LogEventLevel.Info));
            Assert.AreEqual(1, testSink.GetCountAtLevel(LogEventLevel.Warn));
            Assert.AreEqual(1, testSink.GetCountAtLevel(LogEventLevel.Error));
            Assert.AreEqual(1, testSink.GetCountAtLevel(LogEventLevel.Fatal));
        }

        #endregion

        #region Minimum Level Warn Tests

        /// <summary>
        /// Verifies that Debug messages are filtered out when minimum level is Warn.
        /// </summary>
        [Test]
        public void Debug_MinimumLevelWarn_DoesNotEmitEvent()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Warn()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Debug("Debug message");

            // Assert
            Assert.AreEqual(0, testSink.Count);
        }

        /// <summary>
        /// Verifies that Info messages are filtered out when minimum level is Warn.
        /// </summary>
        [Test]
        public void Info_MinimumLevelWarn_DoesNotEmitEvent()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Warn()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Info("Info message");

            // Assert
            Assert.AreEqual(0, testSink.Count);
        }

        /// <summary>
        /// Verifies that Warn and above messages are emitted when minimum level is Warn.
        /// </summary>
        [Test]
        public void WarnAndAbove_MinimumLevelWarn_EmitsEvents()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Warn()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Warn("Warning message");
            logger.Error("Error message");
            logger.Fatal("Fatal message");

            // Assert
            Assert.AreEqual(3, testSink.Count);
            Assert.AreEqual(0, testSink.GetCountAtLevel(LogEventLevel.Debug));
            Assert.AreEqual(0, testSink.GetCountAtLevel(LogEventLevel.Info));
        }

        #endregion

        #region Log Event Content Tests

        /// <summary>
        /// Verifies that log event contains the correct message.
        /// </summary>
        [Test]
        public void Info_LogMessage_ContainsCorrectMessage()
        {
            // Arrange
            var expectedMessage = "Test log message";
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Info(expectedMessage);

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.AreEqual(expectedMessage, testSink.LastEvent.Message);
        }

        /// <summary>
        /// Verifies that log event contains the correct level.
        /// </summary>
        [Test]
        public void Error_LogLevel_ContainsCorrectLevel()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Error("Error message");

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.AreEqual(LogEventLevel.Error, testSink.LastEvent.Level);
        }

        /// <summary>
        /// Verifies that log event contains valid timestamp.
        /// </summary>
        [Test]
        public void Info_LogEvent_ContainsValidTimestamp()
        {
            // Arrange
            var beforeLog = DateTime.Now;
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Info("Info message");
            var afterLog = DateTime.Now;

            // Assert
            Assert.AreEqual(1, testSink.Count);
            var timestamp = testSink.LastEvent.Timestamp;
            Assert.IsTrue(timestamp >= beforeLog && timestamp <= afterLog.AddSeconds(1));
        }

        #endregion

        #region Exception Logging Tests

        /// <summary>
        /// Verifies that Error with exception captures the exception.
        /// </summary>
        [Test]
        public void Error_WithException_CapturesException()
        {
            // Arrange
            var expectedException = new InvalidOperationException("Test exception");
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Error(expectedException, "Error occurred");

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.AreSame(expectedException, testSink.LastEvent.Exception);
        }

        /// <summary>
        /// Verifies that Error with exception includes the message.
        /// </summary>
        [Test]
        public void Error_WithException_IncludesMessage()
        {
            // Arrange
            var expectedMessage = "Additional error context";
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Error(new InvalidOperationException("Test exception"), expectedMessage);

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.AreEqual(expectedMessage, testSink.LastEvent.Message);
        }

        /// <summary>
        /// Verifies that Error with exception works without message.
        /// </summary>
        [Test]
        public void Error_WithException_NoMessage_LogsException()
        {
            // Arrange
            var expectedException = new ArgumentException("Invalid argument");
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Error(expectedException);

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.AreSame(expectedException, testSink.LastEvent.Exception);
        }

        /// <summary>
        /// Verifies that Fatal with exception captures the exception.
        /// </summary>
        [Test]
        public void Fatal_WithException_CapturesException()
        {
            // Arrange
            var expectedException = new NotImplementedException("Not implemented");
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Fatal(expectedException, "Fatal error occurred");

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.AreSame(expectedException, testSink.LastEvent.Exception);
            Assert.AreEqual(LogEventLevel.Fatal, testSink.LastEvent.Level);
        }

        /// <summary>
        /// Verifies that Fatal with exception works without message.
        /// </summary>
        [Test]
        public void Fatal_WithException_NoMessage_LogsException()
        {
            // Arrange
            var expectedException = new OutOfMemoryException("Out of memory");
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Fatal(expectedException);

            // Assert
            Assert.AreEqual(1, testSink.Count);
            Assert.AreSame(expectedException, testSink.LastEvent.Exception);
            Assert.AreEqual(LogEventLevel.Fatal, testSink.LastEvent.Level);
        }

        #endregion

        #region Multiple Log Calls Tests

        /// <summary>
        /// Verifies that multiple log calls are captured in order.
        /// </summary>
        [Test]
        public void MultipleLogCalls_CaptureInOrder()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Info("First message");
            logger.Warn("Second message");
            logger.Error("Third message");

            // Assert
            Assert.AreEqual(3, testSink.Count);
            Assert.AreEqual(LogEventLevel.Info, testSink.LogEvents[0].Level);
            Assert.AreEqual(LogEventLevel.Warn, testSink.LogEvents[1].Level);
            Assert.AreEqual(LogEventLevel.Error, testSink.LogEvents[2].Level);
        }

        /// <summary>
        /// Verifies that filtered messages do not affect message count.
        /// </summary>
        [Test]
        public void MixedLogLevels_MinimumLevelWarn_OnlyEmitsWarnAndAbove()
        {
            // Arrange
            var testSink = new LoggingTestHelperTypes.TestLogEventSink();
            var logger = LoggerFactory.Configure()
                .MinimumLevel.Warn()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            // Act
            logger.Debug("Debug - filtered");
            logger.Info("Info - filtered");
            logger.Warn("Warning - emitted");
            logger.Error("Error - emitted");

            // Assert
            Assert.AreEqual(2, testSink.Count);
            Assert.IsFalse(testSink.HasEventWithMessage("Debug - filtered"));
            Assert.IsFalse(testSink.HasEventWithMessage("Info - filtered"));
            Assert.IsTrue(testSink.HasEventWithMessage("Warning - emitted"));
            Assert.IsTrue(testSink.HasEventWithMessage("Error - emitted"));
        }

        #endregion

        #region Null Sink Tests

        /// <summary>
        /// Verifies that adding null sink throws ArgumentNullException.
        /// </summary>
        [Test]
        public void WriteTo_NullSink_ThrowsArgumentNullException()
        {
            // Arrange
            var config = LoggerFactory.Configure();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => config.WriteTo.Sink(null));
        }

        #endregion
    }
}
