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
        /// <summary>
        /// Gets all base types of the specified type by traversing the inheritance chain.
        /// </summary>
        /// <param name="type">The type to get base types for.</param>
        /// <param name="includeInterface">Whether to include interfaces implemented by the type.</param>
        /// <param name="includeTargetType">Whether to include the type itself in the result.</param>
        /// <returns>An enumerable collection of all base types in the inheritance chain.</returns>
        /// <remarks>
        /// This method traverses the type hierarchy from immediate base type to <see cref="object"/>.
        /// Base types are returned in inheritance order (closest to farthest).
        /// When <paramref name="includeInterface"/> is true, all interfaces implemented by the type
        /// are included after base types in the result.
        /// </remarks>
        public static IEnumerable<Type> GetAllBaseTypes(this Type type, bool includeInterface = true,
            bool includeTargetType = false)
        {
            if (includeTargetType)
            {
                yield return type;
            }

            var baseType = type.BaseType;

            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }

            if (includeInterface)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    yield return iface;
                }
            }
        }

        /// <summary>
        /// Finds the method with the specified name and parameter count.
        /// </summary>
        /// <param name="targetType">The type to search for the method.</param>
        /// <param name="methodName">The name of the method to find.</param>
        /// <param name="flags">A bitmask comprised of one or more BindingFlags that specify how the search is conducted.</param>
        /// <param name="parameterCount">The number of parameters the method should have.</param>
        /// <returns>The <see cref="MethodInfo"/> matching the criteria.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when no method with the specified name and parameter count is found.
        /// </exception>
        public static MethodInfo FindMethod(
            [NotNull] this Type targetType,
            [NotNull] string methodName,
            BindingFlags flags,
            int parameterCount)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException(nameof(methodName), "Method name cannot be null or whitespace.");

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
        /// Finds the method with the specified name and parameter types.
        /// </summary>
        /// <param name="targetType">The type to search for the method.</param>
        /// <param name="methodName">The name of the method to find.</param>
        /// <param name="flags">A bitmask comprised of one or more BindingFlags that specify how the search is conducted.</param>
        /// <param name="parameterTypes">The parameter types to match. Use null or empty to find a parameterless method.</param>
        /// <returns>The <see cref="MethodInfo"/> matching the parameter types.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when no method with the specified name and parameter types is found.
        /// The exception message includes all available method signatures for debugging.
        /// </exception>
        /// <remarks>
        /// Parameter type matching uses <see cref="Type.IsAssignableFrom"/> to support covariance.
        /// The error message displays all available overloads when no match is found.
        /// </remarks>
        public static MethodInfo FindMethod([NotNull] this Type targetType, [NotNull] string methodName,
            BindingFlags flags, params Type[] parameterTypes)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException(nameof(methodName), "Method name cannot be null or whitespace.");

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
                var paramTypeNames = parameterTypes.Length > 0
                    ? string.Join(", ", parameterTypes.Select(t => t.ToCodeString()))
                    : "none";

                var availableSignatures = string.Join("\n  ",
                    candidates.Select(m =>
                    {
                        var parameters = m.GetParameters();
                        return $"{m.Name}({string.Join(", ", parameters.Select(p => p.ParameterType.ToCodeString()))})";
                    }));

                throw new ArgumentException(
                    $"No matching method overload found for '{methodName}({paramTypeNames})' on type '{targetType.ToCodeString()}'.\n" +
                    $"Available overloads:\n  {availableSignatures}");
            }

            return result;
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
