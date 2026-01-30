using NUnit.Framework;
using UnityEngine;

namespace EasyToolkit.Core.Mathematics.Tests
{
    /// <summary>
    /// Unit tests for MathUtility components.
    /// </summary>
    public class TestMathUtility
    {
        #region Remap Tests

        /// <summary>
        /// Verifies that Remap maps value from input range to output range correctly.
        /// </summary>
        [Test]
        public void Remap_ValueInRange_ReturnsCorrectMappedValue()
        {
            // Arrange
            float value = 5f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 0f;
            float outMax = 100f;

            // Act
            float result = MathUtility.Remap(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(50f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that Remap maps minimum input to minimum output.
        /// </summary>
        [Test]
        public void Remap_ValueAtInMin_ReturnsOutMin()
        {
            // Arrange
            float value = 0f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 100f;
            float outMax = 200f;

            // Act
            float result = MathUtility.Remap(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(100f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that Remap maps maximum input to maximum output.
        /// </summary>
        [Test]
        public void Remap_ValueAtInMax_ReturnsOutMax()
        {
            // Arrange
            float value = 10f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 100f;
            float outMax = 200f;

            // Act
            float result = MathUtility.Remap(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(200f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that Remap extrapolates values outside input range.
        /// </summary>
        [Test]
        public void Remap_ValueAboveRange_ExtrapolatesAboveOutputRange()
        {
            // Arrange
            float value = 15f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 0f;
            float outMax = 100f;

            // Act
            float result = MathUtility.Remap(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(150f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that Remap extrapolates values below input range.
        /// </summary>
        [Test]
        public void Remap_ValueBelowRange_ExtrapolatesBelowOutputRange()
        {
            // Arrange
            float value = -5f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 0f;
            float outMax = 100f;

            // Act
            float result = MathUtility.Remap(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(-50f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that Remap handles reversed input ranges.
        /// </summary>
        [Test]
        public void Remap_ReversedInputRange_WorksCorrectly()
        {
            // Arrange
            float value = 5f;
            float inMin = 10f;
            float inMax = 0f;
            float outMin = 0f;
            float outMax = 100f;

            // Act
            float result = MathUtility.Remap(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(50f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that Remap handles reversed output ranges.
        /// </summary>
        [Test]
        public void Remap_ReversedOutputRange_InvertsMapping()
        {
            // Arrange
            float value = 5f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 100f;
            float outMax = 0f;

            // Act
            float result = MathUtility.Remap(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(50f, result, 0.0001f);
        }

        #endregion

        #region RemapClamped Tests

        /// <summary>
        /// Verifies that RemapClamped maps value within range correctly.
        /// </summary>
        [Test]
        public void RemapClamped_ValueInRange_ReturnsCorrectMappedValue()
        {
            // Arrange
            float value = 5f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 0f;
            float outMax = 100f;

            // Act
            float result = MathUtility.RemapClamped(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(50f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that RemapClamped clamps values above input range to output max.
        /// </summary>
        [Test]
        public void RemapClamped_ValueAboveRange_ClampsToOutMax()
        {
            // Arrange
            float value = 15f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 0f;
            float outMax = 100f;

            // Act
            float result = MathUtility.RemapClamped(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(100f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that RemapClamped clamps values below input range to output min.
        /// </summary>
        [Test]
        public void RemapClamped_ValueBelowRange_ClampsToOutMin()
        {
            // Arrange
            float value = -5f;
            float inMin = 0f;
            float inMax = 10f;
            float outMin = 0f;
            float outMax = 100f;

            // Act
            float result = MathUtility.RemapClamped(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(0f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that RemapClamped handles reversed ranges correctly.
        /// </summary>
        [Test]
        public void RemapClamped_ReversedRanges_ClampsCorrectly()
        {
            // Arrange
            float value = -5f;
            float inMin = 10f;
            float inMax = 0f;
            float outMin = 100f;
            float outMax = 0f;

            // Act
            float result = MathUtility.RemapClamped(value, inMin, inMax, outMin, outMax);

            // Assert
            Assert.AreEqual(0f, result, 0.0001f);
        }

        #endregion

        #region Quadratic Bezier Point Tests

        /// <summary>
        /// Verifies that CalculateQuadraticBezierPoint returns start point at t=0.
        /// </summary>
        [Test]
        public void CalculateQuadraticBezierPoint_T0_ReturnsStartPoint()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateQuadraticBezierPoint(p0, p1, p2, 0f);

            // Assert
            Assert.AreEqual(p0, result);
        }

        /// <summary>
        /// Verifies that CalculateQuadraticBezierPoint returns end point at t=1.
        /// </summary>
        [Test]
        public void CalculateQuadraticBezierPoint_T1_ReturnsEndPoint()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateQuadraticBezierPoint(p0, p1, p2, 1f);

            // Assert
            Assert.AreEqual(p2, result);
        }

        /// <summary>
        /// Verifies that CalculateQuadraticBezierPoint returns midpoint at t=0.5.
        /// </summary>
        [Test]
        public void CalculateQuadraticBezierPoint_T05_ReturnsMidpoint()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateQuadraticBezierPoint(p0, p1, p2, 0.5f);

            // Assert
            Assert.AreEqual(5f, result.x, 0.0001f);
            Assert.AreEqual(2.5f, result.y, 0.0001f);
            Assert.AreEqual(0f, result.z, 0.0001f);
        }

        /// <summary>
        /// Verifies that CalculateQuadraticBezierPoint handles 3D coordinates.
        /// </summary>
        [Test]
        public void CalculateQuadraticBezierPoint_3DCoordinates_CalculatesCorrectly()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 3f, 4f);
            Vector3 p2 = new Vector3(10f, 10f, 10f);

            // Act
            Vector3 result = MathUtility.CalculateQuadraticBezierPoint(p0, p1, p2, 0.5f);

            // Assert
            Assert.AreEqual(3.5f, result.x, 0.0001f);
            Assert.AreEqual(4f, result.y, 0.0001f);
            Assert.AreEqual(4.5f, result.z, 0.0001f);
        }

        #endregion

        #region Quadratic Bezier Derivative Tests

        /// <summary>
        /// Verifies that CalculateQuadraticBezierDerivative returns correct tangent at t=0.
        /// </summary>
        [Test]
        public void CalculateQuadraticBezierDerivative_T0_ReturnsCorrectTangent()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateQuadraticBezierDerivative(p0, p1, p2, 0f);

            // Assert
            Vector3 expected = 2f * (p1 - p0);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Verifies that CalculateQuadraticBezierDerivative returns correct tangent at t=1.
        /// </summary>
        [Test]
        public void CalculateQuadraticBezierDerivative_T1_ReturnsCorrectTangent()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateQuadraticBezierDerivative(p0, p1, p2, 1f);

            // Assert
            Vector3 expected = 2f * (p2 - p1);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Verifies that CalculateQuadraticBezierDerivative returns non-zero tangent at midpoint.
        /// </summary>
        [Test]
        public void CalculateQuadraticBezierDerivative_T05_ReturnsNonZeroTangent()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateQuadraticBezierDerivative(p0, p1, p2, 0.5f);

            // Assert
            Assert.AreNotEqual(Vector3.zero, result);
        }

        #endregion

        #region Cubic Bezier Point Tests

        /// <summary>
        /// Verifies that CalculateCubicBezierPoint returns start point at t=0.
        /// </summary>
        [Test]
        public void CalculateCubicBezierPoint_T0_ReturnsStartPoint()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateCubicBezierPoint(p0, p1, p2, p3, 0f);

            // Assert
            Assert.AreEqual(p0, result);
        }

        /// <summary>
        /// Verifies that CalculateCubicBezierPoint returns end point at t=1.
        /// </summary>
        [Test]
        public void CalculateCubicBezierPoint_T1_ReturnsEndPoint()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateCubicBezierPoint(p0, p1, p2, p3, 1f);

            // Assert
            Assert.AreEqual(p3, result);
        }

        /// <summary>
        /// Verifies that CalculateCubicBezierPoint calculates point at t=0.5.
        /// </summary>
        [Test]
        public void CalculateCubicBezierPoint_T05_ReturnsCorrectPoint()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateCubicBezierPoint(p0, p1, p2, p3, 0.5f);

            // Assert
            Assert.AreEqual(5f, result.x, 0.0001f);
            Assert.AreEqual(3.75f, result.y, 0.0001f);
            Assert.AreEqual(0f, result.z, 0.0001f);
        }

        /// <summary>
        /// Verifies that CalculateCubicBezierPoint handles 3D coordinates.
        /// </summary>
        [Test]
        public void CalculateCubicBezierPoint_3DCoordinates_CalculatesCorrectly()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 3f, 1f);
            Vector3 p2 = new Vector3(8f, 3f, 2f);
            Vector3 p3 = new Vector3(10f, 0f, 10f);

            // Act
            Vector3 result = MathUtility.CalculateCubicBezierPoint(p0, p1, p2, p3, 0.5f);

            // Assert
            Assert.AreEqual(5f, result.x, 0.0001f);
            Assert.AreEqual(2.25f, result.y, 0.0001f);
            Assert.AreEqual(2.375f, result.z, 0.0001f);
        }

        #endregion

        #region Cubic Bezier Derivative Tests

        /// <summary>
        /// Verifies that CalculateCubicBezierDerivative returns correct tangent at t=0.
        /// </summary>
        [Test]
        public void CalculateCubicBezierDerivative_T0_ReturnsCorrectTangent()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateCubicBezierDerivative(p0, p1, p2, p3, 0f);

            // Assert
            Vector3 expected = 3f * (p1 - p0);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Verifies that CalculateCubicBezierDerivative returns correct tangent at t=1.
        /// </summary>
        [Test]
        public void CalculateCubicBezierDerivative_T1_ReturnsCorrectTangent()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateCubicBezierDerivative(p0, p1, p2, p3, 1f);

            // Assert
            Vector3 expected = 3f * (p3 - p2);
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Verifies that CalculateCubicBezierDerivative returns non-zero tangent at midpoint.
        /// </summary>
        [Test]
        public void CalculateCubicBezierDerivative_T05_ReturnsNonZeroTangent()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            Vector3 result = MathUtility.CalculateCubicBezierDerivative(p0, p1, p2, p3, 0.5f);

            // Assert
            Assert.AreNotEqual(Vector3.zero, result);
        }

        #endregion

        #region Quadratic Bezier Length Tests

        /// <summary>
        /// Verifies that EstimateQuadraticBezierLength returns zero for collinear points.
        /// </summary>
        [Test]
        public void EstimateQuadraticBezierLength_SamePoints_ReturnsZero()
        {
            // Arrange
            Vector3 p0 = new Vector3(5f, 5f, 5f);
            Vector3 p1 = new Vector3(5f, 5f, 5f);
            Vector3 p2 = new Vector3(5f, 5f, 5f);

            // Act
            float result = MathUtility.EstimateQuadraticBezierLength(p0, p1, p2);

            // Assert
            Assert.AreEqual(0f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that EstimateQuadraticBezierLength returns non-zero for arc.
        /// </summary>
        [Test]
        public void EstimateQuadraticBezierLength_Arc_ReturnsPositiveLength()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            float result = MathUtility.EstimateQuadraticBezierLength(p0, p1, p2);

            // Assert
            Assert.IsTrue(result > 0f);
        }

        /// <summary>
        /// Verifies that EstimateQuadraticBezierLength handles segments parameter correctly.
        /// </summary>
        [Test]
        public void EstimateQuadraticBezierLength_MoreSegments_IncreasesAccuracy()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            float length10 = MathUtility.EstimateQuadraticBezierLength(p0, p1, p2, 10);
            float length100 = MathUtility.EstimateQuadraticBezierLength(p0, p1, p2, 100);

            // Assert
            Assert.IsTrue(length100 > 0f);
        }

        /// <summary>
        /// Verifies that EstimateQuadraticBezierLength handles zero segments.
        /// </summary>
        [Test]
        public void EstimateQuadraticBezierLength_ZeroSegments_ReturnsDistance()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 5f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            float result = MathUtility.EstimateQuadraticBezierLength(p0, p1, p2, 0);

            // Assert
            Assert.IsTrue(result > 0f);
        }

        /// <summary>
        /// Verifies that EstimateQuadraticBezierLength returns straight line distance for linear curve.
        /// </summary>
        [Test]
        public void EstimateQuadraticBezierLength_LinearCurve_ReturnsDirectDistance()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(5f, 0f, 0f);
            Vector3 p2 = new Vector3(10f, 0f, 0f);

            // Act
            float result = MathUtility.EstimateQuadraticBezierLength(p0, p1, p2, 100);

            // Assert
            Assert.AreEqual(10f, result, 0.01f);
        }

        #endregion

        #region Cubic Bezier Length Tests

        /// <summary>
        /// Verifies that EstimateCubicBezierLength returns zero for collinear points.
        /// </summary>
        [Test]
        public void EstimateCubicBezierLength_SamePoints_ReturnsZero()
        {
            // Arrange
            Vector3 p0 = new Vector3(5f, 5f, 5f);
            Vector3 p1 = new Vector3(5f, 5f, 5f);
            Vector3 p2 = new Vector3(5f, 5f, 5f);
            Vector3 p3 = new Vector3(5f, 5f, 5f);

            // Act
            float result = MathUtility.EstimateCubicBezierLength(p0, p1, p2, p3);

            // Assert
            Assert.AreEqual(0f, result, 0.0001f);
        }

        /// <summary>
        /// Verifies that EstimateCubicBezierLength returns non-zero for S-curve.
        /// </summary>
        [Test]
        public void EstimateCubicBezierLength_SCurve_ReturnsPositiveLength()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            float result = MathUtility.EstimateCubicBezierLength(p0, p1, p2, p3);

            // Assert
            Assert.IsTrue(result > 0f);
        }

        /// <summary>
        /// Verifies that EstimateCubicBezierLength handles segments parameter correctly.
        /// </summary>
        [Test]
        public void EstimateCubicBezierLength_MoreSegments_IncreasesAccuracy()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            float length10 = MathUtility.EstimateCubicBezierLength(p0, p1, p2, p3, 10);
            float length100 = MathUtility.EstimateCubicBezierLength(p0, p1, p2, p3, 100);

            // Assert
            Assert.IsTrue(length100 > 0f);
        }

        /// <summary>
        /// Verifies that EstimateCubicBezierLength handles zero segments.
        /// </summary>
        [Test]
        public void EstimateCubicBezierLength_ZeroSegments_ReturnsDistance()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(2f, 5f, 0f);
            Vector3 p2 = new Vector3(8f, 5f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            float result = MathUtility.EstimateCubicBezierLength(p0, p1, p2, p3, 0);

            // Assert
            Assert.IsTrue(result > 0f);
        }

        /// <summary>
        /// Verifies that EstimateCubicBezierLength returns straight line distance for linear curve.
        /// </summary>
        [Test]
        public void EstimateCubicBezierLength_LinearCurve_ReturnsDirectDistance()
        {
            // Arrange
            Vector3 p0 = new Vector3(0f, 0f, 0f);
            Vector3 p1 = new Vector3(3.33f, 0f, 0f);
            Vector3 p2 = new Vector3(6.66f, 0f, 0f);
            Vector3 p3 = new Vector3(10f, 0f, 0f);

            // Act
            float result = MathUtility.EstimateCubicBezierLength(p0, p1, p2, p3, 100);

            // Assert
            Assert.AreEqual(10f, result, 0.01f);
        }

        #endregion
    }
}
