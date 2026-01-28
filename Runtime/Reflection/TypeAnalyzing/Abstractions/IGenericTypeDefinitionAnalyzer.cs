using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Provides analysis capabilities for generic type definitions, extracting parameter constraints and dependencies.
    /// </summary>
    public interface IGenericTypeDefinitionAnalyzer
    {
        /// <summary>
        /// Gets the generic type definition being analyzed.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets all generic parameter information for the type, including constraints and dependencies.
        /// </summary>
        IReadOnlyList<GenericParameterInfo> Parameters { get; }

        /// <summary>
        /// Gets the parameter information for the parameter with the specified name.
        /// </summary>
        /// <param name="name">The name of the generic parameter (e.g., "T1", "TKey").</param>
        /// <returns>The <see cref="GenericParameterInfo"/> for the parameter with the specified name, or <c>null</c> if not found.</returns>
        GenericParameterInfo GetParameterByName(string name);

        /// <summary>
        /// Gets all generic parameters that have no dependencies on other parameters.
        /// These are the parameters that can be independently specified.
        /// </summary>
        /// <returns>A list of generic parameters that have no dependencies.</returns>
        IReadOnlyList<GenericParameterInfo> GetIndependentParameters();

        /// <summary>
        /// Analyzes whether the specified type arguments satisfy the generic parameter constraints.
        /// </summary>
        /// <param name="typeArguments">The type arguments to validate.</param>
        /// <returns><c>true</c> if all type arguments satisfy their constraints; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of type arguments does not match the number of generic parameters.
        /// </exception>
        bool ValidateTypeArguments(params Type[] typeArguments);

        /// <summary>
        /// Attempts to infer generic type arguments based on the provided input type arguments and parameter dependencies.
        /// </summary>
        /// <param name="inputTypeArguments">
        /// The input type arguments to use for inference. Array positions correspond to generic parameter positions.
        /// Use <c>null</c> for parameters that should be inferred. Empty array means all parameters should be inferred.
        /// </param>
        /// <param name="inferredTypes">
        /// When this method returns, contains the type arguments with any inferred types filled in.
        /// Positions with <c>null</c> in <paramref name="inputTypeArguments"/> are filled with inferred types if successful.
        /// Positions with non-<c>null</c> values in <paramref name="inputTypeArguments"/> retain their original values.
        /// </param>
        /// <returns>
        /// <c>true</c> if at least one type argument was successfully inferred; otherwise, <c>false</c>.
        /// Returns <c>false</c> if no inference could be performed from the provided input.
        /// </returns>
        /// <remarks>
        /// This method uses the provided input type arguments to infer remaining generic parameters
        /// based on parameter dependencies. For example, for a type like <c>Container&lt;T1, T2&gt;</c> where <c>T1 : List&lt;T2&gt;</c>,
        /// if <paramref name="inputTypeArguments"/> provides <c>T1 = List&lt;int&gt;</c> and <c>T2 = null</c>,
        /// this method can infer that <c>T2</c> should be <c>int</c>.
        /// <para/>
        /// The method supports chain inference (e.g., T1 → T2 → T3) through multiple passes.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of input type arguments does not match the number of generic parameters.
        /// </exception>
        bool TryInferTypeArguments(Type[] inputTypeArguments, out Type[] inferredTypes);
    }
}
