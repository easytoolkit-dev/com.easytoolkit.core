using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Represents the type of step in a reflection path.
    /// </summary>
    public enum PathStepType
    {
        /// <summary>
        /// A field, property, or method member access.
        /// </summary>
        Member,
        /// <summary>
        /// An element access in a non-generic IList collection.
        /// </summary>
        WeakListElement,
        /// <summary>
        /// An element access in a generic IList&lt;T&gt; collection.
        /// </summary>
        StrongListElement,
        /// <summary>
        /// An element access in an array.
        /// </summary>
        ArrayElement
    }

    /// <summary>
    /// Unified delegate type for getting values from a path step, supporting both instance and static members.
    /// </summary>
    /// <param name="instance">The instance to get the value from (null for static members).</param>
    /// <returns>The value retrieved from this step.</returns>
    public delegate object PathStepGetter(object instance);

    /// <summary>
    /// Represents a single step in a reflection path, containing information about how to access
    /// the next value in the path traversal.
    /// </summary>
    public class PathStep
    {
        private PathStepGetter _compiledGetter;
        private bool _isGetterCompiled;

        /// <summary>
        /// Gets the type of this path step.
        /// </summary>
        public PathStepType StepType { get; }

        /// <summary>
        /// Gets the member info for member access steps. Can be null for non-member steps.
        /// </summary>
        public MemberInfo Member { get; }

        /// <summary>
        /// Gets the element index for list/array access steps.
        /// </summary>
        public int ElementIndex { get; }

        /// <summary>
        /// Gets the element type for strongly typed list/array access.
        /// </summary>
        public Type ElementType { get; }

        /// <summary>
        /// Gets the method info for accessing strongly typed list elements.
        /// </summary>
        public MethodInfo StrongListGetItemMethod { get; }

        /// <summary>
        /// Gets the compiled getter delegate for optimized value retrieval.
        /// </summary>
        public PathStepGetter CompiledGetter
        {
            get
            {
                if (!_isGetterCompiled)
                {
                    _compiledGetter = CompileGetter();
                    _isGetterCompiled = true;
                }
                return _compiledGetter;
            }
        }

        /// <summary>
        /// Creates a member access path step.
        /// </summary>
        /// <param name="member">The field, property, or method to access.</param>
        public PathStep(MemberInfo member)
        {
            this.StepType = PathStepType.Member;
            this.Member = member;
            this.ElementIndex = -1;
            this.ElementType = null;
            this.StrongListGetItemMethod = null;
        }

        /// <summary>
        /// Creates a weak list element access path step (non-generic IList).
        /// </summary>
        /// <param name="elementIndex">The index of the element to access.</param>
        public PathStep(int elementIndex)
        {
            this.StepType = PathStepType.WeakListElement;
            this.Member = null;
            this.ElementIndex = elementIndex;
            this.ElementType = null;
            this.StrongListGetItemMethod = null;
        }

        /// <summary>
        /// Creates a strong list or array element access path step.
        /// </summary>
        /// <param name="elementIndex">The index of the element to access.</param>
        /// <param name="strongListElementType">The element type of the list or array.</param>
        /// <param name="isArray">True if this is an array access, false for generic list access.</param>
        public PathStep(int elementIndex, Type strongListElementType, bool isArray)
        {
            this.StepType = isArray ? PathStepType.ArrayElement : PathStepType.StrongListElement;
            this.Member = null;
            this.ElementIndex = elementIndex;
            this.ElementType = strongListElementType;
            this.StrongListGetItemMethod = typeof(IList<>).MakeGenericType(strongListElementType).GetMethod("get_Item");
        }

        /// <summary>
        /// Compiles an optimized getter delegate for this path step using ReflectionCompiler.
        /// </summary>
        /// <returns>A compiled getter delegate for fast value retrieval.</returns>
        private PathStepGetter CompileGetter()
        {
            PathStepGetter stepGetter;
            switch (StepType)
            {
                case PathStepType.Member:
                    if (Member is FieldInfo field)
                    {
                        if (field.IsStatic)
                        {
                            var getter = ReflectionCompiler.CreateStaticFieldGetter(field);
                            stepGetter = _ => getter();
                        }
                        else
                        {
                            var getter = ReflectionCompiler.CreateInstanceFieldGetter(field);
                            stepGetter = instance => getter(instance);
                        }
                        break;
                    }
                    if (Member is PropertyInfo property)
                    {
                        if (property.GetMethod.IsStatic)
                        {
                            var getter = ReflectionCompiler.CreateStaticPropertyGetter(property);
                            stepGetter = _ => getter();
                        }
                        else
                        {
                            var getter = ReflectionCompiler.CreateInstancePropertyGetter(property);
                            stepGetter = instance => getter(instance);
                        }
                        break;
                    }
                    if (Member is MethodInfo method)
                    {
                        if (method.IsStatic)
                        {
                            var getter = ReflectionCompiler.CreateStaticMethodInvoker(method);
                            stepGetter = _ => getter();
                        }
                        else
                        {
                            var getter = ReflectionCompiler.CreateInstanceMethodInvoker(method);
                            stepGetter = instance => getter(instance);
                        }
                        break;
                    }
                    throw new NotSupportedException($"Member type '{Member.GetType()}' is not supported for compiled getters.");

                case PathStepType.WeakListElement:
                    return instance => ((IList)instance)[ElementIndex];

                case PathStepType.ArrayElement:
                    return instance => ((Array)instance).GetValue(ElementIndex);

                case PathStepType.StrongListElement:
                {
                    var getter = ReflectionCompiler.CreateInstanceMethodInvoker(StrongListGetItemMethod);
                    stepGetter = instance => getter(instance, ElementIndex);
                    break;
                }
                default:
                    throw new NotImplementedException($"PathStepType '{StepType}' is not implemented for compiled getters.");
            }

            return stepGetter;
        }

        /// <summary>
        /// Gets a value indicating whether this step is a static member access.
        /// </summary>
        public bool IsStatic
        {
            get
            {
                if (StepType != PathStepType.Member || Member == null)
                    return false;

                return Member.IsStaticMember();
            }
        }

        /// <summary>
        /// Gets the return type of this path step.
        /// </summary>
        /// <returns>The type of the value returned by this step.</returns>
        public Type GetReturnType()
        {
            switch (StepType)
            {
                case PathStepType.Member:
                    if (Member is FieldInfo field)
                        return field.FieldType;
                    if (Member is PropertyInfo property)
                        return property.PropertyType;
                    if (Member is MethodInfo method)
                        return method.ReturnType;
                    throw new NotSupportedException($"Member type '{Member.GetType()}' is not supported.");

                case PathStepType.WeakListElement:
                    return typeof(object);

                case PathStepType.ArrayElement:
                case PathStepType.StrongListElement:
                    return ElementType;

                default:
                    throw new NotImplementedException($"PathStepType '{StepType}' is not implemented.");
            }
        }
    }
}
