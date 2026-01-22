using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides analysis capabilities for generic type definitions, extracting parameter constraints and dependencies.
    /// </summary>
    public class GenericTypeDefinitionAnalyzer : IGenericTypeDefinitionAnalyzer
    {
        private readonly Dictionary<int, IGenericParameterAnalyzer> _analyzersByPosition;
        private readonly Dictionary<string, IGenericParameterAnalyzer> _analyzersByName;
        private readonly Lazy<IReadOnlyList<GenericParameterInfo>> _parameters;

        /// <inheritdoc />
        public Type Type { get; }

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> Parameters => _parameters.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericTypeDefinitionAnalyzer"/> class.
        /// </summary>
        /// <param name="type">The generic type definition to analyze.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="type"/> is not a generic type definition.
        /// </exception>
        public GenericTypeDefinitionAnalyzer(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type), "Type cannot be null.");
            if (!type.IsGenericTypeDefinition)
                throw new ArgumentException($"Type '{type.Name}' must be a generic type definition.", nameof(type));

            Type = type;
            var genericParameters = type.GetGenericArguments();

            _analyzersByPosition = new Dictionary<int, IGenericParameterAnalyzer>(genericParameters.Length);
            _analyzersByName = new Dictionary<string, IGenericParameterAnalyzer>(genericParameters.Length, StringComparer.Ordinal);

            foreach (var genericParameter in genericParameters)
            {
                var analyzer = TypeAnalyzerFactory.GetGenericParameterAnalyzer(genericParameter);
                _analyzersByPosition[analyzer.Position] = analyzer;
                _analyzersByName[analyzer.Name] = analyzer;
            }

            // Lazy initialization of Parameters
            _parameters = new Lazy<IReadOnlyList<GenericParameterInfo>>(() =>
                _analyzersByPosition.Values.OrderBy(a => a.Position).Select(a => a.ParameterInfo).ToList());
        }

        /// <inheritdoc />
        public GenericParameterInfo GetParameterByName(string name)
        {
            return _analyzersByName.TryGetValue(name, out var analyzer) ? analyzer.ParameterInfo : null;
        }

        /// <inheritdoc />
        public IReadOnlyList<GenericParameterInfo> GetIndependentParameters()
        {
            return Parameters.Where(p => !p.HasDependencies).ToList();
        }

        /// <inheritdoc />
        public bool ValidateTypeArguments(params Type[] typeArguments)
        {
            if (typeArguments == null)
                throw new ArgumentNullException(nameof(typeArguments));

            if (typeArguments.Length != _analyzersByPosition.Count)
                throw new ArgumentException(
                    $"Expected {_analyzersByPosition.Count} type arguments, but got {typeArguments.Length}.",
                    nameof(typeArguments));

            // Build a map of parameter names to type arguments for dependency validation
            var genericParameters = Type.GetGenericArguments();
            var typeArgumentsByName = genericParameters
                .Zip(typeArguments, (param, arg) => (param.Name, arg))
                .ToDictionary(x => x.Name, x => x.arg, StringComparer.Ordinal);

            // Validate each type argument against its constraints using IGenericParameterAnalyzer
            for (int i = 0; i < typeArguments.Length; i++)
            {
                var analyzer = _analyzersByPosition[i];
                var typeArgument = typeArguments[i];

                if (!analyzer.SatisfiesConstraints(typeArgument))
                {
                    return false;
                }

                // Validate type constraints, substituting generic parameters with actual types
                foreach (var constraint in analyzer.TypeConstraints)
                {
                    var substitutedConstraint = SubstituteGenericParameters(constraint, typeArgumentsByName);
                    if (!substitutedConstraint.IsAssignableFrom(typeArgument))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc />
        public bool TryInferTypeArguments(Type[] inputTypeArguments, out Type[] inferredTypes)
        {
            if (inputTypeArguments == null)
                throw new ArgumentNullException(nameof(inputTypeArguments));

            if (inputTypeArguments.Length != _analyzersByPosition.Count)
                throw new ArgumentException(
                    $"Expected {_analyzersByPosition.Count} type arguments, but got {inputTypeArguments.Length}.",
                    nameof(inputTypeArguments));

            // Get the generic parameters
            var genericParameters = Type.GetGenericArguments();

            // Start with a copy of the input type arguments
            inferredTypes = (Type[])inputTypeArguments.Clone();

            // Check if all types are already provided (no nulls to infer)
            if (!inputTypeArguments.Any(t => t == null || t.IsGenericParameter))
            {
                return false;
            }

            // If all input types are null (nothing to infer from), return false
            if (inputTypeArguments.All(t => t == null || t.IsGenericParameter))
            {
                return false;
            }

            bool anyInferred = false;
            bool changedInPass;

            // Build a map of parameter names to type arguments for dependency resolution
            var typeArgumentsByName = genericParameters
                .Zip(inferredTypes, (param, arg) => (param.Name, arg))
                .ToDictionary(x => x.Name, x => x.arg, StringComparer.Ordinal);

            // Multiple passes to support chain inference (e.g., T1 -> T2 -> T3)
            do
            {
                changedInPass = false;

                // Try to infer types for parameters that are still null
                for (int i = 0; i < inferredTypes.Length; i++)
                {
                    // Skip if already provided (not null)
                    if (inferredTypes[i] != null && !inferredTypes[i].IsGenericParameter)
                    {
                        continue;
                    }

                    var currentAnalyzer = _analyzersByPosition[i];

                    // Try to infer from parameters that reference this one
                    foreach (var referencedByType in currentAnalyzer.ReferencedBy)
                    {
                        // Find the analyzer and type of the referenced-by parameter
                        var referencedByAnalyzer = _analyzersByName[referencedByType.Name];
                        int referencedByIndex = referencedByAnalyzer.Position;
                        var referencedByTypeValue = inferredTypes[referencedByIndex];

                        // Skip if referenced-by parameter is also not provided (null)
                        if (referencedByTypeValue == null)
                        {
                            continue;
                        }

                        // Try to infer using the referenced-by parameter's type
                        if (currentAnalyzer.TryInferTypeFrom(referencedByType, referencedByTypeValue, out var inferredType))
                        {
                            inferredTypes[i] = inferredType;
                            typeArgumentsByName[genericParameters[i].Name] = inferredType;
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

        private Type SubstituteGenericParameters(Type type, Dictionary<string, Type> typeArgumentsByName)
        {
            if (type == null)
            {
                return null;
            }

            // Check if this is a generic parameter from our type
            if (type.IsGenericParameter && typeArgumentsByName.TryGetValue(type.Name, out var substituted))
            {
                return substituted;
            }

            // For non-generic types or already-constructed generic types with no generic parameters, return as-is
            if (!type.IsGenericType || !type.ContainsGenericParameters)
            {
                return type;
            }

            // For open generic types (contain generic parameters), recursively substitute
            var genericArgs = type.GetGenericArguments();
            var substitutedArgs = new Type[genericArgs.Length];
            bool anySubstituted = false;

            for (int i = 0; i < genericArgs.Length; i++)
            {
                var original = genericArgs[i];
                substituted = SubstituteGenericParameters(original, typeArgumentsByName);
                substitutedArgs[i] = substituted;

                if (substituted != original)
                {
                    anySubstituted = true;
                }
            }

            // Only reconstruct if we actually substituted something
            if (anySubstituted)
            {
                return type.GetGenericTypeDefinition().MakeGenericType(substitutedArgs);
            }

            return type;
        }
    }
}
