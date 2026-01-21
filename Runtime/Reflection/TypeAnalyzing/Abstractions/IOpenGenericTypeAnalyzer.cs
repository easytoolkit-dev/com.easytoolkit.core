using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Reflection
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
        Type Type { get; }

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
        /// Attempts to infer remaining generic type arguments based on substituted parameters and dependencies.
        /// </summary>
        /// <param name="inferredTypes">
        /// When this method returns, contains the type arguments with any inferred types filled in.
        /// Generic parameters that cannot be inferred retain their original types.
        /// For fully constructed types, this contains all concrete types.
        /// </param>
        /// <returns>
        /// <c>true</c> if at least one type argument was successfully inferred; otherwise, <c>false</c>.
        /// Returns <c>false</c> for fully constructed types (no inference needed).
        /// </returns>
        /// <remarks>
        /// This method uses the current type's substituted parameters to infer remaining generic parameters
        /// based on parameter dependencies. For example, in a type like <c>DependencyContainer&lt;List&lt;int&gt;, T&gt;</c>,
        /// if T1 is substituted with <c>List&lt;int&gt;</c> and T1 depends on T2 (e.g., <c>T1 : List&lt;T2&gt;</c>),
        /// this method can infer that T2 should be <c>int</c>.
        /// For fully constructed types where all parameters are already concrete, this method returns <c>false</c>
        /// and provides the current type arguments in the output parameter.
        /// </remarks>
        bool TryInferTypeArguments(out Type[] inferredTypes);
    }
}
