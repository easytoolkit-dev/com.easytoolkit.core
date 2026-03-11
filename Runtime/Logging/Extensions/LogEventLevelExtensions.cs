using System;
using UnityEngine;

namespace EasyToolkit.Core.Logging
{
    /// <summary>
    /// Provides extension methods for <see cref="LogEventLevel"/>.
    /// </summary>
    public static class LogEventLevelExtensions
    {
        /// <summary>
        /// Converts a <see cref="LogEventLevel"/> to the corresponding Unity <see cref="LogType"/>.
        /// </summary>
        /// <param name="level">The log event level to convert.</param>
        /// <returns>The matching Unity log type.</returns>
        /// <remarks>
        /// The mapping is as follows:
        /// <list type="bullet">
        ///   <item><description>Debug and Info → <see cref="LogType.Log"/></description></item>
        ///   <item><description>Warn → <see cref="LogType.Warning"/></description></item>
        ///   <item><description>Error and Fatal → <see cref="LogType.Error"/></description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="level"/> is not a valid <see cref="LogEventLevel"/> value.
        /// </exception>
        public static LogType ToUnityLogType(this LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Debug:
                case LogEventLevel.Info:
                    return LogType.Log;
                case LogEventLevel.Warn:
                    return LogType.Warning;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    return LogType.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, "The specified log level is not valid.");
            }
        }
    }
}
