using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Factory for creating and caching type analyzers.
    /// </summary>
    public static class TypeAnalyzerFactory
    {
        private static readonly Dictionary<Type, IGenericTypeDefinitionAnalyzer> GenericTypeAnalyzerCache = new();
        private static readonly Dictionary<Type, IGenericParameterAnalyzer> GenericParameterAnalyzerCache = new();
        private static readonly Dictionary<Type, IOpenGenericTypeAnalyzer> OpenGenericTypeAnalyzerCache = new();

        /// <summary>
        /// Gets a generic type analyzer for the specified generic type definition.
        /// </summary>
        /// <param name="genericType">The generic type definition to analyze.</param>
        /// <returns>An <see cref="IGenericTypeDefinitionAnalyzer"/> for analyzing the generic type definition.</returns>
        public static IGenericTypeDefinitionAnalyzer GetGenericTypeDefinitionAnalyzer(Type genericType)
        {
            if (!genericType.IsGenericTypeDefinition)
                throw new ArgumentException($"Type '{genericType.Name}' must be a generic type definition.",
                    nameof(genericType));

            if (GenericTypeAnalyzerCache.TryGetValue(genericType, out var cached))
            {
                return cached;
            }

            var analyzer = new Implementations.GenericTypeDefinitionAnalyzer(genericType);
            GenericTypeAnalyzerCache[genericType] = analyzer;
            return analyzer;
        }

        /// <summary>
        /// Gets a generic parameter analyzer for the specified generic parameter type.
        /// </summary>
        /// <param name="genericParameterType">The generic parameter type to analyze.</param>
        /// <returns>An <see cref="IGenericParameterAnalyzer"/> for analyzing the generic parameter.</returns>
        public static IGenericParameterAnalyzer GetGenericParameterAnalyzer(Type genericParameterType)
        {
            if (!genericParameterType.IsGenericParameter)
                throw new ArgumentException($"Type '{genericParameterType.Name}' must be a generic parameter.",
                    nameof(genericParameterType));

            if (GenericParameterAnalyzerCache.TryGetValue(genericParameterType, out var cached))
            {
                return cached;
            }

            var analyzer = new Implementations.GenericParameterAnalyzer(genericParameterType);
            GenericParameterAnalyzerCache[genericParameterType] = analyzer;
            return analyzer;
        }

        /// <summary>
        /// Gets an open generic type analyzer for the specified open generic type.
        /// </summary>
        /// <param name="openGenericType">The open generic type to analyze. Can be a generic type definition,
        /// partially constructed type, or fully constructed type.</param>
        /// <returns>An <see cref="IOpenGenericTypeAnalyzer"/> for analyzing the open generic type.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="openGenericType"/> is not a generic type.
        /// </exception>
        public static IOpenGenericTypeAnalyzer GetOpenGenericTypeAnalyzer(Type openGenericType)
        {
            if (!Implementations.OpenGenericTypeAnalyzer.IsValidType(openGenericType))
                throw new ArgumentException($"Type '{openGenericType.Name}' must be a generic type, an array of a generic type, or inherit from a generic type.", nameof(openGenericType));


            if (OpenGenericTypeAnalyzerCache.TryGetValue(openGenericType, out var cached))
            {
                return cached;
            }

            var analyzer = new Implementations.OpenGenericTypeAnalyzer(openGenericType);
            OpenGenericTypeAnalyzerCache[openGenericType] = analyzer;
            return analyzer;
        }
    }
}
