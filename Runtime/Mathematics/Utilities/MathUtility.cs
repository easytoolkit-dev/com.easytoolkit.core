using UnityEngine;

namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Provides extended mathematical utility functions for value mapping and transformation.
    /// </summary>
    public static class MathUtility
    {
        /// <summary>
        /// Remaps a value from one range to another using linear interpolation.
        /// </summary>
        /// <param name="value">The input value to remap.</param>
        /// <param name="inMin">The minimum value of the input range.</param>
        /// <param name="inMax">The maximum value of the input range.</param>
        /// <param name="outMin">The minimum value of the output range.</param>
        /// <param name="outMax">The maximum value of the output range.</param>
        /// <returns>The remapped value corresponding to the output range.</returns>
        /// <remarks>
        /// This method performs an unbounded linear mapping. Values outside the input range
        /// will be extrapolated outside the output range. Use <see cref="RemapClamped"/> to
        /// constrain results to the output range.
        /// <br/>
        /// Formula: result = outMin + (value - inMin) / (inMax - inMin) * (outMax - outMin)
        /// </remarks>
        public static float Remap(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (value - inMin) / (inMax - inMin) * (outMax - outMin);
        }

        /// <summary>
        /// Remaps a value from one range to another with clamping to the output range.
        /// </summary>
        /// <param name="value">The input value to remap.</param>
        /// <param name="inMin">The minimum value of the input range.</param>
        /// <param name="inMax">The maximum value of the input range.</param>
        /// <param name="outMin">The minimum value of the output range.</param>
        /// <param name="outMax">The maximum value of the output range.</param>
        /// <returns>The clamped and remapped value within the output range.</returns>
        /// <remarks>
        /// Unlike <see cref="Remap"/>, this method constrains the result to the output range.
        /// Values outside the input range will be clamped to the nearest output boundary.
        /// This is equivalent to applying <see cref="Mathf.InverseLerp"/> followed by <see cref="Mathf.Lerp"/>.
        /// </remarks>
        public static float RemapClamped(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return Mathf.Lerp(outMin, outMax, Mathf.InverseLerp(inMin, inMax, value));
        }
    }
}
