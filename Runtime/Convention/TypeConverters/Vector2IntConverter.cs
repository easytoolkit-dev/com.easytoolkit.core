using System;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

namespace EasyToolKit.Core.Convention.TypeConverters
{
    /// <summary>
    /// Provides type conversion for Vector2Int values to and from string representations.
    /// </summary>
    public class Vector2IntConverter : TypeConverter
    {
        /// <summary>
        /// Determines whether this converter can convert an object from the specified source type to Vector2Int.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="sourceType">The type you want to convert from.</param>
        /// <returns>True if this converter can perform the conversion; otherwise, false.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Determines whether this converter can convert an object to the specified destination type.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="destinationType">The type you want to convert to.</param>
        /// <returns>True if this converter can perform the conversion; otherwise, false.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the specified value to a Vector2Int.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">The CultureInfo to use for the conversion.</param>
        /// <param name="value">The object to convert.</param>
        /// <returns>A Vector2Int that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">Thrown when the conversion cannot be performed.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return ParseVector2Int(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the specified value to a string representation.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">The CultureInfo to use for the conversion.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="destinationType">The type to convert to.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotSupportedException">Thrown when the conversion cannot be performed.</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Vector2Int vector2Int)
            {
                return FormatVector2Int(vector2Int);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private static Vector2Int ParseVector2Int(string value)
        {
            var parts = value.Split(new[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 &&
                int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var x) &&
                int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var y))
            {
                return new Vector2Int(x, y);
            }

            throw new FormatException($"Invalid Vector2Int format: {value}. Expected format: (x,y) or x,y");
        }

        private static string FormatVector2Int(Vector2Int value)
        {
            return $"({value.x.ToString(CultureInfo.InvariantCulture)},{value.y.ToString(CultureInfo.InvariantCulture)})";
        }
    }
}
