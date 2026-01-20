using System;
using System.Collections.Generic;
using EasyToolKit.Core.Reflection.Implementations;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Factory for creating and caching type analyzers.
    /// </summary>
    public static class TypeAnalyzerFactory
    {
        private static readonly Dictionary<Type, IGenericTypeAnalyzer> GenericTypeAnalyzerCache = new();
        private static readonly Dictionary<Type, IGenericParameterAnalyzer> GenericParameterAnalyzerCache = new();

        /// <summary>
        /// Gets a generic type analyzer for the specified generic type definition.
        /// </summary>
        /// <param name="genericType">The generic type definition to analyze.</param>
        /// <returns>An <see cref="IGenericTypeAnalyzer"/> for analyzing the generic type.</returns>
        public static IGenericTypeAnalyzer GetGenericTypeAnalyzer(Type genericType)
        {
            if (!genericType.IsGenericTypeDefinition)
                throw new ArgumentException($"Type '{genericType.Name}' must be a generic type definition.",
                    nameof(genericType));

            if (GenericTypeAnalyzerCache.TryGetValue(genericType, out var cached))
            {
                return cached;
            }

            var analyzer = new GenericTypeAnalyzer(genericType);
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

            var analyzer = new GenericParameterAnalyzer(genericParameterType);
            GenericParameterAnalyzerCache[genericParameterType] = analyzer;
            return analyzer;
        }
    }
}
