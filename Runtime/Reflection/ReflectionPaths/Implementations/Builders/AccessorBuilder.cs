using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Builder for creating member accessors that get or set values through member paths.
    /// </summary>
    public sealed class AccessorBuilder : ReflectionBuilderBase, IAccessorBuilder
    {
        /// <summary>
        /// Initializes a new instance of the AccessorBuilder.
        /// </summary>
        /// <param name="memberPath">The path to the member (e.g., "Field", "Property", "Nested.Field").</param>
        public AccessorBuilder(string memberPath) : base(memberPath)
        {
        }

        /// <inheritdoc/>
        public StaticGetter BuildStaticGetter(Type targetType)
        {
            List<PathStep> pathSteps = MemberPathParser.Parse(targetType, MemberPath, isStatic: true);

            return () =>
            {
                object current = null;
                foreach (var step in pathSteps)
                {
                    current = step.CompiledGetter(current);
                }
                return current;
            };
        }

        /// <inheritdoc/>
        public InstanceGetter BuildInstanceGetter(Type targetType)
        {
            List<PathStep> pathSteps = MemberPathParser.Parse(targetType, MemberPath, isStatic: false);

            return target =>
            {
                object current = target;
                foreach (var step in pathSteps)
                {
                    current = step.CompiledGetter(current);
                }
                return current;
            };
        }

        /// <inheritdoc/>
        public InstanceSetter BuildInstanceSetter(Type targetType)
        {
            List<PathStep> pathSteps = MemberPathParser.Parse(targetType, MemberPath, isStatic: false);

            // Validate that the last step is settable (field or property)
            PathStep lastStep = pathSteps[pathSteps.Count - 1];
            ValidateSetter(lastStep);

            InstanceSetter setter = CreateInstanceSetter(lastStep);

            return (target, value) =>
            {
                object current = target;

                // Navigate to the parent of the last step
                for (int i = 0; i < pathSteps.Count - 1; i++)
                {
                    current = pathSteps[i].CompiledGetter(current);
                }

                // Set the value on the last step
                setter.Invoke(current, value);
            };
        }

        /// <inheritdoc/>
        public StaticSetter BuildStaticSetter(Type targetType)
        {
            List<PathStep> pathSteps = MemberPathParser.Parse(targetType, MemberPath, isStatic: true);

            // Validate that the last step is settable (field or property)
            PathStep lastStep = pathSteps[pathSteps.Count - 1];
            ValidateSetter(lastStep);

            // Create the appropriate setter based on whether the final member is static or instance
            StaticSetter staticSetter = CreateStaticSetter(lastStep, out InstanceSetter instanceSetter);

            // If the final member is an instance member, we need to navigate to the parent and use instance setter
            if (instanceSetter != null)
            {
                return (value) =>
                {
                    object current = null;

                    // Navigate to the parent of the last step
                    for (int i = 0; i < pathSteps.Count - 1; i++)
                    {
                        current = pathSteps[i].CompiledGetter(current);
                    }

                    // Set the value on the last step using the instance setter
                    instanceSetter.Invoke(current, value);
                };
            }
            else
            {
                // Static member - use the static setter directly
                return staticSetter;
            }
        }

        /// <summary>
        /// Validates that the last path step can be set.
        /// </summary>
        private void ValidateSetter(PathStep step)
        {
            if (step.StepType != PathStepType.Member)
            {
                throw new ArgumentException($"Cannot set value to {step.StepType}. Only fields and properties can be set.");
            }

            if (step.Member is MethodInfo)
            {
                throw new ArgumentException($"Cannot set value to method '{step.Member.Name}'. Only fields and properties are supported.");
            }
        }

        /// <summary>
        /// Creates an instance setter delegate for the given path step.
        /// </summary>
        /// <param name="step">The path step containing the member to create a setter for.</param>
        /// <returns>An instance setter delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member is not a field or property.</exception>
        private InstanceSetter CreateInstanceSetter(PathStep step)
        {
            return step.Member switch
            {
                FieldInfo field => ReflectionCompiler.CreateInstanceFieldSetter(field),
                PropertyInfo property => ReflectionCompiler.CreateInstancePropertySetter(property),
                _ => throw new ArgumentException($"Cannot create setter for '{step.Member.Name}'. Only fields and properties are supported.")
            };
        }

        /// <summary>
        /// Creates a setter delegate for the given path step.
        /// For nested paths, the final step may be an instance field/property on a statically accessed object.
        /// This method returns both a static setter (for static members) and an instance setter (for instance members).
        /// </summary>
        /// <param name="step">The path step containing the member to create a setter for.</param>
        /// <param name="instanceSetter">Output parameter for the instance setter (used when the member is an instance member).</param>
        /// <returns>A static setter delegate, or null if an instance setter should be used instead.</returns>
        /// <exception cref="ArgumentException">Thrown when the member is not a field or property.</exception>
        private StaticSetter CreateStaticSetter(PathStep step, out InstanceSetter instanceSetter)
        {
            instanceSetter = null;

            if (step.Member is FieldInfo field)
            {
                if (field.IsStatic)
                {
                    return ReflectionCompiler.CreateStaticFieldSetter(field);
                }
                else
                {
                    // For instance fields in nested static paths, return an instance setter
                    instanceSetter = ReflectionCompiler.CreateInstanceFieldSetter(field);
                    return null;
                }
            }
            else if (step.Member is PropertyInfo property)
            {
                if (property.GetMethod.IsStatic)
                {
                    return ReflectionCompiler.CreateStaticPropertySetter(property);
                }
                else
                {
                    // For instance properties in nested static paths, return an instance setter
                    instanceSetter = ReflectionCompiler.CreateInstancePropertySetter(property);
                    return null;
                }
            }
            else
            {
                throw new ArgumentException($"Cannot create setter for '{step.Member.Name}'. Only fields and properties are supported.");
            }
        }
    }
}
