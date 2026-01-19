using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyToolKit.Core.Diagnostics;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyToolKit.Core.Reflection
{
    public static class TypeExtensions
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

        public static string GetAliases(this Type t)
        {
            if (TypeAliasesByType.TryGetValue(t, out string alias))
            {
                return alias;
            }

            // If not found in the alias dictionary, return the full name without the namespace
            return t.IsGenericType ? GetGenericTypeName(t) : t.Name;
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
                // Lenient mode: any constructor is acceptable
                var ctors = type.GetConstructors(MemberAccessFlags.AllInstance);
                return ctors.Length > 0;
            }
            else
            {
                // Strict mode: requires parameterless constructor
                var ctor = type.GetConstructor(
                    MemberAccessFlags.AllInstance,
                    binder: null,
                    types: Type.EmptyTypes,
                    modifiers: null);
                return ctor != null;
            }
        }


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

        public static object CreateInstance(this Type type, params object[] args)
        {
            if (type == null)
                return null;

            if (type == typeof(string))
                return string.Empty;

            return Activator.CreateInstance(type, args);
        }


        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            if (!typeof(T).IsAssignableFrom(type))
                throw new ArgumentException($"Generic type '{typeof(T)}' must be convertible by '{type}'");
            return (T)CreateInstance(type, args);
        }

        public static bool IsStructType(this Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        public static bool IsUnityBuiltInType(this Type type)
        {
            return type == typeof(Vector2) || type == typeof(Vector2Int) || type == typeof(Vector3) ||
                   type == typeof(Vector3Int) ||
                   type == typeof(Vector4) || type == typeof(Quaternion) || type == typeof(Color) ||
                   type == typeof(Color32) ||
                   type == typeof(Rect) || type == typeof(RectInt) || type == typeof(Bounds) ||
                   type == typeof(BoundsInt);
        }

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

        public static bool IsBooleanType(this Type type)
        {
            return type == typeof(bool);
        }

        public static bool IsStringType(this Type type)
        {
            return type == typeof(string);
        }

        public static bool IsBasicValueType([NotNull] this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsEnum || type.IsStringType() || type.IsBooleanType() || type.IsFloatingPointType() ||
                   type.IsIntegerType();
        }

        public static bool IsUnityObjectType([NotNull] this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.IsDerivedFrom<UnityEngine.Object>();
        }

        public static bool IsNullableType(this Type type)
        {
            return !(type.IsPrimitive || type.IsValueType || type.IsEnum);
        }


        public static MethodInfo ResolveOverloadMethod(
            [NotNull] this Type targetType,
            [NotNull] string methodName,
            BindingFlags flags, int parameterCount)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("Method name cannot be null or whitespace.", nameof(methodName));

            var result = targetType.GetMethods(flags)
                .FirstOrDefault(info => info.Name == methodName && info.GetParameters().Length == parameterCount);
            if (result == null)
            {
                throw new ArgumentException(
                    $"Method '{methodName}' with parameter count '{parameterCount}' not found in type '{targetType}'.");
            }

            return result;
        }

        public static MethodInfo ResolveOverloadMethod([NotNull] this Type targetType, [NotNull] string methodName,
            BindingFlags flags, params Type[] parameterTypes)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("Method name cannot be null or whitespace.", nameof(methodName));

            var candidates = targetType.GetMethods(flags).Where(info => info.Name == methodName).ToArray();

            var result = candidates.FirstOrDefault(info =>
            {
                var parameters = info.GetParameters();
                if (parameterTypes == null)
                {
                    return parameters.Length == 0;
                }

                if (parameterTypes.Length != parameters.Length)
                {
                    return false;
                }

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!parameters[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                    {
                        return false;
                    }
                }

                return true;
            });
            if (result == null)
            {
                // Build error message
                string paramTypeNames = parameterTypes.Length > 0
                    ? string.Join(", ", parameterTypes.Select(t => t.GetAliases()))
                    : "none";

                string availableSignatures = string.Join("\n  ",
                    candidates.Select(m =>
                    {
                        var parameters = m.GetParameters();
                        return $"{m.Name}({string.Join(", ", parameters.Select(p => p.ParameterType.GetAliases()))})";
                    }));

                throw new ArgumentException(
                    $"No matching method overload found for '{methodName}({paramTypeNames})' on type '{targetType.GetAliases()}'.\n" +
                    $"Available overloads:\n  {availableSignatures}");
            }

            return result;
        }

        public static Type[] GetAllBaseTypes(this Type type, bool includeInterface = true,
            bool includeTargetType = false)
        {
            var parentTypes = new List<Type>();

            if (includeTargetType)
            {
                parentTypes.Add(type);
            }

            var baseType = type.BaseType;

            while (baseType != null)
            {
                parentTypes.Add(baseType);
                baseType = baseType.BaseType;
            }

            if (includeInterface)
            {
                foreach (var i in type.GetInterfaces())
                {
                    parentTypes.Add(i);
                }
            }

            return parentTypes.ToArray();
        }

        public static bool IsDerivedFrom<T>(this Type type)
        {
            return type.IsDerivedFrom(typeof(T));
        }

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

                if (baseType.IsGenericTypeDefinition && currentType.IsGenericType && currentType.GetGenericTypeDefinition() == baseType)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }

        public static bool IsGenericArray(this Type type)
        {
            if (!type.IsArray)
                return false;

            var elementType = type.GetElementType();
            return elementType != null && elementType.IsGenericParameter;
        }

        /// <summary>
        /// Gets all members of the specified type, including inherited members, using the specified binding flags.
        /// </summary>
        /// <param name="type">The type to get members from.</param>
        /// <param name="flags">A bitmask comprised of one or more BindingFlags that specify how the search is conducted.</param>
        /// <returns>An enumerable collection of all members matching the binding flags.</returns>
        public static IEnumerable<MemberInfo> GetAllMembers(this Type type, BindingFlags flags)
        {
            if ((flags & BindingFlags.DeclaredOnly) == BindingFlags.DeclaredOnly)
            {
                var members = type.GetMembers(flags);
                for (int i = 0; i < members.Length; i++)
                {
                    yield return members[i];
                }

                yield break;
            }

            flags |= BindingFlags.DeclaredOnly;

            var currentType = type;
            var baseTypes = new Stack<Type>();
            do
            {
                baseTypes.Push(currentType);
                currentType = currentType.BaseType;
            } while (currentType != null);

            while (baseTypes.Count > 0)
            {
                currentType = baseTypes.Pop();
                var members = currentType.GetMembers(flags);
                for (int i = 0; i < members.Length; i++)
                {
                    yield return members[i];
                }
            }
        }

        /// <summary>
        /// Gets all members of the specified type with the given name, including inherited members, using the specified binding flags.
        /// </summary>
        /// <param name="type">The type to get members from.</param>
        /// <param name="name">The name of the member to find.</param>
        /// <param name="flags">A bitmask comprised of one or more BindingFlags that specify how the search is conducted.</param>
        /// <returns>An enumerable collection of members matching the binding flags and specified name.</returns>
        public static IEnumerable<MemberInfo> GetAllMembers(this Type type, string name, BindingFlags flags)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Member name cannot be null or whitespace.", nameof(name));
            }

            return type.GetAllMembers(flags).Where(member => member.Name == name);
        }
    }
}
