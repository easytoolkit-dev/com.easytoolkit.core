using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that performs generic parameter inference to construct concrete generic types.
    /// </summary>
    /// <remarks>
    /// This rule handles generic type matching by inferring type arguments from target types through constraint analysis.
    /// It enables flexible generic type resolution when constraints contain a mix of generic parameters and concrete types.
    /// </remarks>
    public sealed class GenericParameterInferenceRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            return candidate.SourceType.SatisfiesConstraints(targets);
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            return candidate.SourceType.MakeGenericTypeExtended(targets);
        }
    }
}
