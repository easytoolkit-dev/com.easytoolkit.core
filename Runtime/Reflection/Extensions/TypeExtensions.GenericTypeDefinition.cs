using System;

namespace EasyToolkit.Core.Reflection
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Attempts to infer generic type arguments based on the provided input type arguments and parameter dependencies.
        /// </summary>
        /// <param name="genericTypeDefinition">The generic type definition to infer type arguments for.</param>
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
        /// Thrown when <paramref name="genericTypeDefinition"/> is not a generic type definition,
        /// or when the number of input type arguments does not match the number of generic parameters.
        /// </exception>
        public static bool TryInferTypeArguments(this Type genericTypeDefinition,
            Type[] inputTypeArguments,
            out Type[] inferredTypes)
        {
            if (genericTypeDefinition == null)
                throw new ArgumentNullException(nameof(genericTypeDefinition));

            return TypeAnalyzerFactory.GetGenericTypeDefinitionAnalyzer(genericTypeDefinition)
                .TryInferTypeArguments(inputTypeArguments,out inferredTypes);
        }
    }
}
