using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Provides analysis capabilities for generic type definitions, extracting parameter constraints and dependencies.
    /// </summary>
    public interface IGenericTypeAnalyzer
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
        /// Attempts to infer generic type arguments from the provided type arguments.
        /// </summary>
        /// <param name="typeArguments">The type arguments used for inference.</param>
        /// <param name="inferredTypes">
        /// When this method returns <c>true</c>, contains the inferred type arguments.
        /// Generic parameters that cannot be inferred retain their original types.
        /// </param>
        /// <returns>
        /// <c>true</c> if at least one type argument was successfully inferred; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of type arguments does not match the number of generic parameters.
        /// </exception>
        bool TryInferTypeArguments(Type[] typeArguments, out Type[] inferredTypes);
    }
}
