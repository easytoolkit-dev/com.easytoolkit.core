using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyToolkit.Core.Reflection
{
    public static partial class TypeExtensions
    {
        #region Inheritance

        /// <summary>
        /// Gets all base types and optionally interfaces for the specified type.
        /// </summary>
        /// <param name="type">The type to get base types for.</param>
        /// <param name="includeInterface">Whether to include interfaces in the result.</param>
        /// <param name="includeTargetType">Whether to include the target type itself in the result.</param>
        /// <returns>An array of all base types and optionally interfaces.</returns>
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
        #endregion

        #region Method Query

        /// <summary>
        /// Gets the method overload with the specified parameter count.
        /// </summary>
        /// <param name="targetType">The type to search for the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="flags">Binding flags for the search.</param>
        /// <param name="parameterCount">The number of parameters the method should have.</param>
        /// <returns>The method info matching the criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when targetType or methodName is null.</exception>
        /// <exception cref="ArgumentException">Thrown when methodName is whitespace or method not found.</exception>
        public static MethodInfo GetOverloadMethod(
            [NotNull] this Type targetType,
            [NotNull] string methodName,
            BindingFlags flags,
            int parameterCount)
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

        /// <summary>
        /// Gets the method overload with the specified parameter types.
        /// </summary>
        /// <param name="targetType">The type to search for the method.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="flags">Binding flags for the search.</param>
        /// <param name="parameterTypes">The parameter types to match.</param>
        /// <returns>The method info matching the criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when targetType or methodName is null.</exception>
        /// <exception cref="ArgumentException">Thrown when methodName is whitespace or no matching method found.</exception>
        public static MethodInfo GetOverloadMethod([NotNull] this Type targetType, [NotNull] string methodName,
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

        #endregion

        #region Member Query

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

        #endregion
    }
}
