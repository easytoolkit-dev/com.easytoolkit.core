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
    public class OpenGenericTypeAnalyzer : IOpenGenericTypeAnalyzer
    {
        private readonly Lazy<IReadOnlyList<GenericParameterInfo>> _lazyAllParameters;
        private readonly Lazy<IReadOnlyList<GenericParameterInfo>> _lazyGenericParameters;
        private readonly Lazy<IReadOnlyList<GenericParameterInfo>> _lazySubstitutedParameters;

        /// <inheritdoc />
        public Type Type { get; }

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> Parameters => _lazyAllParameters.Value;

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> GenericParameters => _lazyGenericParameters.Value;

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> SubstitutedParameters => _lazySubstitutedParameters.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGenericTypeAnalyzer"/> class.
        /// </summary>
        /// <param name="type">The open generic type to analyze. Can be a generic type definition,
        /// partially constructed type, or fully constructed type.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="type"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="type"/> is not a generic type.
        /// </exception>
        public OpenGenericTypeAnalyzer(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Type cannot be null.");
            if (!type.IsGenericType)
                throw new ArgumentException($"Type '{type.Name}' must be a generic type.", nameof(type));

            Type = type;

            _lazyAllParameters = new Lazy<IReadOnlyList<GenericParameterInfo>>(BuildAllParameters);
            _lazyGenericParameters = new Lazy<IReadOnlyList<GenericParameterInfo>>(BuildGenericParameters);
            _lazySubstitutedParameters = new Lazy<IReadOnlyList<GenericParameterInfo>>(BuildSubstitutedParameters);
        }

        /// <summary>
        /// Builds the list of all parameter information, including both generic parameters and substituted parameters.
        /// </summary>
        private IReadOnlyList<GenericParameterInfo> BuildAllParameters()
        {
            var typeDefinition = Type.IsGenericTypeDefinition
                ? Type
                : Type.GetGenericTypeDefinition();

            var genericArguments = typeDefinition.GetGenericArguments();
            var currentTypeArguments = Type.GetGenericArguments();

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
                    analyzer.DependsOn,
                    analyzer.DependedOnBy
                ));
            }

            return result;
        }

        /// <summary>
        /// Builds the list of only the generic parameters that are still generic (not substituted).
        /// </summary>
        private IReadOnlyList<GenericParameterInfo> BuildGenericParameters()
        {
            var typeDefinition = Type.IsGenericTypeDefinition
                ? Type
                : Type.GetGenericTypeDefinition();

            var genericArguments = typeDefinition.GetGenericArguments();
            var currentTypeArguments = Type.GetGenericArguments();

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
                        analyzer.DependsOn,
                        analyzer.DependedOnBy
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
            var typeDefinition = Type.IsGenericTypeDefinition
                ? Type
                : Type.GetGenericTypeDefinition();

            var genericArguments = typeDefinition.GetGenericArguments();
            var currentTypeArguments = Type.GetGenericArguments();

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
                        analyzer.DependsOn,
                        analyzer.DependedOnBy
                    ));
                }
                // Check if this is a generic parameter from a different type (nested substitution)
                else if (currentArgument != parameterType)
                {
                    var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(parameterType);

                    result.Add(new GenericParameterInfo(
                        parameterType,
                        currentArgument,
                        analyzer.DependsOn,
                        analyzer.DependedOnBy
                    ));
                }
            }

            return result;
        }

        /// <inheritdoc />
        public bool TryInferTypeArguments(out Type[] inferredTypes)
        {
            // Get the type definition to retrieve all generic parameter positions
            var typeDefinition = Type.IsGenericTypeDefinition
                ? Type
                : Type.GetGenericTypeDefinition();

            var genericParameters = typeDefinition.GetGenericArguments();
            var currentTypeArguments = Type.GetGenericArguments();

            // Build a map of parameter analyzers by position and name
            var analyzersByPosition = new Dictionary<int, IGenericParameterAnalyzer>(genericParameters.Length);
            var analyzersByName = new Dictionary<string, IGenericParameterAnalyzer>(genericParameters.Length, StringComparer.Ordinal);

            foreach (var genericParameter in genericParameters)
            {
                var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(genericParameter);
                analyzersByPosition[analyzer.Position] = analyzer;
                analyzersByName[analyzer.Name] = analyzer;
            }

            // Start with a copy of the current type arguments
            inferredTypes = (Type[])currentTypeArguments.Clone();

            // If all types are already concrete (no generic parameters), return false
            if (!currentTypeArguments.Any(t => t.IsGenericParameter))
            {
                return false;
            }

            bool anyInferred = false;
            bool changedInPass;

            // Multiple passes to support chain inference (e.g., T1 -> T2 -> T3)
            do
            {
                changedInPass = false;

                // Try to infer types for parameters that are still generic parameters
                for (int i = 0; i < inferredTypes.Length; i++)
                {
                    // Skip if already a concrete type (not a generic parameter)
                    if (!inferredTypes[i].IsGenericParameter)
                    {
                        continue;
                    }

                    var currentAnalyzer = analyzersByPosition[i];

                    // Try to infer from parameters that this one depends on
                    foreach (var dependsOnType in currentAnalyzer.DependsOn)
                    {
                        // Find the analyzer and type of the depended-on parameter
                        var dependedOnAnalyzer = analyzersByName[dependsOnType.Name];
                        int dependedOnIndex = dependedOnAnalyzer.Position;
                        var dependedOnType = inferredTypes[dependedOnIndex];

                        // Skip if depended-on parameter is also not inferred (still generic parameter)
                        if (dependedOnType == null || dependedOnType.IsGenericParameter)
                        {
                            continue;
                        }

                        // Try to infer using the depended-on parameter's type
                        if (currentAnalyzer.TryInferTypeFrom(dependsOnType, dependedOnType, out var inferredType))
                        {
                            inferredTypes[i] = inferredType;
                            anyInferred = true;
                            changedInPass = true;
                            break; // Successfully inferred, move to next parameter
                        }
                    }
                }
            }
            while (changedInPass); // Continue until no more types can be inferred in a pass

            return anyInferred;
        }
    }
}
