using System;
using System.Collections;
using System.Reflection;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Base class for builders that operate on member paths.
    /// Provides common functionality for parsing and navigating member paths.
    /// </summary>
    public abstract class ReflectionBuilderBase : IReflectionBuilder
    {
        private readonly string _memberPath;

        /// <summary>
        /// Initializes a new instance of the ReflectionBuilderBase.
        /// </summary>
        /// <param name="memberPath">The path to the member (e.g., "Field", "Property", "Nested.Method()").</param>
        protected ReflectionBuilderBase(string memberPath)
        {
            _memberPath = memberPath ?? throw new ArgumentNullException(nameof(memberPath));
        }

        /// <summary>
        /// Gets the member path this builder operates on.
        /// </summary>
        public string MemberPath => _memberPath;

        /// <summary>
        /// Executes a single path step to get the value.
        /// </summary>
        /// <param name="step">The path step to execute.</param>
        /// <param name="instance">The instance to execute the step on.</param>
        /// <returns>The value retrieved from this step.</returns>
        protected object ExecuteStep(PathStep step, object instance)
        {
            switch (step.StepType)
            {
                case PathStepType.Member:
                    if (step.Member is FieldInfo field)
                    {
                        return field.GetValue(instance);
                    }
                    if (step.Member is PropertyInfo property)
                    {
                        return property.GetValue(instance, null);
                    }
                    if (step.Member is MethodInfo method)
                    {
                        return method.Invoke(instance, null);
                    }
                    throw new NotSupportedException($"Member type '{step.Member.GetType()}' is not supported for getting values.");

                case PathStepType.WeakListElement:
                    return ((IList)instance)[step.ElementIndex];

                case PathStepType.ArrayElement:
                    return ((Array)instance).GetValue(step.ElementIndex);

                case PathStepType.StrongListElement:
                    return step.StrongListGetItemMethod.Invoke(instance, new object[] { step.ElementIndex });

                default:
                    throw new NotImplementedException($"PathStepType '{step.StepType}' is not implemented for getting values.");
            }
        }
    }
}
