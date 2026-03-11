using System;
using NUnit.Framework;

namespace EasyToolkit.Core.Logging.Tests
{
    /// <summary>
    /// Unit tests for <see cref="Log"/> static class.
    /// </summary>
    public class TestLoggingLog
    {
        #region Setup and Teardown

        /// <summary>
        /// Stores the original logger to restore after each test.
        /// </summary>
        private ILogger _originalLogger;

        /// <summary>
        /// Stores the test sink for inspection.
        /// </summary>
        private LoggingTestHelperTypes.TestLogEventSink _testSink;

        /// <summary>
        /// Saves the original logger and sets up a test logger before each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _originalLogger = Log.Logger;
            _testSink = new LoggingTestHelperTypes.TestLogEventSink();
            Log.Logger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(_testSink)
                .CreateLogger();
        }

        /// <summary>
        /// Restores the original logger after each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Log.Logger = _originalLogger;
        }

        #endregion

        #region Debug Method Tests

        /// <summary>
        /// Verifies that Debug logs a message at Debug level.
        /// </summary>
        [Test]
        public void Debug_Message_EmitsDebugEvent()
        {
            // Arrange
            var message = "Debug message";

            // Act
            Log.Debug(message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Debug, _testSink.LastEvent.Level);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        #endregion

        #region Info Method Tests

        /// <summary>
        /// Verifies that Info logs a message at Info level.
        /// </summary>
        [Test]
        public void Info_Message_EmitsInfoEvent()
        {
            // Arrange
            var message = "Info message";

            // Act
            Log.Info(message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Info, _testSink.LastEvent.Level);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        #endregion

        #region Warn Method Tests

        /// <summary>
        /// Verifies that Warn logs a message at Warn level.
        /// </summary>
        [Test]
        public void Warn_Message_EmitsWarnEvent()
        {
            // Arrange
            var message = "Warning message";

            // Act
            Log.Warn(message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Warn, _testSink.LastEvent.Level);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        #endregion

        #region Error Method Tests

        /// <summary>
        /// Verifies that Error logs a message at Error level.
        /// </summary>
        [Test]
        public void Error_Message_EmitsErrorEvent()
        {
            // Arrange
            var message = "Error message";

            // Act
            Log.Error(message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Error, _testSink.LastEvent.Level);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        /// <summary>
        /// Verifies that Error with exception captures the exception.
        /// </summary>
        [Test]
        public void Error_ExceptionAndMessage_EmitsErrorEventWithException()
        {
            // Arrange
            var exception = new InvalidOperationException("Test exception");
            var message = "Error occurred";

            // Act
            Log.Error(exception, message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Error, _testSink.LastEvent.Level);
            Assert.AreSame(exception, _testSink.LastEvent.Exception);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        /// <summary>
        /// Verifies that Error with exception works without message.
        /// </summary>
        [Test]
        public void Error_ExceptionOnly_EmitsErrorEventWithException()
        {
            // Arrange
            var exception = new ArgumentException("Invalid argument");

            // Act
            Log.Error(exception);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Error, _testSink.LastEvent.Level);
            Assert.AreSame(exception, _testSink.LastEvent.Exception);
        }

        /// <summary>
        /// Verifies that Error with null exception and message works correctly.
        /// </summary>
        [Test]
        public void Error_NullException_EmitsErrorEventWithoutException()
        {
            // Arrange
            var message = "Error without exception";

            // Act
            Log.Error(null, message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Error, _testSink.LastEvent.Level);
            Assert.IsNull(_testSink.LastEvent.Exception);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        #endregion

        #region Fatal Method Tests

        /// <summary>
        /// Verifies that Fatal logs a message at Fatal level.
        /// </summary>
        [Test]
        public void Fatal_Message_EmitsFatalEvent()
        {
            // Arrange
            var message = "Fatal message";

            // Act
            Log.Fatal(message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Fatal, _testSink.LastEvent.Level);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        /// <summary>
        /// Verifies that Fatal with exception captures the exception.
        /// </summary>
        [Test]
        public void Fatal_ExceptionAndMessage_EmitsFatalEventWithException()
        {
            // Arrange
            var exception = new NotImplementedException("Not implemented");
            var message = "Fatal error occurred";

            // Act
            Log.Fatal(exception, message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Fatal, _testSink.LastEvent.Level);
            Assert.AreSame(exception, _testSink.LastEvent.Exception);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        /// <summary>
        /// Verifies that Fatal with exception works without message.
        /// </summary>
        [Test]
        public void Fatal_ExceptionOnly_EmitsFatalEventWithException()
        {
            // Arrange
            var exception = new OutOfMemoryException("Out of memory");

            // Act
            Log.Fatal(exception);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Fatal, _testSink.LastEvent.Level);
            Assert.AreSame(exception, _testSink.LastEvent.Exception);
        }

        /// <summary>
        /// Verifies that Fatal with null exception and message works correctly.
        /// </summary>
        [Test]
        public void Fatal_NullException_EmitsFatalEventWithoutException()
        {
            // Arrange
            var message = "Fatal without exception";

            // Act
            Log.Fatal(null, message);

            // Assert
            Assert.AreEqual(1, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Fatal, _testSink.LastEvent.Level);
            Assert.IsNull(_testSink.LastEvent.Exception);
            Assert.AreEqual(message, _testSink.LastEvent.Message);
        }

        #endregion

        #region Null Logger Tests

        /// <summary>
        /// Verifies that calling Debug with null logger throws NullReferenceException.
        /// </summary>
        [Test]
        public void Debug_NullLogger_ThrowsNullReferenceException()
        {
            // Arrange
            Log.Logger = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => Log.Debug("Test message"));
        }

        /// <summary>
        /// Verifies that calling Info with null logger throws NullReferenceException.
        /// </summary>
        [Test]
        public void Info_NullLogger_ThrowsNullReferenceException()
        {
            // Arrange
            Log.Logger = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => Log.Info("Test message"));
        }

        /// <summary>
        /// Verifies that calling Warn with null logger throws NullReferenceException.
        /// </summary>
        [Test]
        public void Warn_NullLogger_ThrowsNullReferenceException()
        {
            // Arrange
            Log.Logger = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => Log.Warn("Test message"));
        }

        /// <summary>
        /// Verifies that calling Error with null logger throws NullReferenceException.
        /// </summary>
        [Test]
        public void Error_NullLogger_ThrowsNullReferenceException()
        {
            // Arrange
            Log.Logger = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => Log.Error("Test message"));
        }

        /// <summary>
        /// Verifies that calling Fatal with null logger throws NullReferenceException.
        /// </summary>
        [Test]
        public void Fatal_NullLogger_ThrowsNullReferenceException()
        {
            // Arrange
            Log.Logger = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => Log.Fatal("Test message"));
        }

        #endregion

        #region Level Filtering Tests

        /// <summary>
        /// Verifies that Debug messages are filtered when minimum level is Info.
        /// </summary>
        [Test]
        public void Debug_MinimumLevelInfo_DoesNotEmit()
        {
            // Arrange
            _testSink.Clear();
            Log.Logger = LoggerFactory.Configure()
                .MinimumLevel.Info()
                .WriteTo.Sink(_testSink)
                .CreateLogger();

            // Act
            Log.Debug("Debug message");

            // Assert
            Assert.AreEqual(0, _testSink.Count);
        }

        /// <summary>
        /// Verifies that Info and above are emitted when minimum level is Info.
        /// </summary>
        [Test]
        public void InfoAndAbove_MinimumLevelInfo_EmitsAll()
        {
            // Arrange
            _testSink.Clear();
            Log.Logger = LoggerFactory.Configure()
                .MinimumLevel.Info()
                .WriteTo.Sink(_testSink)
                .CreateLogger();

            // Act
            Log.Info("Info message");
            Log.Warn("Warning message");
            Log.Error("Error message");
            Log.Fatal("Fatal message");

            // Assert
            Assert.AreEqual(4, _testSink.Count);
        }

        #endregion

        #region Multiple Log Calls Tests

        /// <summary>
        /// Verifies that multiple log calls are captured in order.
        /// </summary>
        [Test]
        public void MultipleLogCalls_CaptureInOrder()
        {
            // Act
            Log.Debug("Debug message");
            Log.Info("Info message");
            Log.Warn("Warning message");
            Log.Error("Error message");
            Log.Fatal("Fatal message");

            // Assert
            Assert.AreEqual(5, _testSink.Count);
            Assert.AreEqual(LogEventLevel.Debug, _testSink.LogEvents[0].Level);
            Assert.AreEqual(LogEventLevel.Info, _testSink.LogEvents[1].Level);
            Assert.AreEqual(LogEventLevel.Warn, _testSink.LogEvents[2].Level);
            Assert.AreEqual(LogEventLevel.Error, _testSink.LogEvents[3].Level);
            Assert.AreEqual(LogEventLevel.Fatal, _testSink.LogEvents[4].Level);
        }

        #endregion

        #region Logger Property Tests

        /// <summary>
        /// Verifies that Logger property can be set and retrieved.
        /// </summary>
        [Test]
        public void Logger_PropertyCanBeSetAndRetrieved()
        {
            // Arrange
            var testLogger = LoggerFactory.Configure()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new LoggingTestHelperTypes.TestLogEventSink())
                .CreateLogger();

            // Act
            Log.Logger = testLogger;
            var retrieved = Log.Logger;

            // Assert
            Assert.AreSame(testLogger, retrieved);
        }

        #endregion
    }
}
