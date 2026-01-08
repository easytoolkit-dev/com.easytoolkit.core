using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolKit.Core
{
    /// <summary>
    /// Builder for creating member accessors that get or set values through member paths.
    /// </summary>
    public sealed class AccessorBuilder : MemberPathBuilderBase, IAccessorBuilder
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

            return (ref object target) =>
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

            return (ref object target, object value) =>
            {
                object current = target;

                // Navigate to the parent of the last step
                for (int i = 0; i < pathSteps.Count - 1; i++)
                {
                    current = ExecuteStep(pathSteps[i], current);
                }

                // Set the value on the last step
                SetMemberValue(lastStep, current, value);
            };
        }

        /// <inheritdoc/>
        public StaticSetter BuildStaticSetter(Type targetType)
        {
            List<PathStep> pathSteps = MemberPathParser.Parse(targetType, MemberPath, isStatic: true);

            // Validate that the last step is settable (field or property)
            PathStep lastStep = pathSteps[pathSteps.Count - 1];
            ValidateSetter(lastStep);

            return (value) =>
            {
                object current = null;

                // Navigate to the parent of the last step
                for (int i = 0; i < pathSteps.Count - 1; i++)
                {
                    current = ExecuteStep(pathSteps[i], current);
                }

                // Set the value on the last step
                SetMemberValue(lastStep, current, value);
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
        /// Sets a value to a member.
        /// </summary>
        private void SetMemberValue(PathStep step, object instance, object value)
        {
            if (step.StepType != PathStepType.Member)
            {
                throw new NotSupportedException($"Cannot set value to {step.StepType}. Only field and property setters are supported.");
            }

            if (step.Member is FieldInfo field)
            {
                field.SetValue(instance, value);
            }
            else if (step.Member is PropertyInfo property)
            {
                property.SetValue(instance, value, null);
            }
            else
            {
                throw new NotSupportedException($"Cannot set value to method '{step.Member.Name}'. Only fields and properties are supported.");
            }
        }
    }
}
