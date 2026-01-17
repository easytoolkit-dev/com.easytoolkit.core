using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that matches types exactly.
    /// </summary>
    /// <remarks>
    /// This rule performs direct type equality checking. Both the type and all
    /// target types must match exactly for the rule to succeed. This is the
    /// most specific matching rule and is evaluated first.
    /// </remarks>
    public sealed class ExactMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            if (candidate.SourceType.IsGenericTypeDefinition) return false;
            if (targets.Length != candidate.Constraints.Length) return false;

            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] != candidate.Constraints[i]) return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            return candidate.SourceType;
        }
    }
}
