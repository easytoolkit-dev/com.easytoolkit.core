using System;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

namespace EasyToolKit.Core.Convention.TypeConverters
{
    /// <summary>
    /// Provides type conversion for Rect values to and from string representations.
    /// </summary>
    public class RectConverter : TypeConverter
    {
        /// <summary>
        /// Determines whether this converter can convert an object from the specified source type to Rect.
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
        /// Converts the specified value to a Rect.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">The CultureInfo to use for the conversion.</param>
        /// <param name="value">The object to convert.</param>
        /// <returns>A Rect that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">Thrown when the conversion cannot be performed.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return ParseRect(stringValue);
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
            if (destinationType == typeof(string) && value is Rect rect)
            {
                return FormatRect(rect);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private static Rect ParseRect(string value)
        {
            var parts = value.Split(new[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4 &&
                float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y) &&
                float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var width) &&
                float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
            {
                return new Rect(x, y, width, height);
            }

            throw new FormatException($"Invalid Rect format: {value}. Expected format: (x,y,width,height) or x,y,width,height");
        }

        private static string FormatRect(Rect value)
        {
            return $"({value.x.ToString(CultureInfo.InvariantCulture)},{value.y.ToString(CultureInfo.InvariantCulture)},{value.width.ToString(CultureInfo.InvariantCulture)},{value.height.ToString(CultureInfo.InvariantCulture)})";
        }
    }
}
