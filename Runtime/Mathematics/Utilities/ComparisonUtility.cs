using System;

namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Provides utility methods for evaluating numeric comparisons with support for
    /// approximate equality and configurable comparison modes.
    /// </summary>
    /// <remarks>
    /// This utility class extends basic comparison operations by supporting approximate equality
    /// checks (useful for floating-point precision issues) and providing a unified interface
    /// for different comparison operations through the <see cref="ComparisonMode"/> enum.
    /// </remarks>
    public static class ComparisonUtility
    {
        #region Float Comparison

        /// <summary>
        /// Evaluates the comparison between two floating-point values.
        /// </summary>
        /// <param name="actual">The actual value to evaluate.</param>
        /// <param name="target">The target value to compare against.</param>
        /// <param name="mode">The comparison mode to use.</param>
        /// <returns>True if the comparison evaluates to true, false otherwise.</returns>
        /// <remarks>
        /// For equality-based comparisons (EqualTo, NotEqualTo, LessThanOrEqualTo, GreaterThanOrEqualTo),
        /// this method uses approximate equality to handle floating-point precision issues.
        /// </remarks>
        public static bool Evaluate(float actual, float target, ComparisonMode mode)
        {
            return mode switch
            {
                ComparisonMode.LessThan => actual < target && !actual.IsApproximatelyOf(target),
                ComparisonMode.LessThanOrEqualTo => actual < target || actual.IsApproximatelyOf(target),
                ComparisonMode.EqualTo => actual.IsApproximatelyOf(target),
                ComparisonMode.NotEqualTo => !actual.IsApproximatelyOf(target),
                ComparisonMode.GreaterThanOrEqualTo => actual > target || actual.IsApproximatelyOf(target),
                ComparisonMode.GreaterThan => actual > target && !actual.IsApproximatelyOf(target),
                _ => false
            };
        }

        /// <summary>
        /// Evaluates the comparison between two floating-point values with a custom epsilon.
        /// </summary>
        /// <param name="actual">The actual value to evaluate.</param>
        /// <param name="target">The target value to compare against.</param>
        /// <param name="mode">The comparison mode to use.</param>
        /// <param name="epsilon">The tolerance for approximate equality.</param>
        /// <returns>True if the comparison evaluates to true, false otherwise.</returns>
        /// <remarks>
        /// This method allows specifying a custom epsilon value for approximate equality checks.
        /// Use this when the default Mathf.Approximately tolerance is not suitable for your use case.
        /// </remarks>
        public static bool Evaluate(float actual, float target, ComparisonMode mode, float epsilon)
        {
            return mode switch
            {
                ComparisonMode.LessThan => actual < target && !actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.LessThanOrEqualTo => actual < target || actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.EqualTo => actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.NotEqualTo => !actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.GreaterThanOrEqualTo => actual > target || actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.GreaterThan => actual > target && !actual.IsApproximatelyOf(target, epsilon),
                _ => false
            };
        }

        #endregion

        #region Integer Comparison

        /// <summary>
        /// Evaluates the comparison between two integer values.
        /// </summary>
        /// <param name="actual">The actual value to evaluate.</param>
        /// <param name="target">The target value to compare against.</param>
        /// <param name="mode">The comparison mode to use.</param>
        /// <returns>True if the comparison evaluates to true, false otherwise.</returns>
        /// <remarks>
        /// Integer comparisons use exact equality without epsilon tolerance, as integers
        /// do not suffer from floating-point precision issues.
        /// </remarks>
        public static bool Evaluate(int actual, int target, ComparisonMode mode)
        {
            return mode switch
            {
                ComparisonMode.LessThan => actual < target,
                ComparisonMode.LessThanOrEqualTo => actual <= target,
                ComparisonMode.EqualTo => actual == target,
                ComparisonMode.NotEqualTo => actual != target,
                ComparisonMode.GreaterThanOrEqualTo => actual >= target,
                ComparisonMode.GreaterThan => actual > target,
                _ => false
            };
        }

        #endregion

        #region Double Comparison

        /// <summary>
        /// Evaluates the comparison between two double-precision floating-point values.
        /// </summary>
        /// <param name="actual">The actual value to evaluate.</param>
        /// <param name="target">The target value to compare against.</param>
        /// <param name="mode">The comparison mode to use.</param>
        /// <returns>True if the comparison evaluates to true, false otherwise.</returns>
        /// <remarks>
        /// For equality-based comparisons, this method uses the default approximate equality
        /// check for double values, which handles precision issues with relative epsilon comparison.
        /// </remarks>
        public static bool Evaluate(double actual, double target, ComparisonMode mode)
        {
            return mode switch
            {
                ComparisonMode.LessThan => actual < target && !actual.IsApproximatelyOf(target),
                ComparisonMode.LessThanOrEqualTo => actual < target || actual.IsApproximatelyOf(target),
                ComparisonMode.EqualTo => actual.IsApproximatelyOf(target),
                ComparisonMode.NotEqualTo => !actual.IsApproximatelyOf(target),
                ComparisonMode.GreaterThanOrEqualTo => actual > target || actual.IsApproximatelyOf(target),
                ComparisonMode.GreaterThan => actual > target && !actual.IsApproximatelyOf(target),
                _ => false
            };
        }

        /// <summary>
        /// Evaluates the comparison between two double-precision floating-point values with a custom epsilon.
        /// </summary>
        /// <param name="actual">The actual value to evaluate.</param>
        /// <param name="target">The target value to compare against.</param>
        /// <param name="mode">The comparison mode to use.</param>
        /// <param name="epsilon">The tolerance for approximate equality.</param>
        /// <returns>True if the comparison evaluates to true, false otherwise.</returns>
        public static bool Evaluate(double actual, double target, ComparisonMode mode, double epsilon)
        {
            return mode switch
            {
                ComparisonMode.LessThan => actual < target && !actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.LessThanOrEqualTo => actual < target || actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.EqualTo => actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.NotEqualTo => !actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.GreaterThanOrEqualTo => actual > target || actual.IsApproximatelyOf(target, epsilon),
                ComparisonMode.GreaterThan => actual > target && !actual.IsApproximatelyOf(target, epsilon),
                _ => false
            };
        }

        #endregion

        #region Generic Comparison

        /// <summary>
        /// Evaluates the comparison between two comparable values using exact comparison.
        /// </summary>
        /// <typeparam name="T">The type of values to compare, must implement IComparable.</typeparam>
        /// <param name="actual">The actual value to evaluate.</param>
        /// <param name="target">The target value to compare against.</param>
        /// <param name="mode">The comparison mode to use.</param>
        /// <returns>True if the comparison evaluates to true, false otherwise.</returns>
        /// <remarks>
        /// This generic method works with any type that implements IComparable. It uses exact
        /// comparison without epsilon tolerance, making it suitable for value types and custom
        /// comparable types.
        /// </remarks>
        public static bool Evaluate<T>(T actual, T target, ComparisonMode mode) where T : IComparable<T>
        {
            var comparison = actual.CompareTo(target);

            return mode switch
            {
                ComparisonMode.LessThan => comparison < 0,
                ComparisonMode.LessThanOrEqualTo => comparison <= 0,
                ComparisonMode.EqualTo => comparison == 0,
                ComparisonMode.NotEqualTo => comparison != 0,
                ComparisonMode.GreaterThanOrEqualTo => comparison >= 0,
                ComparisonMode.GreaterThan => comparison > 0,
                _ => false
            };
        }

        #endregion
    }
}
