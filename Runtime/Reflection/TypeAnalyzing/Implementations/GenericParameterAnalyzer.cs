using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides analysis capabilities for a single generic type parameter, extracting constraints and dependencies.
    /// </summary>
    public class GenericParameterAnalyzer : IGenericParameterAnalyzer
    {
        private readonly Lazy<IReadOnlyList<Type>> _lazyDependsOn;
        private readonly Lazy<IReadOnlyList<Type>> _lazyDependedOnBy;
        private readonly Lazy<GenericParameterInfo> _lazyParameterInfo;

        /// <inheritdoc />
        public Type ParameterType { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public int Position { get; }

        /// <inheritdoc />
        public GenericParameterSpecialConstraints SpecialConstraints { get; }

        /// <inheritdoc />
        public IReadOnlyList<Type> TypeConstraints { get; }

        /// <inheritdoc />
        public IReadOnlyList<Type> DependsOn => _lazyDependsOn.Value;

        /// <inheritdoc />
        public IReadOnlyList<Type> DependedOnBy => _lazyDependedOnBy.Value;

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
            SpecialConstraints = ExtractSpecialConstraints(genericParameterType);
            TypeConstraints = genericParameterType.GetGenericParameterConstraints().ToList();

            _lazyDependsOn = new Lazy<IReadOnlyList<Type>>(ResolveDependsOn);
            _lazyDependedOnBy = new Lazy<IReadOnlyList<Type>>(ResolveDependedOnBy);
            _lazyParameterInfo = new Lazy<GenericParameterInfo>(BuildParameterInfo);
        }

        private GenericParameterSpecialConstraints ExtractSpecialConstraints(Type genericParameter)
        {
            var attributes = genericParameter.GenericParameterAttributes;
            var constraints = GenericParameterSpecialConstraints.None;

            if ((attributes & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
            {
                constraints |= GenericParameterSpecialConstraints.ReferenceType;
            }

            if ((attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
            {
                constraints |= GenericParameterSpecialConstraints.ValueType;
            }

            if ((attributes & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
            {
                constraints |= GenericParameterSpecialConstraints.DefaultConstructor;
            }

            return constraints;
        }

        private IReadOnlyList<Type> ResolveDependsOn()
        {
            var allGenericParameters = ParameterType.DeclaringType.GetGenericArguments();
            var dependencies = new List<Type>();

            foreach (var constraint in TypeConstraints)
            {
                FindGenericParametersInType(constraint, allGenericParameters, dependencies);
            }

            return dependencies;
        }

        private IReadOnlyList<Type> ResolveDependedOnBy()
        {
            var allGenericParameters = ParameterType.DeclaringType.GetGenericArguments();
            var dependents = new List<Type>();

            foreach (var parameter in allGenericParameters)
            {
                // Skip self
                if (parameter.GenericParameterPosition == Position &&
                    parameter.DeclaringMethod == ParameterType.DeclaringMethod &&
                    parameter.DeclaringType == ParameterType.DeclaringType)
                {
                    continue;
                }

                // Get the analyzer for this parameter and check its dependencies
                var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(parameter);
                if (analyzer.DependsOn.Contains(ParameterType))
                {
                    dependents.Add(parameter);
                }
            }

            return dependents;
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
            // Get analyzers for dependencies to build GenericParameterInfo objects
            return new GenericParameterInfo(
                Name,
                Position,
                SpecialConstraints,
                TypeConstraints,
                DependsOn,
                DependedOnBy
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

            // Check if dependentParameter is in DependedOnBy list
            if (!DependedOnBy.Contains(dependentParameter))
                return false;

            // Get the analyzer for the dependent parameter
            var dependentAnalyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(dependentParameter);

            // Search through all constraints of the dependent parameter
            foreach (var constraint in dependentAnalyzer.TypeConstraints)
            {
                if (TryFindParameterInConstraint(constraint, dependentParameterType, ParameterType, out var foundType))
                {
                    inferredType = foundType;
                    return true;
                }
            }

            return false;
        }

        private bool TryFindParameterInConstraint(Type constraint, Type concreteType, Type targetParameter, out Type inferredType)
        {
            inferredType = null;

            // Direct match: constraint is the target parameter
            if (constraint.IsGenericParameter && IsSameGenericParameter(constraint, targetParameter))
            {
                inferredType = concreteType;
                return true;
            }

            // Handle generic types: search recursively in generic arguments
            if (constraint.IsGenericType)
            {
                // Get the generic type definition of the constraint
                var constraintDef = constraint.GetGenericTypeDefinition();

                // Check if concreteType is directly a matching generic type
                if (concreteType.IsGenericType)
                {
                    var concreteDef = concreteType.GetGenericTypeDefinition();

                    // Direct match: generic type definitions are the same
                    if (constraintDef == concreteDef)
                    {
                        var constraintArgs = constraint.GetGenericArguments();
                        var concreteArgs = concreteType.GetGenericArguments();

                        // Recursively search in generic arguments
                        for (int i = 0; i < constraintArgs.Length; i++)
                        {
                            if (TryFindParameterInConstraint(constraintArgs[i], concreteArgs[i], targetParameter, out var found))
                            {
                                inferredType = found;
                                return true;
                            }
                        }
                    }
                }

                // Handle interface implementations and type inheritance
                // Use GetCompletedGenericArguments with allowTypeInheritance to support derived types
                if (concreteType.IsDerivedFromGenericDefinition(constraintDef))
                {
                    try
                    {
                        // Extract type arguments from the concrete type relative to the constraint definition
                        var extractedArgs = constraint.GetCompletedGenericArguments(concreteType, true);
                        var constraintArgs = constraint.GetGenericArguments();

                        // Map the extracted arguments to the constraint parameters
                        for (int i = 0; i < constraintArgs.Length && i < extractedArgs.Length; i++)
                        {
                            if (TryFindParameterInConstraint(constraintArgs[i], extractedArgs[i], targetParameter, out var found))
                            {
                                inferredType = found;
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        // If GetCompletedGenericArguments fails, continue to other cases
                    }
                }
            }

            // Handle arrays (arrays can implement generic interfaces like IList<T>)
            if (concreteType.IsArray && constraint.IsGenericType)
            {
                try
                {
                    var constraintDef = constraint.GetGenericTypeDefinition();

                    // Check if array types implement this generic interface
                    if (concreteType.IsDerivedFromGenericDefinition(constraintDef))
                    {
                        // Use GetGenericArgumentsRelativeTo to extract type arguments from the implemented interface
                        var extractedArgs = concreteType.GetGenericArgumentsRelativeTo(constraintDef);
                        var constraintArgs = constraint.GetGenericArguments();

                        for (int i = 0; i < constraintArgs.Length && i < extractedArgs.Length; i++)
                        {
                            if (TryFindParameterInConstraint(constraintArgs[i], extractedArgs[i], targetParameter, out var found))
                            {
                                inferredType = found;
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                    // If extraction fails, continue
                }
            }

            // Handle constraint arrays and concrete type arrays
            if (constraint.IsArray && concreteType.IsArray)
            {
                var constraintRank = constraint.GetArrayRank();
                var concreteRank = concreteType.GetArrayRank();

                if (constraintRank == concreteRank)
                {
                    var constraintElement = constraint.GetElementType();
                    var concreteElement = concreteType.GetElementType();

                    if (constraintElement != null && concreteElement != null)
                    {
                        return TryFindParameterInConstraint(constraintElement, concreteElement, targetParameter, out inferredType);
                    }
                }
            }

            return false;
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
            if ((constraints & GenericParameterSpecialConstraints.ReferenceType) != 0)
            {
                if (targetType.IsValueType)
                    return false;
            }

            // Check value type constraint
            if ((constraints & GenericParameterSpecialConstraints.ValueType) != 0)
            {
                if (!targetType.IsValueType)
                    return false;
            }

            // Check default constructor constraint
            if ((constraints & GenericParameterSpecialConstraints.DefaultConstructor) != 0)
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
                    constraint.GetCompletedGenericArguments(targetType, true);
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
