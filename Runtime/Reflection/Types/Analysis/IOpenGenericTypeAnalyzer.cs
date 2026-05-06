using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Provides analysis capabilities for generic types, including generic type definitions,
    /// partially constructed generic types, and fully constructed generic types.
    /// </summary>
    /// <remarks>
    /// This analyzer supports the following generic type scenarios:
    /// <list type="bullet">
    /// <item>Generic type definitions (e.g., <c>List&lt;T&gt;</c>) - all parameters are generic</item>
    /// <item>Partially constructed types (e.g., <c>Dictionary&lt;string, T&gt;</c>) - some parameters substituted</item>
    /// <item>Fully constructed types (e.g., <c>List&lt;int&gt;</c>) - all parameters are concrete types</item>
    /// </list>
    /// </remarks>
    public interface IOpenGenericTypeAnalyzer
    {
        /// <summary>
        /// Gets the generic type being analyzed.
        /// </summary>
        Type OpenGenericType { get; }

        /// <summary>
        /// Gets all generic parameter information for the type, including both generic parameters
        /// and substituted parameters.
        /// </summary>
        /// <remarks>
        /// For a partially constructed type like <c>Dictionary&lt;string, T&gt;</c>, this includes
        /// both the substituted parameter (string) and the remaining generic parameter (T).
        /// </remarks>
        IReadOnlyList<GenericParameterInfo> Parameters { get; }

        /// <summary>
        /// Gets all generic parameter information for the type, including constraints and dependencies.
        /// Only includes parameters that are still generic parameters (not yet constructed).
        /// </summary>
        /// <remarks>
        /// For a generic type definition like <c>List&lt;T&gt;</c>, this includes T.
        /// For a partially constructed type like <c>Dictionary&lt;string, T&gt;</c>, this only includes T (not string).
        /// </remarks>
        IReadOnlyList<GenericParameterInfo> GenericParameters { get; }

        /// <summary>
        /// Gets only the generic parameter information for parameters that have been substituted
        /// with concrete types.
        /// </summary>
        /// <remarks>
        /// For a partially constructed type like <c>Dictionary&lt;string, T&gt;</c>, this only includes
        /// the substituted parameter (string), not the remaining generic parameter (T).
        /// For a fully generic type like <c>List&lt;T&gt;</c>, this returns an empty collection.
        /// </remarks>
        IReadOnlyList<GenericParameterInfo> SubstitutedParameters { get; }

        /// <summary>
        /// Determines whether the analyzed type implements the given generic type definition,
        /// including interface implementations and inheritance hierarchy.
        /// </summary>
        /// <param name="genericTypeDefinition">The generic type definition to compare with. Must be a generic type definition.</param>
        /// <returns>
        /// <c>true</c> if the analyzed type implements or inherits from a type constructed from
        /// <paramref name="genericTypeDefinition"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="genericTypeDefinition"/> is not a generic type definition.
        /// </exception>
        bool IsImplementsGenericDefinition(Type genericTypeDefinition);

        /// <summary>
        /// Gets the generic type arguments relative to the specified generic type definition.
        /// </summary>
        /// <param name="genericTypeDefinition">The generic type definition to compare with. Must be a generic type definition.</param>
        /// <returns>
        /// An array of type arguments that correspond to the generic parameters of <paramref name="genericTypeDefinition"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="genericTypeDefinition"/> is not a generic type definition.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the analyzed type does not implement or inherit from <paramref name="genericTypeDefinition"/>.
        /// </exception>
        Type[] GetGenericArgumentsRelativeTo(Type genericTypeDefinition);

        /// <summary>
        /// Gets the supplementary generic type arguments from a target type relative to the analyzed open generic type.
        /// Supports array types (e.g., T[], List&lt;T&gt;[]) by recursively processing element types.
        /// </summary>
        /// <param name="targetType">The target type from which to extract type arguments.</param>
        /// <param name="allowTypeInheritance">If true, allows type inheritance when extracting generic arguments.</param>
        /// <returns>An array of supplementary generic type arguments.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when array ranks don't match or when types are incompatible.
        /// </exception>
        Type[] GetCompletedGenericArguments(Type targetType, bool allowTypeInheritance = false);
    }
}
