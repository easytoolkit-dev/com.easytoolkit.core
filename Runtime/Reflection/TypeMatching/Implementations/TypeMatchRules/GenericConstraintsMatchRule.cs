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
                if (constraint.IsGenericParameter)
                {
                    if (targets.Length <= targetIndex)
                    {
                        break;
                    }

                    if (!constraint.SatisfiesGenericParameterConstraints(targets[targetIndex++]))
                    {
                        return false;
                    }
                }
            }

            if (targetIndex != targets.Length)
            {
                return false;
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
