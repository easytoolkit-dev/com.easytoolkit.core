using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides analysis capabilities for open generic types, including partially constructed generic types
    /// and generic type definitions.
    /// </summary>
    public partial class OpenGenericTypeAnalyzer : IOpenGenericTypeAnalyzer
    {
        private readonly Lazy<IReadOnlyList<GenericParameterInfo>> _lazyAllParameters;
        private readonly Lazy<IReadOnlyList<GenericParameterInfo>> _lazyGenericParameters;
        private readonly Lazy<IReadOnlyList<GenericParameterInfo>> _lazySubstitutedParameters;

        /// <inheritdoc />
        public Type OpenGenericType { get; }

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> Parameters => _lazyAllParameters.Value;

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> GenericParameters => _lazyGenericParameters.Value;

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> SubstitutedParameters => _lazySubstitutedParameters.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGenericTypeAnalyzer"/> class.
        /// </summary>
        /// <param name="openGenericType">The open generic type to analyze. Can be a generic type definition,
        /// partially constructed type, fully constructed type, array of such types, or a type
        /// that inherits from a generic type.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="openGenericType"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="openGenericType"/> is not a generic type, an array of a generic type,
        /// or a type that inherits from a generic type.
        /// </exception>
        public OpenGenericTypeAnalyzer(Type openGenericType)
        {
            if (openGenericType == null)
                throw new ArgumentNullException(nameof(openGenericType), "Type cannot be null.");

            if (!IsValidType(openGenericType))
                throw new ArgumentException($"Type '{openGenericType.Name}' must be a generic type, an array of a generic type, or inherit from a generic type.", nameof(openGenericType));

            OpenGenericType = openGenericType;

            _lazyAllParameters = new Lazy<IReadOnlyList<GenericParameterInfo>>(BuildAllParameters);
            _lazyGenericParameters = new Lazy<IReadOnlyList<GenericParameterInfo>>(BuildGenericParameters);
            _lazySubstitutedParameters = new Lazy<IReadOnlyList<GenericParameterInfo>>(BuildSubstitutedParameters);
        }

        internal static bool IsValidType(Type type)
        {
            if (type.IsBasicValueType())
            {
                return false;
            }

            bool isValidType = type.IsGenericType;

            // For array types, check if the element type is generic
            if (!isValidType && type.IsArray)
            {
                var elementType = type.GetElementType();
                isValidType = elementType != null && (elementType.IsGenericType || elementType.IsGenericTypeParameter);
            }

            // For types that inherit from generic types, check base types and interfaces
            if (!isValidType)
            {
                isValidType = HasGenericBaseTypeOrInterface(type);
            }

            return isValidType;
        }

        /// <summary>
        /// Checks if the type has a generic base type or implements a generic interface.
        /// </summary>
        private static bool HasGenericBaseTypeOrInterface(Type type)
        {
            // Check base types
            Type currentBase = type.BaseType;
            while (currentBase != null)
            {
                if (currentBase.IsGenericType)
                    return true;
                currentBase = currentBase.BaseType;
            }

            // Check interfaces
            foreach (var iface in type.GetInterfaces())
            {
                if (iface.IsGenericType)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Finds the first generic type in the type hierarchy.
        /// </summary>
        private static Type FindFirstGenericTypeInHierarchy(Type type)
        {
            // Check base types first
            Type currentBase = type.BaseType;
            while (currentBase != null)
            {
                if (currentBase.IsGenericType)
                    return currentBase;
                currentBase = currentBase.BaseType;
            }

            // Check interfaces
            foreach (var iface in type.GetInterfaces())
            {
                if (iface.IsGenericType)
                    return iface;
            }

            return null;
        }

        /// <summary>
        /// Builds the list of all parameter information, including both generic parameters and substituted parameters.
        /// </summary>
        private IReadOnlyList<GenericParameterInfo> BuildAllParameters()
        {
            // For types that inherit from generic types, find the actual generic type in the hierarchy
            Type effectiveType = OpenGenericType;
            if (!OpenGenericType.IsGenericType)
            {
                effectiveType = FindFirstGenericTypeInHierarchy(OpenGenericType);
                if (effectiveType == null)
                    return Array.Empty<GenericParameterInfo>();
            }

            var typeDefinition = effectiveType.IsGenericTypeDefinition
                ? effectiveType
                : effectiveType.GetGenericTypeDefinition();

            var genericArguments = typeDefinition.GetGenericArguments();
            var currentTypeArguments = effectiveType.GetGenericArguments();

            var result = new List<GenericParameterInfo>(genericArguments.Length);

            for (int i = 0; i < genericArguments.Length; i++)
            {
                var parameterType = genericArguments[i];
                var currentArgument = currentTypeArguments[i];

                // Check if this parameter is substituted
                Type substitutedType = null;
                if (!currentArgument.IsGenericParameter)
                {
                    substitutedType = currentArgument;
                }
                // Check if this is a generic parameter from a different type (nested substitution)
                else if (currentArgument != parameterType)
                {
                    substitutedType = currentArgument;
                }

                // Get the analyzer to retrieve dependencies
                var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(parameterType);

                result.Add(new GenericParameterInfo(
                    parameterType,
                    substitutedType,
                    analyzer.References,
                    analyzer.ReferencedBy
                ));
            }

            return result;
        }

        /// <summary>
        /// Builds the list of only the generic parameters that are still generic (not substituted).
        /// </summary>
        private IReadOnlyList<GenericParameterInfo> BuildGenericParameters()
        {
            // For types that inherit from generic types, find the actual generic type in the hierarchy
            Type effectiveType = OpenGenericType;
            if (!OpenGenericType.IsGenericType)
            {
                effectiveType = FindFirstGenericTypeInHierarchy(OpenGenericType);
                if (effectiveType == null)
                    return Array.Empty<GenericParameterInfo>();
            }

            var typeDefinition = effectiveType.IsGenericTypeDefinition
                ? effectiveType
                : effectiveType.GetGenericTypeDefinition();

            var genericArguments = typeDefinition.GetGenericArguments();
            var currentTypeArguments = effectiveType.GetGenericArguments();

            var result = new List<GenericParameterInfo>();

            for (int i = 0; i < genericArguments.Length; i++)
            {
                var parameterType = genericArguments[i];
                var currentArgument = currentTypeArguments[i];

                // Only include if this is still a generic parameter (not substituted)
                if (currentArgument.IsGenericParameter && currentArgument == parameterType)
                {
                    var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(parameterType);

                    result.Add(new GenericParameterInfo(
                        parameterType,
                        null, // Not substituted
                        analyzer.References,
                        analyzer.ReferencedBy
                    ));
                }
            }

            return result;
        }

        /// <summary>
        /// Builds the list of only the parameters that have been substituted with concrete types.
        /// </summary>
        private IReadOnlyList<GenericParameterInfo> BuildSubstitutedParameters()
        {
            // For types that inherit from generic types, find the actual generic type in the hierarchy
            Type effectiveType = OpenGenericType;
            if (!OpenGenericType.IsGenericType)
            {
                effectiveType = FindFirstGenericTypeInHierarchy(OpenGenericType);
                if (effectiveType == null)
                    return Array.Empty<GenericParameterInfo>();
            }

            var typeDefinition = effectiveType.IsGenericTypeDefinition
                ? effectiveType
                : effectiveType.GetGenericTypeDefinition();

            var genericArguments = typeDefinition.GetGenericArguments();
            var currentTypeArguments = effectiveType.GetGenericArguments();

            var result = new List<GenericParameterInfo>();

            for (int i = 0; i < genericArguments.Length; i++)
            {
                var parameterType = genericArguments[i];
                var currentArgument = currentTypeArguments[i];

                // Check if this parameter is substituted with a concrete type
                if (!currentArgument.IsGenericParameter)
                {
                    var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(parameterType);

                    result.Add(new GenericParameterInfo(
                        parameterType,
                        currentArgument,
                        analyzer.References,
                        analyzer.ReferencedBy
                    ));
                }
                // Check if this is a generic parameter from a different type (nested substitution)
                else if (currentArgument != parameterType)
                {
                    var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(parameterType);

                    result.Add(new GenericParameterInfo(
                        parameterType,
                        currentArgument,
                        analyzer.References,
                        analyzer.ReferencedBy
                    ));
                }
            }

            return result;
        }

        /// <inheritdoc />
        public bool IsImplementsGenericDefinition(Type genericTypeDefinition)
        {
            return Helper.IsImplementsGenericDefinition(OpenGenericType, genericTypeDefinition);
        }

        /// <inheritdoc />
        public Type[] GetGenericArgumentsRelativeTo(Type genericTypeDefinition)
        {
            return Helper.GetGenericArgumentsRelativeTo(OpenGenericType, genericTypeDefinition);
        }

        /// <inheritdoc />
        public Type[] GetCompletedGenericArguments(Type targetType, bool allowTypeInheritance = false)
        {
            return Helper.GetCompletedGenericArguments(OpenGenericType, targetType, allowTypeInheritance);
        }
    }
}
