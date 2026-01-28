using UnityEngine;

namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Provides extension methods for Gradient operations.
    /// </summary>
    public static class GradientExtensions
    {
        /// <summary>
        /// Evaluates the gradient at a specific time within a duration.
        /// </summary>
        /// <param name="gradient">The gradient to evaluate.</param>
        /// <param name="time">The current time position.</param>
        /// <param name="duration">The total duration to map the gradient over.</param>
        /// <returns>The color at the evaluated time position.</returns>
        /// <remarks>
        /// The time value is remapped from [0, duration] to [0, 1] for gradient evaluation.
        /// For example, with duration=10 and time=5, the gradient is evaluated at position 0.5.
        /// </remarks>
        public static Color EvaluateAtTime(this Gradient gradient, float time, float duration)
        {
            return gradient.Evaluate(MathUtility.Remap(time, 0f, duration, 0f, 1f));
        }
    }
}
