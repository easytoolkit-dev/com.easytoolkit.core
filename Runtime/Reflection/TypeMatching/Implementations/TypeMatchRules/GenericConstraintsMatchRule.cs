using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    public sealed class GenericConstraintsMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            var targetIndex = 0;
            foreach (var constraint in candidate.Constraints)
            {
                if (constraint.ContainsGenericParameters)
                {
                    if (!constraint.IsStructuralMatchOf(targets[targetIndex++]))
                    {
                        return false;
                    }
                }
            }

            return candidate.SourceType.SatisfiesConstraints(targets);
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            return candidate.SourceType.MakeGenericTypeExtended(targets);
        }
    }
}
