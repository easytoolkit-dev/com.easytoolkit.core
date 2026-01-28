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

        /// <summary>
        /// Calculates a point on a quadratic Bezier curve at parameter t.
        /// </summary>
        /// <param name="p0">The starting point.</param>
        /// <param name="p1">The control point.</param>
        /// <param name="p2">The end point.</param>
        /// <param name="t">The interpolation parameter (usually 0 to 1).</param>
        /// <returns>The point on the curve at t.</returns>
        /// <remarks>
        /// Formula: (1-t)^2 * p0 + 2(1-t)t * p1 + t^2 * p2
        /// </remarks>
        public static Vector3 CalculateQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return uu * p0 + 2 * u * t * p1 + tt * p2;
        }

        /// <summary>
        /// Calculates the first derivative (tangent) of a quadratic Bezier curve at parameter t.
        /// </summary>
        /// <param name="p0">The starting point.</param>
        /// <param name="p1">The control point.</param>
        /// <param name="p2">The end point.</param>
        /// <param name="t">The interpolation parameter (usually 0 to 1).</param>
        /// <returns>The tangent vector at t. Note: This vector is not normalized.</returns>
        /// <remarks>
        /// Formula: 2(1-t)(p1-p0) + 2t(p2-p1)
        /// </remarks>
        public static Vector3 CalculateQuadraticBezierDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
        }

        /// <summary>
        /// Calculates a point on a cubic Bezier curve at parameter t.
        /// </summary>
        /// <param name="p0">The starting point.</param>
        /// <param name="p1">The first control point.</param>
        /// <param name="p2">The second control point.</param>
        /// <param name="p3">The end point.</param>
        /// <param name="t">The interpolation parameter (usually 0 to 1).</param>
        /// <returns>The point on the curve at t.</returns>
        /// <remarks>
        /// Formula: (1-t)^3 * p0 + 3(1-t)^2 * t * p1 + 3(1-t) * t^2 * p2 + t^3 * p3
        /// </remarks>
        public static Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float ttt = t * t * t;
            float uu = u * u;
            float uuu = u * u * u;

            return uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
        }

        /// <summary>
        /// Calculates the first derivative (tangent) of a cubic Bezier curve at parameter t.
        /// </summary>
        /// <param name="p0">The starting point.</param>
        /// <param name="p1">The first control point.</param>
        /// <param name="p2">The second control point.</param>
        /// <param name="p3">The end point.</param>
        /// <param name="t">The interpolation parameter (usually 0 to 1).</param>
        /// <returns>The tangent vector at t. Note: This vector is not normalized.</returns>
        /// <remarks>
        /// Formula: 3(1-t)^2 * (p1-p0) + 6(1-t) * t * (p2-p1) + 3t^2 * (p3-p2)
        /// </remarks>
        public static Vector3 CalculateCubicBezierDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return 3 * uu * (p1 - p0) + 6 * u * t * (p2 - p1) + 3 * tt * (p3 - p2);
        }

        /// <summary>
        /// Estimates the length of a quadratic Bezier curve by dividing it into linear segments.
        /// </summary>
        /// <param name="p0">The starting point.</param>
        /// <param name="p1">The control point.</param>
        /// <param name="p2">The end point.</param>
        /// <param name="segments">The number of segments to use for the estimation. Higher values are more accurate but slower. Default is 10.</param>
        /// <returns>The estimated length of the curve.</returns>
        public static float EstimateQuadraticBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, int segments = 10)
        {
            if (segments < 1) segments = 1;

            float length = 0f;
            Vector3 previousPoint = p0;

            for (int i = 1; i <= segments; i++)
            {
                float t = (float)i / segments;
                Vector3 currentPoint = CalculateQuadraticBezierPoint(p0, p1, p2, t);
                length += Vector3.Distance(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }

            return length;
        }

        /// <summary>
        /// Estimates the length of a cubic Bezier curve by dividing it into linear segments.
        /// </summary>
        /// <param name="p0">The starting point.</param>
        /// <param name="p1">The first control point.</param>
        /// <param name="p2">The second control point.</param>
        /// <param name="p3">The end point.</param>
        /// <param name="segments">The number of segments to use for the estimation. Higher values are more accurate but slower. Default is 10.</param>
        /// <returns>The estimated length of the curve.</returns>
        public static float EstimateCubicBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int segments = 10)
        {
            if (segments < 1) segments = 1;

            float length = 0f;
            Vector3 previousPoint = p0;

            for (int i = 1; i <= segments; i++)
            {
                float t = (float)i / segments;
                Vector3 currentPoint = CalculateCubicBezierPoint(p0, p1, p2, p3, t);
                length += Vector3.Distance(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }

            return length;
        }
    }
}
