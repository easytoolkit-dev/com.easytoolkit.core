using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace EasyToolkit.Core.Reflection
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type is a struct type (value type excluding primitives and enums).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a struct; otherwise, false.</returns>
        public static bool IsStructType(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        /// <summary>
        /// Determines whether the specified type is a Unity built-in type (e.g., Vector2, Vector3, Color, etc.).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a Unity built-in type; otherwise, false.</returns>
        public static bool IsUnityBuiltInType(this Type type)
        {
            return type == typeof(Vector2) || type == typeof(Vector2Int) || type == typeof(Vector3) ||
                   type == typeof(Vector3Int) ||
                   type == typeof(Vector4) || type == typeof(Quaternion) || type == typeof(Color) ||
                   type == typeof(Color32) ||
                   type == typeof(Rect) || type == typeof(RectInt) || type == typeof(Bounds) ||
                   type == typeof(BoundsInt);
        }

        /// <summary>
        /// Determines whether the specified type is an integer type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is an integer type; otherwise, false.</returns>
        public static bool IsIntegerType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified type is a floating-point type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a floating-point type; otherwise, false.</returns>
        public static bool IsFloatingPointType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified type is a boolean type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a boolean; otherwise, false.</returns>
        public static bool IsBooleanType(this Type type)
        {
            return type == typeof(bool);
        }

        /// <summary>
        /// Determines whether the specified type is a string type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a string; otherwise, false.</returns>
        public static bool IsStringType(this Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// Determines whether the specified type is a basic value type (enum, string, boolean, integer, or floating-point).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a basic value type; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        public static bool IsBasicValueType([NotNull] this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsEnum || type.IsStringType() || type.IsBooleanType() || type.IsFloatingPointType() ||
                   type.IsIntegerType();
        }

        /// <summary>
        /// Determines whether the specified type is derived from UnityEngine.Object.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a Unity object type; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when type is null.</exception>
        public static bool IsUnityObjectType([NotNull] this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsDerivedFrom<UnityEngine.Object>();
        }

        /// <summary>
        /// Determines whether the specified type is a nullable reference type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is nullable (not a primitive, value type, or enum); otherwise, false.</returns>
        public static bool IsNullableType(this Type type)
        {
            return !(type.IsPrimitive || type.IsValueType || type.IsEnum);
        }

        /// <summary>
        /// Determines whether the specified type is a generic array (array with generic parameter element type).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is a generic array; otherwise, false.</returns>
        public static bool IsGenericArray(this Type type)
        {
            if (!type.IsArray)
                return false;

            var elementType = type.GetElementType();
            return elementType != null && elementType.IsGenericParameter;
        }

        /// <summary>
        /// Determines whether the specified type is an anonymous type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is an anonymous type; otherwise, false.</returns>
        public static bool IsAnonymousType(this Type type)
        {
            if (type == null)
                return false;

            // Anonymous types have names starting with '<' (compiler-generated format)
            // and are marked with CompilerGeneratedAttribute
            return type.Name.StartsWith("<", StringComparison.Ordinal) &&
                   type.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                       inherit: false).Length > 0;
        }

        /// <summary>
        /// Determines whether the specified type is derived from type T.
        /// </summary>
        /// <typeparam name="T">The base type to check.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is derived from T; otherwise, false.</returns>
        public static bool IsDerivedFrom<T>(this Type type)
        {
            return type.IsDerivedFrom(typeof(T));
        }

        /// <summary>
        /// Determines whether the specified type is derived from the given base type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="baseType">The base type to check against.</param>
        /// <returns>true if the type is derived from baseType; otherwise, false.</returns>
        public static bool IsDerivedFrom(this Type type, Type baseType)
        {
            if (baseType.IsAssignableFrom(type))
            {
                return true;
            }

            if (type.IsInterface && baseType.IsInterface == false)
            {
                return false;
            }

            if (baseType.IsInterface)
            {
                return type.GetInterfaces().Contains(baseType);
            }

            var currentType = type;
            while (currentType != null)
            {
                if (currentType == baseType)
                {
                    return true;
                }

                if (baseType.IsGenericTypeDefinition && currentType.IsGenericType &&
                    currentType.GetGenericTypeDefinition() == baseType)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified type can be instantiated.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="allowLenient">
        /// If true, uses lenient mode where any constructor (not just parameterless) is considered instantiable.
        /// If false, requires a parameterless constructor for reference types.
        /// </param>
        /// <returns>true if the type can be instantiated; otherwise, false.</returns>
        public static bool IsInstantiable(this Type type, bool allowLenient = false)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsUnityObjectType()) return false;
            if (type.IsInterface) return false;
            if (type.IsAbstract) return false;
            if (type.IsArray) return false;
            if (type.ContainsGenericParameters) return false;

            if (type.IsPointer || type.IsByRef || type.IsGenericParameter) return false;
            if (typeof(Delegate).IsAssignableFrom(type)) return false;

            if (type.IsValueType) return true;

            if (allowLenient)
            {
                var ctors = type.GetConstructors(MemberAccessFlags.AllInstance);
                return ctors.Length > 0;
            }
            else
            {
                var ctor = type.GetConstructor(
                    MemberAccessFlags.AllInstance,
                    binder: null,
                    types: Type.EmptyTypes,
                    modifiers: null);
                return ctor != null;
            }
        }
    }
}
