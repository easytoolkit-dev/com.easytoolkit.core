using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Parses member paths into a list of PathStep objects for reflection access.
    /// </summary>
    public static class MemberPathParser
    {
        /// <summary>
        /// Parses a member path string into a list of path steps.
        /// </summary>
        /// <param name="rootType">The root type to start parsing from.</param>
        /// <param name="path">The member path to parse (e.g., "MyField.NestedProperty[0]").</param>
        /// <param name="isStatic">Whether the root member is static.</param>
        /// <returns>A list of PathStep objects representing the path.</returns>
        /// <exception cref="ArgumentException">Thrown when the path is invalid or cannot be parsed.</exception>
        public static List<PathStep> Parse(Type rootType, string path, bool isStatic)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Invalid path; is null or whitespace.");
            }

            List<PathStep> result = new List<PathStep>();
            string[] steps = path.Split('.');
            Type currentType = rootType;
            bool foundRootStatic = false;

            for (int i = 0; i < steps.Length; i++)
            {
                string step = steps[i];
                bool expectMethod = false;

                // Handle array/list element access: "[0]"
                if (step.StartsWith("[", StringComparison.InvariantCulture) &&
                    step.EndsWith("]", StringComparison.InvariantCulture))
                {
                    int index;
                    string indexStr = step.Substring(1, step.Length - 2);

                    if (!int.TryParse(indexStr, out index))
                    {
                        throw new ArgumentException($"Couldn't parse an index from the path step '{step}'.");
                    }

                    // Determine the collection type and create appropriate step
                    if (currentType.IsArray)
                    {
                        Type elementType = currentType.GetElementType();
                        result.Add(new PathStep(index, elementType, isArray: true));
                        currentType = elementType;
                    }
                    else if (currentType.IsDerivedFromGenericDefinition(typeof(IList<>)))
                    {
                        Type elementType = currentType.GetGenericArgumentsRelativeTo(typeof(IList<>))[0];
                        result.Add(new PathStep(index, elementType, isArray: false));
                        currentType = elementType;
                    }
                    else if (typeof(IList).IsAssignableFrom(currentType))
                    {
                        result.Add(new PathStep(index));
                        currentType = typeof(object);
                    }
                    else
                    {
                        throw new ArgumentException($"Cannot get elements by index from the type '{currentType.Name}'.");
                    }

                    continue;
                }

                // Handle method calls: "Method()"
                if (step.EndsWith("()", StringComparison.InvariantCulture))
                {
                    expectMethod = true;
                    step = step.Substring(0, step.Length - 2);
                }

                // Get the member for this step
                MemberInfo member = GetStepMember(currentType, step, expectMethod);

                // Check static member constraints
                if (member.IsStatic())
                {
                    if (currentType == rootType)
                    {
                        foundRootStatic = true;
                    }
                    else
                    {
                        throw new ArgumentException($"The non-root member '{step}' is static; use that member as the path root instead.");
                    }
                }

                currentType = GetMemberReturnType(member);

                // Validate method return type
                if (expectMethod && (currentType == null || currentType == typeof(void)))
                {
                    throw new ArgumentException($"The method '{member.Name}' has no return type and cannot be part of a deep reflection path.");
                }

                result.Add(new PathStep(member));
            }

            // Validate static expectation
            if (isStatic && !foundRootStatic)
            {
                throw new ArgumentException($"The root of given path '{path}' is not static.");
            }
            if (!isStatic && foundRootStatic)
            {
                throw new ArgumentException($"The root of given path '{path}' is static, but instance access was requested.");
            }

            return result;
        }

        /// <summary>
        /// Gets the member for a single path step.
        /// </summary>
        /// <param name="owningType">The type that owns the member.</param>
        /// <param name="name">The name of the member.</param>
        /// <param name="expectMethod">Whether a method is expected.</param>
        /// <returns>The found member info.</returns>
        /// <exception cref="ArgumentException">Thrown when the member cannot be found.</exception>
        private static MemberInfo GetStepMember(Type owningType, string name, bool expectMethod)
        {
            MemberInfo result = null;
            MemberInfo[] possibleMembers = owningType.GetAllMembers(name, MemberAccessFlags.All).ToArray();
            int stepMethodParameterCount = int.MaxValue;

            for (int j = 0; j < possibleMembers.Length; j++)
            {
                MemberInfo member = possibleMembers[j];

                if (expectMethod)
                {
                    MethodInfo method = member as MethodInfo;

                    if (method != null)
                    {
                        int parameterCount = method.GetParameters().Length;

                        if (result == null || parameterCount < stepMethodParameterCount)
                        {
                            result = method;
                            stepMethodParameterCount = parameterCount;
                        }
                    }
                }
                else
                {
                    if (member is MethodInfo)
                    {
                        throw new ArgumentException($"Found method member for name '{name}', but expected a field or property.");
                    }

                    result = member;
                    break;
                }
            }

            if (result == null)
            {
                throw new ArgumentException($"Could not find expected {(expectMethod ? "method" : "field or property")} '{name}' on type '{owningType}' while parsing reflection path.");
            }

            if (expectMethod && stepMethodParameterCount > 0)
            {
                throw new NotSupportedException($"Method '{result}' has {stepMethodParameterCount} parameters, but method parameters are currently not supported in path expressions. Use ReflectionFactory.CreateInvoker for parameterized methods.");
            }

            if ((result is FieldInfo || result is PropertyInfo || result is MethodInfo) == false)
            {
                throw new NotSupportedException($"Members of type {result.GetType()} are not support; only fields, properties and methods are supported.");
            }

            return result;
        }

        /// <summary>
        /// Gets the return type of a member.
        /// </summary>
        /// <param name="member">The member to get the return type from.</param>
        /// <returns>The return type of the member.</returns>
        private static Type GetMemberReturnType(MemberInfo member)
        {
            if (member is FieldInfo field)
                return field.FieldType;

            if (member is PropertyInfo property)
                return property.PropertyType;

            if (member is MethodInfo method)
                return method.ReturnType;

            throw new NotSupportedException($"Member type '{member.GetType()}' is not supported.");
        }
    }
}
