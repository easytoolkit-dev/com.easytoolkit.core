using System;
using System.Collections.Generic;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
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
        GenericParameterAttributes SpecialConstraints { get; }

        /// <summary>
        /// Gets the type constraints (base class and interface constraints) applied to this generic parameter.
        /// </summary>
        IReadOnlyList<Type> TypeConstraints { get; }

        /// <summary>
        /// Gets the generic parameters that this parameter references.
        /// For example, if T1 has constraint List&lt;T2&gt;, then T1 references T2.
        /// </summary>
        IReadOnlyList<Type> References { get; }

        /// <summary>
        /// Gets the generic parameters that are referenced by this parameter.
        /// For example, if T1 has constraint List&lt;T2&gt;, then T2 is referenced by T1.
        /// </summary>
        IReadOnlyList<Type> ReferencedBy { get; }

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
        bool SatisfiesConstraints(Type targetType);

        /// <summary>
        /// Tries to infer the type of this generic parameter based on the known type of a dependent parameter.
        /// For example, if T1 has constraint List&lt;T2&gt; and T1's type is List&lt;int&gt;,
        /// this method can infer that T2 is int.
        /// </summary>
        /// <param name="dependentParameter">
        /// The generic parameter that references this parameter (must be from <see cref="ReferencedBy"/>).
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
        bool TryInferTypeFrom(Type dependentParameter, Type dependentParameterType, out Type inferredType);

        /// <summary>
        /// Finds the position path of this generic parameter within the target type's generic argument hierarchy.
        /// </summary>
        /// <param name="targetType">The type to search for this generic parameter.</param>
        /// <returns>
        /// An array of indices representing the path to this generic parameter, or <c>null</c> if not found.
        /// Each index represents the position at each nesting level.
        /// <para>For example, if this parameter is T and targetType is <see cref="Dictionary{TKey, TValue}">Dictionary&lt;int, List&lt;T&gt;&gt;</see>,
        /// returns [1, 0] meaning T is at position 0 of the type at position 1 (List&lt;T&gt;).</para>
        /// </returns>
        /// <remarks>
        /// This method traverses the generic type hierarchy to find where this parameter is used.
        /// The returned path can be used to extract the actual type argument from a constructed generic type.
        /// </remarks>
        int[] FindPositionPathIn(Type targetType);
    }
}
