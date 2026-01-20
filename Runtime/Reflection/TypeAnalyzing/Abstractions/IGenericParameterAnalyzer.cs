using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Provides analysis capabilities for a single generic type parameter, extracting constraints and dependencies.
    /// </summary>
    public interface IGenericParameterAnalyzer
    {
        /// <summary>
        /// Gets the generic parameter type being analyzed.
        /// </summary>
        Type ParameterType { get; }

        /// <summary>
        /// Gets the name of the generic parameter (e.g., "T1", "TKey").
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the zero-based position of the generic parameter in the parameter list.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Gets the special constraints applied to this generic parameter.
        /// </summary>
        GenericParameterSpecialConstraints SpecialConstraints { get; }

        /// <summary>
        /// Gets the type constraints (base class and interface constraints) applied to this generic parameter.
        /// </summary>
        IReadOnlyList<Type> TypeConstraints { get; }

        /// <summary>
        /// Gets the generic parameters that this parameter depends on.
        /// For example, if T1 has constraint List&lt;T2&gt;, then T1 depends on T2.
        /// </summary>
        IReadOnlyList<Type> DependsOn { get; }

        /// <summary>
        /// Gets the generic parameters that depend on this parameter.
        /// For example, if T1 has constraint List&lt;T2&gt;, then T2 is depended on by T1.
        /// </summary>
        IReadOnlyList<Type> DependedOnBy { get; }

        /// <summary>
        /// Gets the complete parameter information, including dependencies as <see cref="GenericParameterInfo"/> objects.
        /// </summary>
        GenericParameterInfo ParameterInfo { get; }

        /// <summary>
        /// Determines whether the specified type satisfies all constraints of this generic parameter.
        /// </summary>
        /// <param name="targetType">The type to check against the generic parameter constraints.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="targetType"/> satisfies all special constraints and type constraints;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method checks both special constraints (e.g., class, struct, default constructor)
        /// and type constraints (base class and interface constraints).
        /// </remarks>
        bool SatisfiesConstraints(Type targetType);

        /// <summary>
        /// Tries to infer the type of this generic parameter based on the known type of a dependent parameter.
        /// For example, if T1 has constraint List&lt;T2&gt; and T1's type is List&lt;int&gt;,
        /// this method can infer that T2 is int.
        /// </summary>
        /// <param name="dependentParameter">
        /// The generic parameter that depends on this parameter (must be from <see cref="DependedOnBy"/>).
        /// </param>
        /// <param name="dependentParameterType">
        /// The known concrete type of the dependent parameter.
        /// </param>
        /// <param name="inferredType">
        /// When this method returns, contains the inferred type of this generic parameter if inference succeeded;
        /// otherwise, null.
        /// </param>
        /// <returns>
        /// <c>true</c> if the type was successfully inferred; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method searches through all constraints of the dependent parameter to find references
        /// to the current parameter, then extracts the corresponding type argument from the dependent parameter type.
        /// </remarks>
        bool TryInferTypeFrom(Type dependentParameter, Type dependentParameterType, out Type inferredType);
    }
}
