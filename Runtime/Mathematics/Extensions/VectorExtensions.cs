using UnityEngine;

namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Extension methods for Unity vector types (Vector2, Vector3, Vector2Int, Vector3Int).
    /// </summary>
    public static class VectorExtensions
    {
        #region Cardinal Direction

        /// <summary>
        /// Converts a direction vector to its cardinal direction (up, down, left, right).
        /// </summary>
        /// <param name="direction">The direction vector to convert.</param>
        /// <param name="zeroThreshold">The minimum magnitude threshold to consider the vector as non-zero.</param>
        /// <returns>A Vector2Int representing the cardinal direction, or zero if the input is below the threshold.</returns>
        /// <remarks>
        /// This method normalizes the input vector and determines which of the four cardinal directions
        /// (up, down, left, right) it most closely aligns with, based on which component has the greater magnitude.
        /// </remarks>
        public static Vector2Int ToCardinalDirection(this Vector2 direction, float zeroThreshold = 0.01f)
        {
            direction = direction.normalized;

            float absoluteX = Mathf.Abs(direction.x);
            float absoluteY = Mathf.Abs(direction.y);

            if (absoluteX <= zeroThreshold && absoluteY <= zeroThreshold)
            {
                return Vector2Int.zero;
            }

            if (direction.x > zeroThreshold)
            {
                if (direction.y > zeroThreshold)
                {
                    return direction.x > direction.y ? Vector2Int.right : Vector2Int.up;
                }
                else
                {
                    return direction.x > absoluteY ? Vector2Int.right : Vector2Int.down;
                }
            }
            else
            {
                if (direction.y > zeroThreshold)
                {
                    return absoluteX > direction.y ? Vector2Int.left : Vector2Int.up;
                }
                else
                {
                    return absoluteX > absoluteY ? Vector2Int.left : Vector2Int.down;
                }
            }
        }

        /// <summary>
        /// Determines whether a direction vector is primarily vertical (up or down).
        /// </summary>
        /// <param name="direction">The direction vector to check.</param>
        /// <returns>True if the cardinal direction is up or down; otherwise, false.</returns>
        public static bool IsCardinalVertical(this Vector2 direction)
        {
            Vector2Int cardinalDirection = direction.ToCardinalDirection();
            return cardinalDirection == Vector2Int.up || cardinalDirection == Vector2Int.down;
        }

        #endregion

        #region Type Conversion

        /// <summary>
        /// Converts a Vector2 to Vector3 by adding a Z component.
        /// </summary>
        /// <param name="vector">The Vector2 to convert.</param>
        /// <param name="z">The value for the Z component.</param>
        /// <returns>A new Vector3 with the converted components.</returns>
        public static Vector3 ToVector3(this Vector2 vector, float z = 0f)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        /// Converts a Vector2 to Vector3 by using the Y component as the Z component.
        /// </summary>
        /// <param name="vector">The Vector2 to convert.</param>
        /// <param name="y">The value for the Y component.</param>
        /// <returns>A new Vector3 with X from vector.x, Y from the parameter, and Z from vector.y.</returns>
        /// <remarks>
        /// This is useful when working with 2D horizontal positions (XZ plane) in a 3D world.
        /// The Vector2's Y component becomes the Vector3's Z component.
        /// </remarks>
        public static Vector3 ToVector3XZ(this Vector2 vector, float y = 0f)
        {
            return new Vector3(vector.x, y, vector.y);
        }

        /// <summary>
        /// Converts a Vector3 to Vector2 by extracting the X and Y components.
        /// </summary>
        /// <param name="vector">The Vector3 to convert.</param>
        /// <returns>A new Vector2 with the X and Y components from the Vector3.</returns>
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Converts a Vector2Int to Vector2.
        /// </summary>
        /// <param name="vector">The Vector2Int to convert.</param>
        /// <returns>A new Vector2 with the converted components.</returns>
        public static Vector2 ToVector2(this Vector2Int vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Converts a Vector2Int to Vector3 by adding a Z component.
        /// </summary>
        /// <param name="vector">The Vector2Int to convert.</param>
        /// <param name="z">The value for the Z component.</param>
        /// <returns>A new Vector3 with the converted components.</returns>
        public static Vector3 ToVector3(this Vector2Int vector, int z = 0)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        /// Converts a Vector2Int to Vector3 by using the Y component as the Z component.
        /// </summary>
        /// <param name="vector">The Vector2Int to convert.</param>
        /// <param name="y">The value for the Y component.</param>
        /// <returns>A new Vector3 with X from vector.x, Y from the parameter, and Z from vector.y.</returns>
        /// <remarks>
        /// This is useful when working with 2D horizontal positions (XZ plane) in a 3D world.
        /// The Vector2Int's Y component becomes the Vector3's Z component.
        /// </remarks>
        public static Vector3 ToVector3XZ(this Vector2Int vector, float y = 0f)
        {
            return new Vector3(vector.x, y, vector.y);
        }

        /// <summary>
        /// Converts a Vector3Int to Vector3.
        /// </summary>
        /// <param name="vector">The Vector3Int to convert.</param>
        /// <returns>A new Vector3 with the converted components.</returns>
        public static Vector3 ToVector3(this Vector3Int vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }

        /// <summary>
        /// Converts a Vector3Int to Vector2 by extracting the X and Y components.
        /// </summary>
        /// <param name="vector">The Vector3Int to convert.</param>
        /// <returns>A new Vector2 with the X and Y components from the Vector3Int.</returns>
        public static Vector2 ToVector2(this Vector3Int vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        /// <summary>
        /// Converts a Vector2Int to Vector3Int by adding a Z component of zero.
        /// </summary>
        /// <param name="vector">The Vector2Int to convert.</param>
        /// <returns>A new Vector3Int with X, Y from the Vector2Int and Z set to zero.</returns>
        public static Vector3Int ToVector3Int(this Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y);
        }

        /// <summary>
        /// Converts a Vector2Int to Vector3Int by using the Y component as the Z component.
        /// </summary>
        /// <param name="vector">The Vector2Int to convert.</param>
        /// <param name="y">The value for the Y component.</param>
        /// <returns>A new Vector3Int with X from vector.x, Y from the parameter, and Z from vector.y.</returns>
        /// <remarks>
        /// This is useful when working with 2D horizontal positions (XZ plane) in a 3D world.
        /// The Vector2Int's Y component becomes the Vector3Int's Z component.
        /// </remarks>
        public static Vector3Int ToVector3IntXZ(this Vector2Int vector, int y = 0)
        {
            return new Vector3Int(vector.x, y, vector.y);
        }

        /// <summary>
        /// Converts a Vector3Int to Vector2Int by extracting the X and Y components.
        /// </summary>
        /// <param name="vector">The Vector3Int to convert.</param>
        /// <returns>A new Vector2Int with the X and Y components from the Vector3Int.</returns>
        public static Vector2Int ToVector2Int(this Vector3Int vector)
        {
            return new Vector2Int(vector.x, vector.y);
        }

        #endregion

        #region Rounding

        /// <summary>
        /// Rounds a Vector2 to the nearest integer values.
        /// </summary>
        /// <param name="vector">The Vector2 to round.</param>
        /// <returns>A Vector2Int with rounded components.</returns>
        public static Vector2Int RoundToVector2Int(this Vector2 vector)
        {
            return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        }

        /// <summary>
        /// Rounds a Vector2 to the nearest integer values and returns as Vector3Int.
        /// </summary>
        /// <param name="vector">The Vector2 to round.</param>
        /// <returns>A Vector3Int with rounded X and Y components, and Z set to zero.</returns>
        public static Vector3Int RoundToVector3Int(this Vector2 vector)
        {
            return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        }

        /// <summary>
        /// Rounds a Vector3 to the nearest integer values.
        /// </summary>
        /// <param name="vector">The Vector3 to round.</param>
        /// <returns>A Vector3Int with rounded components.</returns>
        public static Vector3Int RoundToVector3Int(this Vector3 vector)
        {
            return new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
        }

        /// <summary>
        /// Rounds a Vector3 to the nearest integer values and returns as Vector2Int.
        /// </summary>
        /// <param name="vector">The Vector3 to round.</param>
        /// <returns>A Vector2Int with the rounded X and Y components.</returns>
        public static Vector2Int RoundToVector2Int(this Vector3 vector)
        {
            return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
        }

        /// <summary>
        /// Floors a Vector2 to integer values.
        /// </summary>
        /// <param name="vector">The Vector2 to floor.</param>
        /// <returns>A Vector2Int with floored components.</returns>
        public static Vector2Int FloorToVector2Int(this Vector2 vector)
        {
            return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        }

        /// <summary>
        /// Floors a Vector2 to integer values and returns as Vector3Int.
        /// </summary>
        /// <param name="vector">The Vector2 to floor.</param>
        /// <returns>A Vector3Int with floored X and Y components, and Z set to zero.</returns>
        public static Vector3Int FloorToVector3Int(this Vector2 vector)
        {
            return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        }

        /// <summary>
        /// Floors a Vector3 to integer values.
        /// </summary>
        /// <param name="vector">The Vector3 to floor.</param>
        /// <returns>A Vector3Int with floored components.</returns>
        public static Vector3Int FloorToVector3Int(this Vector3 vector)
        {
            return new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), Mathf.FloorToInt(vector.z));
        }

        /// <summary>
        /// Floors a Vector3 to integer values and returns as Vector2Int.
        /// </summary>
        /// <param name="vector">The Vector3 to floor.</param>
        /// <returns>A Vector2Int with the floored X and Y components.</returns>
        public static Vector2Int FloorToVector2Int(this Vector3 vector)
        {
            return new Vector2Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y));
        }

        #endregion

        #region Fluent API

        /// <summary>
        /// Returns a new Vector2 rotated by the specified angle in degrees.
        /// </summary>
        /// <param name="vector">The Vector2 to rotate.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <returns>A new Vector2 rotated by the specified angle.</returns>
        /// <remarks>
        /// This method performs a standard 2D rotation using the rotation matrix:
        /// [ cos(θ) -sin(θ) ]
        /// [ sin(θ)  cos(θ) ]
        /// </remarks>
        public static Vector2 WithRotated(this Vector2 vector, float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            float newX = vector.x * cos - vector.y * sin;
            float newY = vector.x * sin + vector.y * cos;

            return new Vector2(newX, newY);
        }

        /// <summary>
        /// Returns a new Vector2 with each component rounded to the nearest integer.
        /// </summary>
        /// <param name="vector">The Vector2 to round.</param>
        /// <returns>A new Vector2 with rounded components.</returns>
        public static Vector2 WithRounded(this Vector2 vector)
        {
            return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
        }

        /// <summary>
        /// Returns a new Vector3 with each component rounded to the nearest integer.
        /// </summary>
        /// <param name="vector">The Vector3 to round.</param>
        /// <returns>A new Vector3 with rounded components.</returns>
        public static Vector3 WithRounded(this Vector3 vector)
        {
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }

        /// <summary>
        /// Returns a new Vector2 with the specified X component.
        /// </summary>
        /// <param name="vector">The Vector2 to modify.</param>
        /// <param name="x">The new X value.</param>
        /// <returns>A new Vector2 with the specified X component.</returns>
        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        /// <summary>
        /// Returns a new Vector2 with the specified Y component.
        /// </summary>
        /// <param name="vector">The Vector2 to modify.</param>
        /// <param name="y">The new Y value.</param>
        /// <returns>A new Vector2 with the specified Y component.</returns>
        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        /// <summary>
        /// Returns a new Vector3 with the specified X component.
        /// </summary>
        /// <param name="vector">The Vector3 to modify.</param>
        /// <param name="x">The new X value.</param>
        /// <returns>A new Vector3 with the specified X component.</returns>
        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        /// <summary>
        /// Returns a new Vector3 with the specified Y component.
        /// </summary>
        /// <param name="vector">The Vector3 to modify.</param>
        /// <param name="y">The new Y value.</param>
        /// <returns>A new Vector3 with the specified Y component.</returns>
        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        /// <summary>
        /// Returns a new Vector3 with the specified Z component.
        /// </summary>
        /// <param name="vector">The Vector3 to modify.</param>
        /// <param name="z">The new Z value.</param>
        /// <returns>A new Vector3 with the specified Z component.</returns>
        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        #endregion

        #region Multiplied Operations

        /// <summary>
        /// Multiplies the X component by a factor and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="factor">The multiplication factor.</param>
        /// <returns>The modified vector with multiplied X component.</returns>
        public static Vector2 WithXMultiplied(this Vector2 vector, float factor)
        {
            vector.x *= factor;
            return vector;
        }

        /// <summary>
        /// Multiplies the Y component by a factor and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="factor">The multiplication factor.</param>
        /// <returns>The modified vector with multiplied Y component.</returns>
        public static Vector2 WithYMultiplied(this Vector2 vector, float factor)
        {
            vector.y *= factor;
            return vector;
        }

        /// <summary>
        /// Multiplies all components by a factor and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="factor">The multiplication factor.</param>
        /// <returns>The modified vector with multiplied components.</returns>
        public static Vector2 WithMultiplied(this Vector2 vector, float factor)
        {
            vector.x *= factor;
            vector.y *= factor;
            return vector;
        }

        /// <summary>
        /// Multiplies the X component by a factor and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="factor">The multiplication factor.</param>
        /// <returns>The modified vector with multiplied X component.</returns>
        public static Vector3 WithXMultiplied(this Vector3 vector, float factor)
        {
            vector.x *= factor;
            return vector;
        }

        /// <summary>
        /// Multiplies the Y component by a factor and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="factor">The multiplication factor.</param>
        /// <returns>The modified vector with multiplied Y component.</returns>
        public static Vector3 WithYMultiplied(this Vector3 vector, float factor)
        {
            vector.y *= factor;
            return vector;
        }

        /// <summary>
        /// Multiplies the Z component by a factor and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="factor">The multiplication factor.</param>
        /// <returns>The modified vector with multiplied Z component.</returns>
        public static Vector3 WithZMultiplied(this Vector3 vector, float factor)
        {
            vector.z *= factor;
            return vector;
        }

        /// <summary>
        /// Multiplies all components by a factor and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="factor">The multiplication factor.</param>
        /// <returns>The modified vector with multiplied components.</returns>
        public static Vector3 WithMultiplied(this Vector3 vector, float factor)
        {
            vector.x *= factor;
            vector.y *= factor;
            vector.z *= factor;
            return vector;
        }

        #endregion

        #region OffsetBy Operations

        /// <summary>
        /// Adds an offset to the X component and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="offset">The offset value to add.</param>
        /// <returns>The modified vector with offset X component.</returns>
        public static Vector2 WithXOffsetBy(this Vector2 vector, float offset)
        {
            vector.x += offset;
            return vector;
        }

        /// <summary>
        /// Adds an offset to the Y component and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="offset">The offset value to add.</param>
        /// <returns>The modified vector with offset Y component.</returns>
        public static Vector2 WithYOffsetBy(this Vector2 vector, float offset)
        {
            vector.y += offset;
            return vector;
        }

        /// <summary>
        /// Adds an offset to all components and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="offset">The offset value to add to each component.</param>
        /// <returns>The modified vector with offset components.</returns>
        public static Vector2 WithOffsetBy(this Vector2 vector, float offset)
        {
            vector.x += offset;
            vector.y += offset;
            return vector;
        }

        /// <summary>
        /// Adds an offset to the X component and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="offset">The offset value to add.</param>
        /// <returns>The modified vector with offset X component.</returns>
        public static Vector3 WithXOffsetBy(this Vector3 vector, float offset)
        {
            vector.x += offset;
            return vector;
        }

        /// <summary>
        /// Adds an offset to the Y component and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="offset">The offset value to add.</param>
        /// <returns>The modified vector with offset Y component.</returns>
        public static Vector3 WithYOffsetBy(this Vector3 vector, float offset)
        {
            vector.y += offset;
            return vector;
        }

        /// <summary>
        /// Adds an offset to the Z component and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="offset">The offset value to add.</param>
        /// <returns>The modified vector with offset Z component.</returns>
        public static Vector3 WithZOffsetBy(this Vector3 vector, float offset)
        {
            vector.z += offset;
            return vector;
        }

        /// <summary>
        /// Adds an offset to all components and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="offset">The offset value to add to each component.</param>
        /// <returns>The modified vector with offset components.</returns>
        public static Vector3 WithOffsetBy(this Vector3 vector, float offset)
        {
            vector.x += offset;
            vector.y += offset;
            vector.z += offset;
            return vector;
        }

        #endregion

        #region Clamped Operations

        /// <summary>
        /// Clamps the X component between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="min">The minimum value to enforce.</param>
        /// <param name="max">The maximum value to enforce.</param>
        /// <returns>The modified vector with X component clamped between minimum and maximum values.</returns>
        public static Vector2 WithXClamped(this Vector2 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps the Y component between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="min">The minimum value to enforce.</param>
        /// <param name="max">The maximum value to enforce.</param>
        /// <returns>The modified vector with Y component clamped between minimum and maximum values.</returns>
        public static Vector2 WithYClamped(this Vector2 vector, float min, float max)
        {
            vector.y = Mathf.Clamp(vector.y, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps all components between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="min">The minimum value to enforce.</param>
        /// <param name="max">The maximum value to enforce.</param>
        /// <returns>The modified vector with components clamped between minimum and maximum values.</returns>
        public static Vector2 WithClamped(this Vector2 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            vector.y = Mathf.Clamp(vector.y, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps the X component between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="min">The minimum value to enforce.</param>
        /// <param name="max">The maximum value to enforce.</param>
        /// <returns>The modified vector with X component clamped between minimum and maximum values.</returns>
        public static Vector3 WithXClamped(this Vector3 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps the Y component between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="min">The minimum value to enforce.</param>
        /// <param name="max">The maximum value to enforce.</param>
        /// <returns>The modified vector with Y component clamped between minimum and maximum values.</returns>
        public static Vector3 WithYClamped(this Vector3 vector, float min, float max)
        {
            vector.y = Mathf.Clamp(vector.y, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps the Z component between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="min">The minimum value to enforce.</param>
        /// <param name="max">The maximum value to enforce.</param>
        /// <returns>The modified vector with Z component clamped between minimum and maximum values.</returns>
        public static Vector3 WithZClamped(this Vector3 vector, float min, float max)
        {
            vector.z = Mathf.Clamp(vector.z, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps all components between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="min">The minimum value to enforce.</param>
        /// <param name="max">The maximum value to enforce.</param>
        /// <returns>The modified vector with components clamped between minimum and maximum values.</returns>
        public static Vector3 WithClamped(this Vector3 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            vector.y = Mathf.Clamp(vector.y, min, max);
            vector.z = Mathf.Clamp(vector.z, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps the magnitude of the vector between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="minMagnitude">The minimum magnitude to enforce.</param>
        /// <param name="maxMagnitude">The maximum magnitude to enforce.</param>
        /// <returns>The modified vector with magnitude clamped between minimum and maximum values.</returns>
        /// <remarks>
        /// If the vector's magnitude is below minMagnitude, it is scaled to have exactly minMagnitude.
        /// If the magnitude is above maxMagnitude, it is scaled to have exactly maxMagnitude.
        /// If the vector is zero, it is returned unchanged.
        /// </remarks>
        public static Vector2 WithMagnitudeClamped(this Vector2 vector, float minMagnitude, float maxMagnitude)
        {
            float magnitude = vector.magnitude;
            if (magnitude > 0f)
            {
                float newMagnitude = Mathf.Clamp(magnitude, minMagnitude, maxMagnitude);
                vector = vector.normalized * newMagnitude;
            }
            return vector;
        }

        /// <summary>
        /// Clamps the magnitude of the vector between the specified minimum and maximum values and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="minMagnitude">The minimum magnitude to enforce.</param>
        /// <param name="maxMagnitude">The maximum magnitude to enforce.</param>
        /// <returns>The modified vector with magnitude clamped between minimum and maximum values.</returns>
        /// <remarks>
        /// If the vector's magnitude is below minMagnitude, it is scaled to have exactly minMagnitude.
        /// If the magnitude is above maxMagnitude, it is scaled to have exactly maxMagnitude.
        /// If the vector is zero, it is returned unchanged.
        /// </remarks>
        public static Vector3 WithMagnitudeClamped(this Vector3 vector, float minMagnitude, float maxMagnitude)
        {
            float magnitude = vector.magnitude;
            if (magnitude > 0f)
            {
                float newMagnitude = Mathf.Clamp(magnitude, minMagnitude, maxMagnitude);
                vector = vector.normalized * newMagnitude;
            }
            return vector;
        }

        #endregion

        #region Scale Operations

        /// <summary>
        /// Scales the vector by another vector component-wise and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="scale">The scale vector to multiply by component-wise.</param>
        /// <returns>The modified vector with scaled components.</returns>
        public static Vector2 WithScaled(this Vector2 vector, Vector2 scale)
        {
            vector.x *= scale.x;
            vector.y *= scale.y;
            return vector;
        }

        /// <summary>
        /// Scales the vector by another vector component-wise and returns the modified vector for fluent chaining.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="scale">The scale vector to multiply by component-wise.</param>
        /// <returns>The modified vector with scaled components.</returns>
        public static Vector3 WithScaled(this Vector3 vector, Vector3 scale)
        {
            vector.x *= scale.x;
            vector.y *= scale.y;
            vector.z *= scale.z;
            return vector;
        }

        #endregion

        #region Swap Operations

        /// <summary>
        /// Returns a new Vector2 with the X and Y components swapped.
        /// </summary>
        /// <param name="vector">The Vector2 to swap.</param>
        /// <returns>A new Vector2 with X and Y components exchanged.</returns>
        public static Vector2 WithXYSwapped(this Vector2 vector)
        {
            return new Vector2(vector.y, vector.x);
        }

        /// <summary>
        /// Returns a new Vector3 with the X and Y components swapped.
        /// </summary>
        /// <param name="vector">The Vector3 to swap.</param>
        /// <returns>A new Vector3 with X and Y components exchanged.</returns>
        public static Vector3 WithXYSwapped(this Vector3 vector)
        {
            return new Vector3(vector.y, vector.x, vector.z);
        }

        /// <summary>
        /// Returns a new Vector2Int with the X and Y components swapped.
        /// </summary>
        /// <param name="vector">The Vector2Int to swap.</param>
        /// <returns>A new Vector2Int with X and Y components exchanged.</returns>
        public static Vector2Int WithXYSwapped(this Vector2Int vector)
        {
            return new Vector2Int(vector.y, vector.x);
        }

        /// <summary>
        /// Returns a new Vector3Int with the X and Y components swapped.
        /// </summary>
        /// <param name="vector">The Vector3Int to swap.</param>
        /// <returns>A new Vector3Int with X and Y components exchanged.</returns>
        public static Vector3Int WithXYSwapped(this Vector3Int vector)
        {
            return new Vector3Int(vector.y, vector.x, vector.z);
        }

        #endregion
    }
}
