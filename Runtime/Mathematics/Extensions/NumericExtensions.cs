using System;
using UnityEngine;

namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Provides extension methods for numeric types to enable fluent API operations for common mathematical calculations.
    /// </summary>
    public static class NumericExtensions
    {
        #region Absolute Value

        /// <summary>
        /// Returns the absolute value of the integer.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>The absolute value.</returns>
        public static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value of the float.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>The absolute value.</returns>
        public static float Abs(this float value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value of the long.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>The absolute value.</returns>
        public static long Abs(this long value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value of the double.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>The absolute value.</returns>
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        #endregion

        #region Clamping

        /// <summary>
        /// Clamps the integer value between a minimum and a maximum value.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(this int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }

        /// <summary>
        /// Clamps the float value between a minimum and a maximum value.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(this float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        /// <summary>
        /// Clamps the value between 0 and 1.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>The clamped value between 0 and 1.</returns>
        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }

        #endregion

        #region Range Checks

        /// <summary>
        /// Checks if the integer value is between the minimum and maximum values (inclusive).
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>True if the value is between min and max (inclusive); otherwise, false.</returns>
        public static bool IsBetween(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Checks if the float value is between the minimum and maximum values (inclusive).
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>True if the value is between min and max (inclusive); otherwise, false.</returns>
        public static bool IsBetween(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        #endregion
    }
}
