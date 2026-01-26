using System;
using System.ComponentModel;

namespace EasyToolKit.Core.Convention
{
    /// <summary>
    /// Provides type conversion utilities for converting between strings and various types.
    /// </summary>
    public static class ConvertUtility
    {
        static ConvertUtility()
        {
            TypeConverters.TypeConverterRegistration.Register();
        }

        /// <summary>
        /// Determines whether the specified type can be converted from a string.
        /// </summary>
        /// <param name="targetType">The type to check.</param>
        /// <returns>True if the type can be converted from a string; otherwise, false.</returns>
        public static bool CanConvertFromString(Type targetType)
        {
            if (targetType == null)
            {
                return false;
            }

            // String type can always be converted from string
            if (targetType == typeof(string))
            {
                return true;
            }

            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                return CanConvertFromString(underlyingType);
            }

            // Enum types can be converted from string
            if (targetType.IsEnum)
            {
                return true;
            }

            // Check if TypeConverter supports conversion from string
            var converter = TypeDescriptor.GetConverter(targetType);
            return converter.CanConvertFrom(typeof(string));
        }

        /// <summary>
        /// Determines whether the specified type can be converted to a string.
        /// </summary>
        /// <param name="valueType">The type to check.</param>
        /// <returns>True if the type can be converted to a string; otherwise, false.</returns>
        public static bool CanConvertToString(Type valueType)
        {
            if (valueType == null)
            {
                return false;
            }

            // String type can always be converted to string
            if (valueType == typeof(string))
            {
                return true;
            }

            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(valueType);
            if (underlyingType != null)
            {
                return CanConvertToString(underlyingType);
            }

            // Enum types can be converted to string
            if (valueType.IsEnum)
            {
                return true;
            }

            // Check if TypeConverter supports conversion to string
            var converter = TypeDescriptor.GetConverter(valueType);
            return converter.CanConvertTo(typeof(string));
        }

        /// <summary>
        /// Tries to convert a string to the specified type.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="targetType">The target type to convert to.</param>
        /// <param name="result">The converted result if successful.</param>
        /// <returns>True if conversion succeeded; otherwise, false.</returns>
        public static bool TryConvertFromString(string value, Type targetType, out object result)
        {
            if (targetType == null)
            {
                result = null;
                return false;
            }

            // Handle string type
            if (targetType == typeof(string))
            {
                result = value;
                return true;
            }

            // Handle null or empty string
            if (string.IsNullOrEmpty(value))
            {
                // Handle nullable types
                if (Nullable.GetUnderlyingType(targetType) != null)
                {
                    result = null;
                    return true;
                }

                result = null;
                return false;
            }

            // Handle enum types
            if (targetType.IsEnum)
            {
                try
                {
                    result = Enum.Parse(targetType, value, ignoreCase: true);
                    return true;
                }
                catch
                {
                    result = null;
                    return false;
                }
            }

            // Handle nullable enums
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null && underlyingType.IsEnum)
            {
                try
                {
                    result = Enum.Parse(underlyingType, value, ignoreCase: true);
                    return true;
                }
                catch
                {
                    result = null;
                    return false;
                }
            }

            // Handle primitive types and types with TypeConverter (including Unity types)
            var converter = TypeDescriptor.GetConverter(targetType);
            if (converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    result = converter.ConvertFromInvariantString(value);
                    return true;
                }
                catch
                {
                    result = null;
                    return false;
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Tries to convert a string to the specified type.
        /// </summary>
        /// <typeparam name="T">The target type to convert to.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <param name="result">The converted result if successful.</param>
        /// <returns>True if conversion succeeded; otherwise, false.</returns>
        public static bool TryConvertFromString<T>(string value, out T result)
        {
            if (TryConvertFromString(value, typeof(T), out var objectResult))
            {
                result = (T)objectResult;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Tries to convert a value to its string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="valueType">The type of the value.</param>
        /// <param name="result">The string representation of the value.</param>
        /// <returns>True if conversion succeeded; otherwise, false.</returns>
        public static bool TryConvertToString(object value, Type valueType, out string result)
        {
            if (valueType == null)
            {
                result = string.Empty;
                return false;
            }

            if (value == null)
            {
                result = string.Empty;
                return true;
            }

            // Handle string type
            if (valueType == typeof(string))
            {
                result = (string)value;
                return true;
            }

            // Handle enum types
            if (valueType.IsEnum)
            {
                result = value.ToString();
                return true;
            }

            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(valueType);
            if (underlyingType != null)
            {
                // Handle nullable enums
                if (underlyingType.IsEnum)
                {
                    result = value.ToString();
                    return true;
                }

                // For other nullable types, check if value equals default
                var defaultValue = Activator.CreateInstance(valueType);
                if (Equals(value, defaultValue))
                {
                    result = string.Empty;
                    return true;
                }
            }

            // Handle primitive types and Unity types using TypeConverter
            var converter = TypeDescriptor.GetConverter(valueType);
            if (converter.CanConvertTo(typeof(string)))
            {
                try
                {
                    result = converter.ConvertToInvariantString(value);
                    return true;
                }
                catch
                {
                    result = string.Empty;
                    return false;
                }
            }

            // Fallback to ToString
            result = value.ToString();
            return true;
        }

        /// <summary>
        /// Tries to convert a value to its string representation.
        /// </summary>
        /// <typeparam name="T">The type of the value to convert.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">The string representation of the value.</param>
        /// <returns>True if conversion succeeded; otherwise, false.</returns>
        public static bool TryConvertToString<T>(T value, out string result)
        {
            return TryConvertToString(value, typeof(T), out result);
        }

        /// <summary>
        /// Converts a string to the specified type, throwing an exception if conversion fails.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="targetType">The target type to convert to.</param>
        /// <returns>The converted object of the specified type.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the conversion fails.</exception>
        public static object ConvertFromString(string value, Type targetType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType), "Target type cannot be null. Provide a valid type to convert the string value to.");
            }

            if (TryConvertFromString(value, targetType, out var result))
            {
                return result;
            }

            throw new InvalidOperationException(
                $"Failed to convert string '{value}' to type {targetType.FullName}. " +
                $"Ensure the value is in the correct format and the target type supports string conversion.");
        }

        /// <summary>
        /// Converts a string to the specified type, throwing an exception if conversion fails.
        /// </summary>
        /// <typeparam name="T">The target type to convert to.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <returns>The converted value of the specified type.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the conversion fails.</exception>
        public static T ConvertFromString<T>(string value)
        {
            return (T)ConvertFromString(value, typeof(T));
        }

        /// <summary>
        /// Converts a value to its string representation, throwing an exception if conversion fails.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="valueType">The type of the value.</param>
        /// <returns>The string representation of the value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the conversion fails.</exception>
        public static string ConvertToString(object value, Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType), "Value type cannot be null. Provide a valid type to convert the value from.");
            }

            if (TryConvertToString(value, valueType, out var result))
            {
                return result;
            }

            throw new InvalidOperationException(
                $"Failed to convert value of type {valueType.FullName} to string. " +
                $"Ensure the type supports string conversion.");
        }

        /// <summary>
        /// Converts a value to its string representation, throwing an exception if conversion fails.
        /// </summary>
        /// <typeparam name="T">The type of the value to convert.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The string representation of the value.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the conversion fails.</exception>
        public static string ConvertToString<T>(T value)
        {
            return ConvertToString(value, typeof(T));
        }
    }
}
