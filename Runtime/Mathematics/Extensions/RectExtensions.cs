using UnityEngine;

namespace EasyToolKit.Core.Mathematics
{
    /// <summary>
    /// Provides extension methods for Unity's Rect struct to enable fluent API operations
    /// for rectangle manipulation, alignment, padding, and subdivision.
    /// </summary>
    public static class RectExtensions
    {
        #region Size

        /// <summary>
        /// Sets the width of the Rect to the specified value and returns the modified Rect for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the new Rect.</param>
        /// <returns>The modified Rect with updated width.</returns>
        public static Rect WithWidth(this Rect rect, float width)
        {
            rect.width = width;
            return rect;
        }

        /// <summary>
        /// Sets the height of the Rect to the specified value and returns the modified Rect for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="height">The desired height of the new Rect.</param>
        /// <returns>The modified Rect with updated height.</returns>
        public static Rect WithHeight(this Rect rect, float height)
        {
            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Sets the size of the Rect to the specified width and height and returns the modified Rect for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the new Rect.</param>
        /// <param name="height">The desired height of the new Rect.</param>
        /// <returns>The modified Rect with updated size.</returns>
        public static Rect WithSize(this Rect rect, float width, float height)
        {
            rect.width = width;
            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Sets the width and height of the Rect to the same value and returns the modified Rect for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="widthAndHeight">The desired width and height of the new Rect.</param>
        /// <returns>The modified Rect with updated size.</returns>
        public static Rect WithSize(this Rect rect, float widthAndHeight)
        {
            rect.width = widthAndHeight;
            rect.height = widthAndHeight;
            return rect;
        }

        /// <summary>
        /// Sets the size of the Rect to the specified Vector2 and returns the modified Rect for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="size">The desired size of the new Rect.</param>
        /// <returns>The modified Rect with updated size.</returns>
        public static Rect WithSize(this Rect rect, Vector2 size)
        {
            rect.size = size;
            return rect;
        }

        /// <summary>
        /// Clamps the Rect's width between the specified minimum and maximum values and returns the modified Rect for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="minWidth">The minimum width to enforce.</param>
        /// <param name="maxWidth">The maximum width to enforce.</param>
        /// <returns>The modified Rect with width clamped between minimum and maximum values.</returns>
        public static Rect WithWidthClamped(this Rect rect, float minWidth, float maxWidth)
        {
            rect.width = Mathf.Clamp(rect.width, minWidth, maxWidth);
            return rect;
        }

        /// <summary>
        /// Clamps the Rect's height between the specified minimum and maximum values and returns the modified Rect for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="minHeight">The minimum height to enforce.</param>
        /// <param name="maxHeight">The maximum height to enforce.</param>
        /// <returns>The modified Rect with height clamped between minimum and maximum values.</returns>
        public static Rect WithHeightClamped(this Rect rect, float minHeight, float maxHeight)
        {
            rect.height = Mathf.Clamp(rect.height, minHeight, maxHeight);
            return rect;
        }

        #endregion

        #region Padding

        /// <summary>
        /// Applies horizontal padding to the Rect by reducing its width and adjusting its X position.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="padding">The padding amount to apply on both left and right sides.</param>
        /// <returns>The modified Rect with horizontal padding applied.</returns>
        public static Rect WithHorizontalPadding(this Rect rect, float padding)
        {
            rect.x += padding;
            rect.width -= padding * 2f;
            return rect;
        }

        /// <summary>
        /// Applies asymmetric horizontal padding to the Rect by reducing its width and adjusting its X position.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="left">The padding amount to apply on the left side.</param>
        /// <param name="right">The padding amount to apply on the right side.</param>
        /// <returns>The modified Rect with horizontal padding applied.</returns>
        public static Rect WithHorizontalPadding(this Rect rect, float left, float right)
        {
            rect.x += left;
            rect.width -= left + right;
            return rect;
        }

        /// <summary>
        /// Applies vertical padding to the Rect by reducing its height and adjusting its Y position.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="padding">The padding amount to apply on both top and bottom sides.</param>
        /// <returns>The modified Rect with vertical padding applied.</returns>
        public static Rect WithVerticalPadding(this Rect rect, float padding)
        {
            rect.y += padding;
            rect.height -= padding * 2f;
            return rect;
        }

        /// <summary>
        /// Applies asymmetric vertical padding to the Rect by reducing its height and adjusting its Y position.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="top">The padding amount to apply on the top side.</param>
        /// <param name="bottom">The padding amount to apply on the bottom side.</param>
        /// <returns>The modified Rect with vertical padding applied.</returns>
        public static Rect WithVerticalPadding(this Rect rect, float top, float bottom)
        {
            rect.y += top;
            rect.height -= top + bottom;
            return rect;
        }

        /// <summary>
        /// Applies uniform padding to the Rect on all sides.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="padding">The padding amount to apply on all sides.</param>
        /// <returns>The modified Rect with padding applied.</returns>
        public static Rect WithPadding(this Rect rect, float padding)
        {
            rect.position += new Vector2(padding, padding);
            rect.size -= new Vector2(padding, padding) * 2f;
            return rect;
        }

        /// <summary>
        /// Applies asymmetric padding to the Rect with different horizontal and vertical values.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="horizontal">The padding amount to apply on left and right sides.</param>
        /// <param name="vertical">The padding amount to apply on top and bottom sides.</param>
        /// <returns>The modified Rect with padding applied.</returns>
        public static Rect WithPadding(this Rect rect, float horizontal, float vertical)
        {
            rect.position += new Vector2(horizontal, vertical);
            rect.size -= new Vector2(horizontal, vertical) * 2f;
            return rect;
        }

        /// <summary>
        /// Applies fully asymmetric padding to the Rect with different values for each side.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="left">The padding amount to apply on the left side.</param>
        /// <param name="right">The padding amount to apply on the right side.</param>
        /// <param name="top">The padding amount to apply on the top side.</param>
        /// <param name="bottom">The padding amount to apply on the bottom side.</param>
        /// <returns>The modified Rect with padding applied.</returns>
        public static Rect WithPadding(this Rect rect, float left, float right, float top, float bottom)
        {
            rect.position += new Vector2(left, top);
            rect.size -= new Vector2(left + right, top + bottom);
            return rect;
        }

        #endregion

        #region Alignment

        /// <summary>
        /// Aligns a Rect with the specified width to the left edge of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the left edge.</returns>
        public static Rect WithLeftAligned(this Rect rect, float width)
        {
            rect.width = width;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified width to the horizontal center of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the horizontal center.</returns>
        public static Rect WithCenterAligned(this Rect rect, float width)
        {
            rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
            rect.width = width;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified width and height to the center of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the aligned Rect.</param>
        /// <param name="height">The desired height of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the center.</returns>
        public static Rect WithCenterAligned(this Rect rect, float width, float height)
        {
            rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
            rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
            rect.width = width;
            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified width to the right edge of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the right edge.</returns>
        public static Rect WithRightAligned(this Rect rect, float width)
        {
            rect.x = rect.x + rect.width - width;
            rect.width = width;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified width to the right edge of the original Rect and returns it for fluent chaining.
        /// When clamped is true, ensures the aligned Rect does not extend beyond the original left edge.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the aligned Rect.</param>
        /// <param name="clamp">If true, prevents the aligned Rect from extending beyond the original left edge.</param>
        /// <returns>The modified Rect aligned to the right edge.</returns>
        public static Rect WithRightAligned(this Rect rect, float width, bool clamp)
        {
            if (clamp)
            {
                rect.xMin = Mathf.Max(rect.xMax - width, rect.xMin);
                return rect;
            }

            rect.x = rect.x + rect.width - width;
            rect.width = width;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified height to the top edge of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="height">The desired height of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the top edge.</returns>
        public static Rect WithTopAligned(this Rect rect, float height)
        {
            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified height to the vertical middle of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="height">The desired height of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the vertical middle.</returns>
        public static Rect WithMiddleAligned(this Rect rect, float height)
        {
            rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified height to the bottom edge of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="height">The desired height of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the bottom edge.</returns>
        public static Rect WithBottomAligned(this Rect rect, float height)
        {
            rect.y = rect.y + rect.height - height;
            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified width horizontally to the center of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the horizontal center.</returns>
        public static Rect WithCenterXAligned(this Rect rect, float width)
        {
            rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
            rect.width = width;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified height vertically to the center of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="height">The desired height of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the vertical center.</returns>
        public static Rect WithCenterYAligned(this Rect rect, float height)
        {
            rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
            rect.height = height;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified width and height (same value) to the center of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="size">The desired width and height of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the center.</returns>
        public static Rect WithCenterXYAligned(this Rect rect, float size)
        {
            rect.y = rect.y + rect.height * 0.5f - size * 0.5f;
            rect.x = rect.x + rect.width * 0.5f - size * 0.5f;
            rect.height = size;
            rect.width = size;
            return rect;
        }

        /// <summary>
        /// Aligns a Rect with the specified width and height to the center of the original Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="width">The desired width of the aligned Rect.</param>
        /// <param name="height">The desired height of the aligned Rect.</param>
        /// <returns>The modified Rect aligned to the center.</returns>
        public static Rect WithCenterXYAligned(this Rect rect, float width, float height)
        {
            rect.y = rect.y + rect.height * 0.5f - height * 0.5f;
            rect.x = rect.x + rect.width * 0.5f - width * 0.5f;
            rect.width = width;
            rect.height = height;
            return rect;
        }

        #endregion

        #region Center

        /// <summary>
        /// Sets the center X position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="x">The desired center X position.</param>
        /// <returns>The modified Rect with updated center X position.</returns>
        public static Rect WithCenterX(this Rect rect, float x)
        {
            rect.center = new Vector2(x, rect.center.y);
            return rect;
        }

        /// <summary>
        /// Sets the center Y position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="y">The desired center Y position.</param>
        /// <returns>The modified Rect with updated center Y position.</returns>
        public static Rect WithCenterY(this Rect rect, float y)
        {
            rect.center = new Vector2(rect.center.x, y);
            return rect;
        }

        /// <summary>
        /// Sets the center position of the Rect to the specified X and Y coordinates and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="x">The desired center X position.</param>
        /// <param name="y">The desired center Y position.</param>
        /// <returns>The modified Rect with updated center position.</returns>
        public static Rect WithCenter(this Rect rect, float x, float y)
        {
            rect.center = new Vector2(x, y);
            return rect;
        }

        /// <summary>
        /// Sets the center position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="center">The desired center position.</param>
        /// <returns>The modified Rect with updated center position.</returns>
        public static Rect WithCenter(this Rect rect, Vector2 center)
        {
            rect.center = center;
            return rect;
        }

        #endregion

        #region Position

        /// <summary>
        /// Sets the position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="position">The desired position.</param>
        /// <returns>The modified Rect with updated position.</returns>
        public static Rect WithPosition(this Rect rect, Vector2 position)
        {
            rect.position = position;
            return rect;
        }

        /// <summary>
        /// Moves the Rect's position by the specified Vector2 offset and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="move">The change in position.</param>
        /// <returns>The modified Rect with adjusted position.</returns>
        public static Rect WithPositionOffsetBy(this Rect rect, Vector2 move)
        {
            rect.position += move;
            return rect;
        }

        /// <summary>
        /// Moves the Rect's position by the specified X and Y offsets and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="x">The X offset to add.</param>
        /// <param name="y">The Y offset to add.</param>
        /// <returns>The modified Rect with adjusted position.</returns>
        public static Rect WithPositionOffsetBy(this Rect rect, float x, float y)
        {
            rect.x += x;
            rect.y += y;
            return rect;
        }

        #endregion

        #region X/Y Position

        /// <summary>
        /// Sets the X position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="x">The desired X position.</param>
        /// <returns>The modified Rect with updated X position.</returns>
        public static Rect WithX(this Rect rect, float x)
        {
            rect.x = x;
            return rect;
        }

        /// <summary>
        /// Sets the Y position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="y">The desired Y position.</param>
        /// <returns>The modified Rect with updated Y position.</returns>
        public static Rect WithY(this Rect rect, float y)
        {
            rect.y = y;
            return rect;
        }

        /// <summary>
        /// Adjusts the Rect's X position by the specified offset and returns it for fluent chaining.
        /// Use a positive value to move right, or a negative value to move left.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply (positive moves right, negative moves left).</param>
        /// <returns>The modified Rect with adjusted X position.</returns>
        public static Rect WithXOffsetBy(this Rect rect, float offset)
        {
            rect.x += offset;
            return rect;
        }

        /// <summary>
        /// Adjusts the Rect's Y position by the specified offset and returns it for fluent chaining.
        /// Use a positive value to move up, or a negative value to move down.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply (positive moves up, negative moves down).</param>
        /// <returns>The modified Rect with adjusted Y position.</returns>
        public static Rect WithYOffsetBy(this Rect rect, float offset)
        {
            rect.y += offset;
            return rect;
        }

        #endregion

        #region Min/Max Position

        /// <summary>
        /// Sets the min position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="min">The desired min position.</param>
        /// <returns>The modified Rect with updated min position.</returns>
        public static Rect WithMin(this Rect rect, Vector2 min)
        {
            rect.min = min;
            return rect;
        }

        /// <summary>
        /// Sets the max position of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="max">The desired max position.</param>
        /// <returns>The modified Rect with updated max position.</returns>
        public static Rect WithMax(this Rect rect, Vector2 max)
        {
            rect.max = max;
            return rect;
        }

        /// <summary>
        /// Adjusts the Rect's min position by the specified offset and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply.</param>
        /// <returns>The modified Rect with adjusted min position.</returns>
        public static Rect WithMinOffsetBy(this Rect rect, Vector2 offset)
        {
            rect.min += offset;
            return rect;
        }

        /// <summary>
        /// Adjusts the Rect's max position by the specified offset and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply.</param>
        /// <returns>The modified Rect with adjusted max position.</returns>
        public static Rect WithMaxOffsetBy(this Rect rect, Vector2 offset)
        {
            rect.max += offset;
            return rect;
        }

        #endregion

        #region Edge Positions

        /// <summary>
        /// Sets the X min position (left edge) of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="xMin">The desired min X position.</param>
        /// <returns>The modified Rect with updated X min position.</returns>
        public static Rect WithXMin(this Rect rect, float xMin)
        {
            rect.xMin = xMin;
            return rect;
        }

        /// <summary>
        /// Sets the X max position (right edge) of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="xMax">The desired X max position.</param>
        /// <returns>The modified Rect with updated X max position.</returns>
        public static Rect WithXMax(this Rect rect, float xMax)
        {
            rect.xMax = xMax;
            return rect;
        }

        /// <summary>
        /// Sets the Y min position (bottom edge) of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="yMin">The desired Y min position.</param>
        /// <returns>The modified Rect with updated Y min position.</returns>
        public static Rect WithYMin(this Rect rect, float yMin)
        {
            rect.yMin = yMin;
            return rect;
        }

        /// <summary>
        /// Sets the Y max position (top edge) of the Rect and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="yMax">The desired Y max position.</param>
        /// <returns>The modified Rect with updated Y max position.</returns>
        public static Rect WithYMax(this Rect rect, float yMax)
        {
            rect.yMax = yMax;
            return rect;
        }

        /// <summary>
        /// Adjusts the X min position (left edge) of the Rect by the specified offset and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply.</param>
        /// <returns>The modified Rect with adjusted X min position.</returns>
        public static Rect WithXMinOffsetBy(this Rect rect, float offset)
        {
            rect.xMin += offset;
            return rect;
        }

        /// <summary>
        /// Adjusts the X max position (right edge) of the Rect by the specified offset and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply.</param>
        /// <returns>The modified Rect with adjusted X max position.</returns>
        public static Rect WithXMaxOffsetBy(this Rect rect, float offset)
        {
            rect.xMax += offset;
            return rect;
        }

        /// <summary>
        /// Adjusts the Y min position (bottom edge) of the Rect by the specified offset and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply.</param>
        /// <returns>The modified Rect with adjusted Y min position.</returns>
        public static Rect WithYMinOffsetBy(this Rect rect, float offset)
        {
            rect.yMin += offset;
            return rect;
        }

        /// <summary>
        /// Adjusts the Y max position (top edge) of the Rect by the specified offset and returns it for fluent chaining.
        /// </summary>
        /// <param name="rect">The source Rect.</param>
        /// <param name="offset">The offset amount to apply.</param>
        /// <returns>The modified Rect with adjusted Y max position.</returns>
        public static Rect WithYMaxOffsetBy(this Rect rect, float offset)
        {
            rect.yMax += offset;
            return rect;
        }

        #endregion
    }
}
