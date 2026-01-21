using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that matches types exactly.
    /// </summary>
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
