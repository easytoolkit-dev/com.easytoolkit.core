using System;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

namespace EasyToolKit.Core.Convention.TypeConverters
{
    /// <summary>
    /// Provides type conversion for Vector3 values to and from string representations.
    /// </summary>
    public class Vector3Converter : TypeConverter
    {
        /// <summary>
        /// Determines whether this converter can convert an object from the specified source type to Vector3.
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
        /// Converts the specified value to a Vector3.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">The CultureInfo to use for the conversion.</param>
        /// <param name="value">The object to convert.</param>
        /// <returns>A Vector3 that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">Thrown when the conversion cannot be performed.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return ParseVector3(stringValue);
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
            if (destinationType == typeof(string) && value is Vector3 vector3)
            {
                return FormatVector3(vector3);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private static Vector3 ParseVector3(string value)
        {
            var parts = value.Split(new[] { ',', '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 &&
                float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y) &&
                float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var z))
            {
                return new Vector3(x, y, z);
            }

            throw new FormatException($"Invalid Vector3 format: {value}. Expected format: (x,y,z) or x,y,z");
        }

        private static string FormatVector3(Vector3 value)
        {
            return $"({value.x.ToString(CultureInfo.InvariantCulture)},{value.y.ToString(CultureInfo.InvariantCulture)},{value.z.ToString(CultureInfo.InvariantCulture)})";
        }
    }
}
