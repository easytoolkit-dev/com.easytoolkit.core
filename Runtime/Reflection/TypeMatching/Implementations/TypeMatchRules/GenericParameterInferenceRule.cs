using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that performs generic parameter inference.
    /// </summary>
    /// <remarks>
    /// This rule handles complex generic type matching by inferring generic parameters
    /// from the target types. It supports scenarios where some target types are generic
    /// parameters and others are concrete types, allowing for flexible generic type resolution.
    /// </remarks>
    public sealed class GenericParameterInferenceRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            return TryGetInferTargets(candidate, targets, out _);
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            TryGetInferTargets(candidate, targets, out var inferTargets);
            candidate.SourceType.TryInferGenericParameters(out var inferredArgs, inferTargets);
            return candidate.SourceType.GetGenericTypeDefinition().MakeGenericType(inferredArgs);
        }

        private static bool TryGetInferTargets(TypeMatchCandidate candidate, Type[] targets, out Type[] inferTargets)
        {
            if (!candidate.SourceType.IsGenericType)
            {
                inferTargets = null;
                return false;
            }

            int genericParameterTargetCount = 0;

            for (int i = 0; i < candidate.Constraints.Length; i++)
            {
                if (candidate.Constraints[i].IsGenericParameter)
                {
                    genericParameterTargetCount++;
                }
                else if (candidate.Constraints[i] != targets[i])
                {
                    inferTargets = null;
                    return false;
                }
            }

            if (genericParameterTargetCount == 0)
            {
                inferTargets = null;
                return false;
            }

            if (genericParameterTargetCount != targets.Length)
            {
                inferTargets = new Type[genericParameterTargetCount];
                int count = 0;
                for (int i = 0; i < candidate.Constraints.Length; i++)
                {
                    if (candidate.Constraints[i].IsGenericParameter)
                    {
                        inferTargets[count++] = targets[i];
                    }
                }
            }
            else
            {
                inferTargets = targets;
            }

            return candidate.SourceType.TryInferGenericParameters(out _, inferTargets);
        }
    }
}
