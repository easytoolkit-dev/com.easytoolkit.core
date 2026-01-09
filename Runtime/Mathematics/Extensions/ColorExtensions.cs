using UnityEngine;

namespace EasyToolKit.Core.Mathematics
{
    /// <summary>
    /// Provides extension methods for Unity's Color struct to enable fluent API operations
    /// for color manipulation and modification.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Sets the red component of the color and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="r">The new red value (0-1)</param>
        /// <returns>The modified color with updated red component</returns>
        public static Color WithR(this Color color, float r)
        {
            color.r = r;
            return color;
        }

        /// <summary>
        /// Sets the green component of the color and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="g">The new green value (0-1)</param>
        /// <returns>The modified color with updated green component</returns>
        public static Color WithG(this Color color, float g)
        {
            color.g = g;
            return color;
        }

        /// <summary>
        /// Sets the blue component of the color and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="b">The new blue value (0-1)</param>
        /// <returns>The modified color with updated blue component</returns>
        public static Color WithB(this Color color, float b)
        {
            color.b = b;
            return color;
        }

        /// <summary>
        /// Sets the alpha component of the color and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="a">The new alpha value (0-1)</param>
        /// <returns>The modified color with updated alpha component</returns>
        public static Color WithA(this Color color, float a)
        {
            color.a = a;
            return color;
        }

        #region Multiplied Operations

        /// <summary>
        /// Multiplies the red component by a factor and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="factor">The multiplication factor</param>
        /// <returns>The modified color with multiplied red component</returns>
        public static Color WithRMultiplied(this Color color, float factor)
        {
            color.r *= factor;
            return color;
        }

        /// <summary>
        /// Multiplies the green component by a factor and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="factor">The multiplication factor</param>
        /// <returns>The modified color with multiplied green component</returns>
        public static Color WithGMultiplied(this Color color, float factor)
        {
            color.g *= factor;
            return color;
        }

        /// <summary>
        /// Multiplies the blue component by a factor and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="factor">The multiplication factor</param>
        /// <returns>The modified color with multiplied blue component</returns>
        public static Color WithBMultiplied(this Color color, float factor)
        {
            color.b *= factor;
            return color;
        }

        /// <summary>
        /// Multiplies the alpha component by a factor and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="factor">The multiplication factor</param>
        /// <returns>The modified color with multiplied alpha component</returns>
        public static Color WithAMultiplied(this Color color, float factor)
        {
            color.a *= factor;
            return color;
        }

        /// <summary>
        /// Multiplies all RGB components by a factor and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="factor">The multiplication factor</param>
        /// <returns>The modified color with multiplied RGB components</returns>
        public static Color WithRgbMultiplied(this Color color, float factor)
        {
            color.r *= factor;
            color.g *= factor;
            color.b *= factor;
            return color;
        }

        #endregion

        #region OffsetBy Operations

        /// <summary>
        /// Adds an offset to the red component and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="offset">The offset value to add</param>
        /// <returns>The modified color with offset red component</returns>
        public static Color WithROffsetBy(this Color color, float offset)
        {
            color.r += offset;
            return color;
        }

        /// <summary>
        /// Adds an offset to the green component and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="offset">The offset value to add</param>
        /// <returns>The modified color with offset green component</returns>
        public static Color WithGOffsetBy(this Color color, float offset)
        {
            color.g += offset;
            return color;
        }

        /// <summary>
        /// Adds an offset to the blue component and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="offset">The offset value to add</param>
        /// <returns>The modified color with offset blue component</returns>
        public static Color WithBOffsetBy(this Color color, float offset)
        {
            color.b += offset;
            return color;
        }

        /// <summary>
        /// Adds an offset to the alpha component and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="offset">The offset value to add</param>
        /// <returns>The modified color with offset alpha component</returns>
        public static Color WithAOffsetBy(this Color color, float offset)
        {
            color.a += offset;
            return color;
        }

        /// <summary>
        /// Adds an offset to all RGB components and returns the modified color for fluent chaining.
        /// </summary>
        /// <param name="color">The source color</param>
        /// <param name="offset">The offset value to add to each component</param>
        /// <returns>The modified color with offset RGB components</returns>
        public static Color WithRgbOffsetBy(this Color color, float offset)
        {
            color.r += offset;
            color.g += offset;
            color.b += offset;
            return color;
        }

        #endregion
    }
}
