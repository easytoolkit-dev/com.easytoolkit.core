using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides analysis capabilities for a single generic type parameter, extracting constraints and dependencies.
    /// </summary>
    public class GenericParameterAnalyzer : IGenericParameterAnalyzer
    {
        private readonly Lazy<IReadOnlyList<Type>> _lazyReferences;
        private readonly Lazy<IReadOnlyList<Type>> _lazyReferencedBy;
        private readonly Lazy<GenericParameterInfo> _lazyParameterInfo;

        /// <inheritdoc />
        public Type ParameterType { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public int Position { get; }

        /// <inheritdoc />
        public GenericParameterAttributes SpecialConstraints { get; }

        /// <inheritdoc />
        public IReadOnlyList<Type> TypeConstraints { get; }

        /// <inheritdoc />
        public IReadOnlyList<Type> References => _lazyReferences.Value;

        /// <inheritdoc />
        public IReadOnlyList<Type> ReferencedBy => _lazyReferencedBy.Value;

        /// <inheritdoc />
        public GenericParameterInfo ParameterInfo => _lazyParameterInfo.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterAnalyzer"/> class.
        /// </summary>
        /// <param name="genericParameterType">The generic parameter type to analyze.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="genericParameterType"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="genericParameterType"/> is not a generic parameter
        /// or is a method generic parameter.
        /// </exception>
        public GenericParameterAnalyzer(Type genericParameterType)
        {
            if (genericParameterType == null)
                throw new ArgumentNullException(nameof(genericParameterType), "Generic parameter type cannot be null.");
            if (!genericParameterType.IsGenericParameter)
                throw new ArgumentException($"Type '{genericParameterType.Name}' must be a generic parameter.", nameof(genericParameterType));
            if (genericParameterType.DeclaringMethod != null)
                throw new ArgumentException($"Type '{genericParameterType.Name}' must be a type generic parameter, not a method generic parameter.", nameof(genericParameterType));

            ParameterType = genericParameterType;
            Name = genericParameterType.Name;
            Position = genericParameterType.GenericParameterPosition;
            SpecialConstraints = genericParameterType.GenericParameterAttributes;
            TypeConstraints = genericParameterType.GetGenericParameterConstraints().ToList();

            _lazyReferences = new Lazy<IReadOnlyList<Type>>(ResolveReferences);
            _lazyReferencedBy = new Lazy<IReadOnlyList<Type>>(ResolveReferencedBy);
            _lazyParameterInfo = new Lazy<GenericParameterInfo>(BuildParameterInfo);
        }

        private IReadOnlyList<Type> ResolveReferences()
        {
            var allGenericParameters = ParameterType.DeclaringType.GetGenericArguments();
            var references = new List<Type>();

            foreach (var constraint in TypeConstraints)
            {
                FindGenericParametersInType(constraint, allGenericParameters, references);
            }

            return references;
        }

        private IReadOnlyList<Type> ResolveReferencedBy()
        {
            var allGenericParameters = ParameterType.DeclaringType.GetGenericArguments();
            var referencedBy = new List<Type>();

            foreach (var parameter in allGenericParameters)
            {
                // Skip self
                if (parameter.GenericParameterPosition == Position &&
                    parameter.DeclaringMethod == ParameterType.DeclaringMethod &&
                    parameter.DeclaringType == ParameterType.DeclaringType)
                {
                    continue;
                }

                // Get the analyzer for this parameter and check its references
                var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(parameter);
                if (analyzer.References.Contains(ParameterType))
                {
                    referencedBy.Add(parameter);
                }
            }

            return referencedBy;
        }

        private void FindGenericParametersInType(Type type, Type[] allGenericParameters, List<Type> foundTypes)
        {
            if (type == null)
                return;

            // If this is a generic parameter, check if it belongs to our type
            if (type.IsGenericParameter)
            {
                for (int i = 0; i < allGenericParameters.Length; i++)
                {
                    if (type.GenericParameterPosition == allGenericParameters[i].GenericParameterPosition &&
                        type.DeclaringMethod == allGenericParameters[i].DeclaringMethod &&
                        type.DeclaringType == allGenericParameters[i].DeclaringType)
                    {
                        if (!foundTypes.Contains(allGenericParameters[i]))
                        {
                            foundTypes.Add(allGenericParameters[i]);
                        }
                        break;
                    }
                }
            }

            // Recursively check generic arguments
            if (type.IsGenericType)
            {
                foreach (var arg in type.GetGenericArguments())
                {
                    FindGenericParametersInType(arg, allGenericParameters, foundTypes);
                }
            }

            // Check element type for arrays
            if (type.HasElementType)
            {
                FindGenericParametersInType(type.GetElementType(), allGenericParameters, foundTypes);
            }
        }

        private GenericParameterInfo BuildParameterInfo()
        {
            // Get analyzers for references to build GenericParameterInfo objects
            return new GenericParameterInfo(
                ParameterType,
                null, // No substituted type in this context
                References,
                ReferencedBy
            );
        }

        /// <inheritdoc />
        public bool SatisfiesConstraints(Type targetType)
        {
            if (targetType == null)
                return false;

            // Check special constraints
            if (!CheckSpecialConstraints(targetType))
                return false;

            // Check type constraints (base class and interface constraints)
            if (!CheckTypeConstraints(targetType))
                return false;

            return true;
        }

        /// <inheritdoc />
        public bool TryInferTypeFrom(Type dependentParameter, Type dependentParameterType, out Type inferredType)
        {
            inferredType = null;

            if (dependentParameter == null || dependentParameterType == null)
                return false;

            // Check if dependentParameter is in ReferencedBy list
            if (!ReferencedBy.Contains(dependentParameter))
                return false;

            // Get the analyzer for the dependent parameter
            var dependentAnalyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(dependentParameter);

            // Search through all constraints of the dependent parameter
            foreach (var constraint in dependentAnalyzer.TypeConstraints)
            {
                if (TryFindParameterInConstraint(constraint, dependentParameterType, out var foundType))
                {
                    inferredType = foundType;
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public int[] FindPositionPathIn(Type targetType)
        {
            if (targetType == null)
                return null;

            var path = new List<int>();
            if (TryFindParameterPath(targetType, ParameterType, path))
            {
                return path.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Recursively finds the path to a generic parameter within a type's generic hierarchy.
        /// </summary>
        /// <param name="type">The type to search in.</param>
        /// <param name="targetParameter">The generic parameter to find.</param>
        /// <param name="path">The current path being built.</param>
        /// <returns><c>true</c> if the parameter was found; otherwise, <c>false</c>.</returns>
        private bool TryFindParameterPath(Type type, Type targetParameter, List<int> path)
        {
            // Direct match: type itself is the target generic parameter
            if (type.IsGenericParameter && IsSameGenericParameter(type, targetParameter))
            {
                return true;
            }

            // Search in generic arguments if this is a generic type
            if (type.IsGenericType)
            {
                var genericArgs = type.GetGenericArguments();
                for (int i = 0; i < genericArgs.Length; i++)
                {
                    path.Add(i);
                    if (TryFindParameterPath(genericArgs[i], targetParameter, path))
                    {
                        return true;
                    }
                    path.RemoveAt(path.Count - 1);
                }
            }

            // Check element type for arrays
            if (type.HasElementType)
            {
                return TryFindParameterPath(type.GetElementType(), targetParameter, path);
            }

            return false;
        }

        private bool TryFindParameterInConstraint(Type constraint, Type concreteType, out Type inferredType)
        {
            inferredType = null;

            // Direct match: constraint is the target parameter
            if (constraint.IsGenericParameter && IsSameGenericParameter(constraint, ParameterType))
            {
                inferredType = concreteType;
                return true;
            }

            try
            {
                // Get the position path of target parameter in the constraint
                var fullPath = FindPositionPathIn(constraint);
                if (fullPath == null)
                {
                    return false;
                }

                // If the path only has one level, the target is directly in constraint's generic arguments
                if (fullPath.Length == 1)
                {
                    int targetIndex = fullPath[0];
                    var completedArgs = TypeAnalyzerFactory.GetOpenGenericTypeAnalyzer(constraint)
                        .GetCompletedGenericArguments(concreteType, true);
                    inferredType = completedArgs[targetIndex];
                    return true;
                }
                else
                {
                    // For nested types, navigate to the parent type first
                    // Create parent path by removing the last element
                    var parentPath = fullPath.Take(fullPath.Length - 1).ToArray();
                    var parentConstraint = NavigatePath(constraint, parentPath);
                    var parentConcrete = NavigatePath(concreteType, parentPath);

                    if (parentConstraint == null || parentConcrete == null)
                    {
                        return false;
                    }

                    // Use the last index to get the target from parent's generic arguments
                    int targetIndex = fullPath[^1];
                    var completedArgs = TypeAnalyzerFactory.GetOpenGenericTypeAnalyzer(parentConstraint)
                        .GetCompletedGenericArguments(parentConcrete, true);
                    inferredType = completedArgs[targetIndex];
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Navigates through a generic type hierarchy following the specified path of generic argument indices.
        /// </summary>
        /// <param name="type">The starting type.</param>
        /// <param name="path">The path of indices to follow through generic arguments.</param>
        /// <returns>The type at the specified path, or null if navigation fails.</returns>
        private static Type NavigatePath(Type type, int[] path)
        {
            if (type == null || path == null || path.Length == 0)
                return null;

            Type current = type;
            foreach (int index in path)
            {
                if (!current.IsGenericType)
                    return null;

                var args = current.GetGenericArguments();
                if (index < 0 || index >= args.Length)
                    return null;

                current = args[index];
            }

            return current;
        }

        private bool IsSameGenericParameter(Type param1, Type param2)
        {
            if (!param1.IsGenericParameter || !param2.IsGenericParameter)
                return false;

            return param1.GenericParameterPosition == param2.GenericParameterPosition &&
                   param1.DeclaringMethod == param2.DeclaringMethod &&
                   param1.DeclaringType == param2.DeclaringType;
        }

        private bool CheckSpecialConstraints(Type targetType)
        {
            var constraints = SpecialConstraints;

            // Check reference type constraint
            if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
            {
                if (targetType.IsValueType)
                    return false;
            }

            // Check value type constraint
            if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
            {
                if (!targetType.IsValueType)
                    return false;
            }

            // Check default constructor constraint
            if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
            {
                // Value types have default constructors
                if (targetType.IsValueType)
                    return true;

                // Reference types must have a public parameterless constructor
                var constructor = targetType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (constructor == null)
                    return false;
            }

            return true;
        }

        private bool CheckTypeConstraints(Type targetType)
        {
            foreach (var constraint in TypeConstraints)
            {
                // Skip if constraint is System.ValueType (already handled by special constraint)
                if (constraint == typeof(ValueType))
                    continue;

                // Check if targetType implements the interface or derives from the base class
                if (constraint.IsAssignableFrom(targetType))
                    continue;

                try
                {
                    TypeAnalyzerFactory.GetOpenGenericTypeAnalyzer(constraint)
                        .GetCompletedGenericArguments(targetType, true);
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
