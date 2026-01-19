using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
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
                    current = ExecuteStep(step, current);
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
                    current = ExecuteStep(step, current);
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
                    current = ExecuteStep(pathSteps[i], current);
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

            StaticSetter setter = CreateStaticSetter(lastStep);

            return (value) =>
            {
                object current = null;

                // Navigate to the parent of the last step
                for (int i = 0; i < pathSteps.Count - 1; i++)
                {
                    current = ExecuteStep(pathSteps[i], current);
                }

                // Set the value on the last step
                setter.Invoke(value);
            };
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
        /// Creates a static setter delegate for the given path step.
        /// </summary>
        /// <param name="step">The path step containing the member to create a setter for.</param>
        /// <returns>A static setter delegate.</returns>
        /// <exception cref="ArgumentException">Thrown when the member is not a field or property.</exception>
        private StaticSetter CreateStaticSetter(PathStep step)
        {
            return step.Member switch
            {
                FieldInfo field => ReflectionCompiler.CreateStaticFieldSetter(field),
                PropertyInfo property => ReflectionCompiler.CreateStaticPropertySetter(property),
                _ => throw new ArgumentException($"Cannot create setter for '{step.Member.Name}'. Only fields and properties are supported.")
            };
        }
    }
}
