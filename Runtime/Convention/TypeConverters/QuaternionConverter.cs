using System;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

namespace EasyToolKit.Core.Convention.TypeConverters
{
    /// <summary>
    /// Provides type conversion for Quaternion values to and from string representations.
    /// </summary>
    public class QuaternionConverter : TypeConverter
    {
        /// <summary>
        /// Determines whether this converter can convert an object from the specified source type to Quaternion.
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
        /// Converts the specified value to a Quaternion.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">The CultureInfo to use for the conversion.</param>
        /// <param name="value">The object to convert.</param>
        /// <returns>A Quaternion that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">Thrown when the conversion cannot be performed.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return ParseQuaternion(stringValue);
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
            if (destinationType == typeof(string) && value is Quaternion quaternion)
            {
                return FormatQuaternion(quaternion);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private static Quaternion ParseQuaternion(string value)
        {
            var parts = value.Split(new[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4 &&
                float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y) &&
                float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var z) &&
                float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var w))
            {
                return new Quaternion(x, y, z, w);
            }

            throw new FormatException($"Invalid Quaternion format: {value}. Expected format: (x,y,z,w) or x,y,z,w");
        }

        private static string FormatQuaternion(Quaternion value)
        {
            return $"({value.x.ToString(CultureInfo.InvariantCulture)},{value.y.ToString(CultureInfo.InvariantCulture)},{value.z.ToString(CultureInfo.InvariantCulture)},{value.w.ToString(CultureInfo.InvariantCulture)})";
        }
    }
}
