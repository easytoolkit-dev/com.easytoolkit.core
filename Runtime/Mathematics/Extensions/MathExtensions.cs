using System;
using UnityEngine;

namespace EasyToolKit.Core.Mathematics
{
    /// <summary>
    /// Provides extension methods for mathematical operations including approximate comparisons,
    /// rounding with tolerance, and floating-point equality checks.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class extends common mathematical types with approximately comparison methods that
    /// handle floating-point precision issues. All approximately methods support custom epsilon
    /// values or use sensible defaults.
    /// </para>
    /// <para>
    /// The floor and ceiling approximately methods round values only when they are within epsilon
    /// distance of an integer, providing tolerance-aware rounding operations.
    /// </para>
    /// </remarks>
    public static class MathExtensions
    {
        #region Rounding Operations

        /// <summary>
        /// Floors a floating-point value to the nearest integer if it is within epsilon distance.
        /// </summary>
        /// <param name="value">The value to floor.</param>
        /// <param name="epsilon">The tolerance for considering the value as rounded. When null, uses Mathf.Approximately default.</param>
        /// <returns>The rounded value if within epsilon of an integer, otherwise the floored value.</returns>
        /// <remarks>
        /// This method is useful for handling floating-point precision issues where a value like 2.9999999f
        /// should be treated as 3.0f. If the value is approximately equal to its rounded counterpart,
        /// the rounded value is returned; otherwise, Mathf.Floor is applied.
        /// </remarks>
        public static float FloorApproximately(this float value, float? epsilon = null)
        {
            var rounded = Mathf.Round(value);
            if (value.IsApproximatelyOf(rounded, epsilon))
            {
                return rounded;
            }
            else
            {
                return Mathf.Floor(value);
            }
        }

        /// <summary>
        /// Ceils a floating-point value to the nearest integer if it is within epsilon distance.
        /// </summary>
        /// <param name="value">The value to ceil.</param>
        /// <param name="epsilon">The tolerance for considering the value as rounded. When null, uses Mathf.Approximately default.</param>
        /// <returns>The rounded value if within epsilon of an integer, otherwise the ceiled value.</returns>
        /// <remarks>
        /// This method is useful for handling floating-point precision issues where a value like 3.0000001f
        /// should be treated as 3.0f. If the value is approximately equal to its rounded counterpart,
        /// the rounded value is returned; otherwise, Mathf.Ceil is applied.
        /// </remarks>
        public static float CeilApproximately(this float value, float? epsilon = null)
        {
            var rounded = Mathf.Round(value);
            if (value.IsApproximatelyOf(rounded, epsilon))
            {
                return rounded;
            }
            else
            {
                return Mathf.Ceil(value);
            }
        }

        /// <summary>
        /// Floors a floating-point value to integer if it is within epsilon distance, then converts to int.
        /// </summary>
        /// <param name="value">The value to floor and convert.</param>
        /// <param name="epsilon">The tolerance for considering the value as rounded. When null, uses Mathf.Approximately default.</param>
        /// <returns>The integer value after approximately-aware flooring.</returns>
        public static int FloorToIntApproximately(this float value, float? epsilon = null)
        {
            return (int)value.FloorApproximately(epsilon);
        }

        /// <summary>
        /// Ceils a floating-point value to integer if it is within epsilon distance, then converts to int.
        /// </summary>
        /// <param name="value">The value to ceil and convert.</param>
        /// <param name="epsilon">The tolerance for considering the value as rounded. When null, uses Mathf.Approximately default.</param>
        /// <returns>The integer value after approximately-aware ceiling.</returns>
        public static int CeilToIntApproximately(this float value, float? epsilon = null)
        {
            return (int)value.CeilApproximately(epsilon);
        }

        #endregion

        #region Comparison

        /// <summary>
        /// Determines whether two floating-point values are approximately equal.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        /// <param name="epsilon">The maximum allowed difference between values. When null, uses Mathf.Approximately default.</param>
        /// <returns>True if the values are approximately equal, false otherwise.</returns>
        /// <remarks>
        /// When epsilon is null, this method uses Unity's Mathf.Approximately which uses a small
        /// internal epsilon value. When epsilon is specified, it uses absolute difference comparison.
        /// </remarks>
        public static bool IsApproximatelyOf(this float a, float b, float? epsilon = null)
        {
            if (epsilon == null)
            {
                return Mathf.Approximately(a, b);
            }
            else
            {
                return Mathf.Abs(a - b) < epsilon.Value;
            }
        }

        /// <summary>
        /// Determines whether two double-precision floating-point values are approximately equal.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        /// <param name="epsilon">The maximum allowed difference between values. When null, uses relative epsilon.</param>
        /// <returns>True if the values are approximately equal, false otherwise.</returns>
        /// <remarks>
        /// <para>
        /// When epsilon is null, this method uses a relative epsilon approach suitable for double precision.
        /// For very small values (magnitude less than 1e-6), absolute epsilon comparison is used.
        /// For larger values, relative epsilon comparison is used (difference relative to the magnitude).
        /// </para>
        /// <para>
        /// This approach handles the full range of double values without overflow issues and provides
        /// appropriate tolerance scaling for values of different magnitudes.
        /// </para>
        /// </remarks>
        public static bool IsApproximatelyOf(this double a, double b, double? epsilon = null)
        {
            if (epsilon == null)
            {
                const double defaultEpsilon = 1e-6d;

                double max = Math.Max(Math.Abs(a), Math.Abs(b));
                if (max < defaultEpsilon)
                    return Math.Abs(a - b) < defaultEpsilon;

                return Math.Abs(a - b) < max * defaultEpsilon;
            }
            else
            {
                return Math.Abs(a - b) < epsilon.Value;
            }
        }

        /// <summary>
        /// Determines whether two Quaternion values are approximately equal based on dot product similarity.
        /// </summary>
        /// <param name="a">The first Quaternion to compare.</param>
        /// <param name="b">The second Quaternion to compare.</param>
        /// <param name="similarityThreshold">The similarity threshold from 0 to 1, where 1 is identical. Default is 0.99f.</param>
        /// <returns>True if the quaternions represent approximately the same rotation, false otherwise.</returns>
        /// <remarks>
        /// This method uses the dot product of normalized quaternions to determine rotational similarity.
        /// A threshold of 0.99 means the rotations must be within approximately 8 degrees of each other.
        /// The threshold is clamped to the range [0, 1].
        /// </remarks>
        public static bool IsApproximatelyOf(this Quaternion a, Quaternion b, float similarityThreshold = 0.99f)
        {
            var dot = Quaternion.Dot(a, b);
            var threshold = Mathf.Clamp(similarityThreshold, 0f, 1f);
            return dot >= threshold;
        }

        /// <summary>
        /// Determines whether two Vector2 values are approximately equal based on distance similarity.
        /// </summary>
        /// <param name="a">The first Vector2 to compare.</param>
        /// <param name="b">The second Vector2 to compare.</param>
        /// <param name="similarityThreshold">The similarity threshold from 0 to 1, where 1 is identical. Default is 0.99f.</param>
        /// <returns>True if the vectors are approximately equal, false otherwise.</returns>
        /// <remarks>
        /// The method uses the normalized distance between vectors. A threshold of 0.99 means the vectors
        /// must be within 1% of being identical (distance &lt;= 0.01). This is useful for comparing 2D positions
        /// that may have minor floating-point precision differences. The threshold is clamped to the range [0, 1].
        /// </remarks>
        public static bool IsApproximatelyOf(this Vector2 a, Vector2 b, float similarityThreshold = 0.99f)
        {
            var distance = Vector2.Distance(a, b);
            var threshold = Mathf.Clamp(similarityThreshold, 0f, 1f);
            return distance <= 1 - threshold;
        }

        /// <summary>
        /// Determines whether two Vector3 values are approximately equal based on distance similarity.
        /// </summary>
        /// <param name="a">The first Vector3 to compare.</param>
        /// <param name="b">The second Vector3 to compare.</param>
        /// <param name="similarityThreshold">The similarity threshold from 0 to 1, where 1 is identical. Default is 0.99f.</param>
        /// <returns>True if the vectors are approximately equal, false otherwise.</returns>
        /// <remarks>
        /// The method uses the normalized distance between vectors. A threshold of 0.99 means the vectors
        /// must be within 1% of being identical (distance &lt;= 0.01). This is useful for comparing 3D positions
        /// that may have minor floating-point precision differences. The threshold is clamped to the range [0, 1].
        /// </remarks>
        public static bool IsApproximatelyOf(this Vector3 a, Vector3 b, float similarityThreshold = 0.99f)
        {
            var distance = Vector3.Distance(a, b);
            var threshold = Mathf.Clamp(similarityThreshold, 0f, 1f);
            return distance <= 1 - threshold;
        }

        /// <summary>
        /// Determines whether two Vector4 values are approximately equal based on distance similarity.
        /// </summary>
        /// <param name="a">The first Vector4 to compare.</param>
        /// <param name="b">The second Vector4 to compare.</param>
        /// <param name="similarityThreshold">The similarity threshold from 0 to 1, where 1 is identical. Default is 0.99f.</param>
        /// <returns>True if the vectors are approximately equal, false otherwise.</returns>
        /// <remarks>
        /// The method uses the normalized distance between vectors. A threshold of 0.99 means the vectors
        /// must be within 1% of being identical (distance &lt;= 0.01). This is useful for comparing 4D values
        /// that may have minor floating-point precision differences. The threshold is clamped to the range [0, 1].
        /// </remarks>
        public static bool IsApproximatelyOf(this Vector4 a, Vector4 b, float similarityThreshold = 0.99f)
        {
            var distance = Vector4.Distance(a, b);
            var threshold = Mathf.Clamp(similarityThreshold, 0f, 1f);
            return distance <= 1 - threshold;
        }

        #endregion
    }
}
