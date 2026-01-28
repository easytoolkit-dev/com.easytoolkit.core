using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Provides extension methods for System.Type reflection operations.
    /// </summary>
    public static partial class TypeExtensions
    {
        private static readonly Dictionary<Type, string> TypeAliasesByType = new Dictionary<Type, string>
        {
            { typeof(void), "void" },
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(object), "object" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(string), "string" }
        };

        /// <summary>
        /// Gets the friendly name alias for a type.
        /// </summary>
        /// <param name="type">The type to get the alias for.</param>
        /// <returns>The friendly type name (e.g., "int", "string", "List&lt;T&gt;").</returns>
        public static string GetAliases(this Type type)
        {
            if (TypeAliasesByType.TryGetValue(type, out string alias))
            {
                return alias;
            }

            return type.IsGenericType ? GetGenericTypeName(type) : type.Name;
        }

        private static string GetGenericTypeName(this Type type)
        {
            var genericArguments = type.GetGenericArguments();
            var typeName = type.Name;
            var genericPartIndex = typeName.IndexOf('`');
            if (genericPartIndex > -1)
            {
                typeName = typeName.Substring(0, genericPartIndex);
            }

            var genericArgs = string.Join(", ", Array.ConvertAll(genericArguments, GetAliases));
            return $"{typeName}<{genericArgs}>";
        }


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

        /// <summary>
        /// Tries to create an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="instance">When this method returns, contains the created instance, or null if creation failed.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>true if an instance was created; otherwise, false.</returns>
        public static bool TryCreateInstance(this Type type, out object instance, params object[] args)
        {
            instance = null;
            try
            {
                instance = type.CreateInstance(args);
                return instance != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to create an instance of the specified type as type T.
        /// </summary>
        /// <typeparam name="T">The type to cast the instance to.</typeparam>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="instance">When this method returns, contains the created instance, or default if creation failed.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>true if an instance was created; otherwise, false.</returns>
        public static bool TryCreateInstance<T>(this Type type, out T instance, params object[] args)
        {
            instance = default;
            try
            {
                instance = type.CreateInstance<T>(args);
                return instance != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>A new instance of the specified type.</returns>
        public static object CreateInstance(this Type type, params object[] args)
        {
            if (type == null)
                return null;

            if (type == typeof(string))
                return string.Empty;

            return Activator.CreateInstance(type, args);
        }

        /// <summary>
        /// Creates an instance of the specified type as type T.
        /// </summary>
        /// <typeparam name="T">The type to cast the instance to.</typeparam>
        /// <param name="type">The type to instantiate.</param>
        /// <param name="args">Constructor arguments.</param>
        /// <returns>A new instance of the specified type cast to T.</returns>
        /// <exception cref="ArgumentException">Generic type T is not assignable from the created instance.</exception>
        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            if (!typeof(T).IsAssignableFrom(type))
                throw new ArgumentException($"Generic type '{typeof(T)}' must be convertible by '{type}'");
            return (T)CreateInstance(type, args);
        }
    }
}
