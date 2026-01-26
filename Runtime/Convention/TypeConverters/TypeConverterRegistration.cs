using System;
using System.ComponentModel;
using UnityEngine;

namespace EasyToolKit.Core.Convention.TypeConverters
{
    /// <summary>
    /// Manages registration of custom TypeConverter instances for Unity types.
    /// </summary>
    public static class TypeConverterRegistration
    {
        private static bool _isRegistered;

        /// <summary>
        /// Registers all custom TypeConverter instances for Unity types.
        /// </summary>
        public static void Register()
        {
            if (_isRegistered)
            {
                return;
            }

            TypeDescriptor.AddAttributes(typeof(Vector2), new TypeConverterAttribute(typeof(Vector2Converter)));
            TypeDescriptor.AddAttributes(typeof(Vector3), new TypeConverterAttribute(typeof(Vector3Converter)));
            TypeDescriptor.AddAttributes(typeof(Vector4), new TypeConverterAttribute(typeof(Vector4Converter)));
            TypeDescriptor.AddAttributes(typeof(Vector2Int), new TypeConverterAttribute(typeof(Vector2IntConverter)));
            TypeDescriptor.AddAttributes(typeof(Vector3Int), new TypeConverterAttribute(typeof(Vector3IntConverter)));
            TypeDescriptor.AddAttributes(typeof(Color), new TypeConverterAttribute(typeof(ColorConverter)));
            TypeDescriptor.AddAttributes(typeof(Rect), new TypeConverterAttribute(typeof(RectConverter)));
            TypeDescriptor.AddAttributes(typeof(Quaternion), new TypeConverterAttribute(typeof(QuaternionConverter)));

            _isRegistered = true;
        }
    }
}
