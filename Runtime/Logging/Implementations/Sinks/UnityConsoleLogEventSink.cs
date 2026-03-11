using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EasyToolkit.Core.Textual;
using UnityEngine;

namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// A log event sink that emits log events to the Unity console.
    /// </summary>
    /// <remarks>
    /// This sink maps <see cref="LogEventLevel"/> to Unity's <see cref="LogType"/> and outputs
    /// log messages using the Unity logger. It includes clickable stack trace links in the console
    /// for easy navigation to the source code location.
    /// </remarks>
    public class UnityConsoleLogEventSink : ILogEventSink
    {
        private readonly UnityEngine.ILogger _unityLogger;

        /// <summary>
        /// Initializes a new instance with the specified Unity logger.
        /// </summary>
        /// <param name="unityLogger">The Unity logger to use for outputting log events.</param>
        public UnityConsoleLogEventSink(UnityEngine.ILogger unityLogger)
        {
            _unityLogger = unityLogger;
        }

        /// <summary>
        /// Emits the log event to the Unity console.
        /// </summary>
        /// <param name="logEvent">The log event to emit.</param>
        /// <remarks>
        /// Maps log levels to Unity log types:
        /// - Debug and Info → <see cref="LogType.Log"/>
        /// - Warn → <see cref="LogType.Warning"/>
        /// - Error and Fatal → <see cref="LogType.Error"/>
        ///
        /// If the log event contains an exception, it is logged using <see cref="UnityEngine.ILogger.LogException"/>.
        /// Otherwise, the message is logged with a clickable stack trace for navigation.
        /// </remarks>
        public void Emit(LogEvent logEvent)
        {
            LogType type;
            switch (logEvent.Level)
            {
                case LogEventLevel.Debug:
                case LogEventLevel.Info:
                    type = LogType.Log;
                    break;
                case LogEventLevel.Warn:
                    type = LogType.Warning;
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    type = LogType.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (logEvent.Exception != null)
            {
                _unityLogger.LogException(logEvent.Exception);
            }
            else
            {
                var stringBuilder = new StringBuilder(logEvent.Message);
                var stackTrack = GetStackTrack();
                if (stackTrack.IsNotNullOrWhiteSpace())
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine(stackTrack);
                    stringBuilder.Append("--------- Unity Stack Track ---------");
                }
                _unityLogger.Log(type, stringBuilder.ToString());
            }
        }

        /// <summary>
        /// Extracts and formats the stack trace from the current call stack.
        /// </summary>
        /// <returns>A formatted string containing clickable links to source code locations.</returns>
        /// <remarks>
        /// Skips logging framework internal frames and starts from the user code that called the log method.
        /// Generates HTML-formatted links that Unity console can render as clickable for navigation.
        /// </remarks>
        private string GetStackTrack()
        {
            var stackTrace = new StackTrace(true);
            var index = GetFrameIndex(stackTrace);
            var frames = stackTrace.GetFrames();

            if (frames == null || index >= frames.Length)
                return null;

            var stringBuilder = new StringBuilder();
            for (int i = index; i < frames.Length; i++)
            {
                var frame = frames[i];
                var method = frame.GetMethod();
                stringBuilder.Append($"{method.DeclaringType.Namespace}.{method.DeclaringType.Name}:");
                stringBuilder.Append(method.Name);

                var fileName = frame.GetFileName();
                var lineNum = frame.GetFileLineNumber();

                stringBuilder.Append(" at (");
                stringBuilder.Append($"<a href=\"{fileName}\" line={lineNum}>");
                stringBuilder.Append($"{fileName}:{lineNum}");
                stringBuilder.AppendLine("</a>)");
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Determines the starting frame index for user code in the stack trace.
        /// </summary>
        /// <param name="stackTrace">The stack trace to analyze.</param>
        /// <returns>The index of the first frame that should be included in the output.</returns>
        /// <remarks>
        /// Skips frames without source file information and internal logging framework frames
        /// to show only the relevant user code call stack.
        /// </remarks>
        private int GetFrameIndex(StackTrace stackTrace)
        {
            var frames = stackTrace.GetFrames();
            if (frames == null)
                return 0;

            for (int i = 0; i < frames.Length; i++)
            {
                var frame = frames[i];
                var fileName = frame.GetFileName();
                if (fileName.IsNullOrEmpty())
                {
                    return i;
                }

                if (fileName.Contains("com.easytoolkit.core\\Runtime\\Logging\\Log.cs"))
                {
                    return i + 1;
                }
            }

            return 0;
        }
    }
}
