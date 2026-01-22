using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolKit.Core.Reflection.Implementations
{
    public sealed class GenericParameterConstraintsMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            return TryInferTypeArguments(candidate, targets, out _);
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            TryInferTypeArguments(candidate, targets, out var typeArguments);
            return candidate.SourceType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
        }

        private bool TryInferTypeArguments(TypeMatchCandidate candidate, Type[] targets, out Type[] typeArguments)
        {
            typeArguments = null;
            var targetIndexByConstraint = new Dictionary<Type, int>();

            {
                var targetIndex = 0;
                for (int i = 0; i < candidate.Constraints.Length; i++)
                {
                    if (!candidate.Constraints[i].IsGenericParameter)
                        continue;

                    if (!candidate.Constraints[i].SatisfiesGenericParameterConstraints(targets[targetIndex]))
                    {
                        return false;
                    }

                    targetIndexByConstraint.Add(candidate.Constraints[i], targetIndex);
                    targetIndex++;
                }
            }

            typeArguments = candidate.SourceType.GetGenericArguments();
            for (var i = 0; i < typeArguments.Length; i++)
            {
                if (typeArguments[i].IsGenericParameter &&
                    targetIndexByConstraint.TryGetValue(typeArguments[i], out var targetIndex))
                {
                    typeArguments[i] = targets[targetIndex];
                }
            }

            try
            {
                if (typeArguments.Any(type => type.IsGenericParameter))
                {
                    if (candidate.SourceType.GetGenericTypeDefinition()
                        .TryInferTypeArguments(typeArguments, out var inferredTypes))
                    {
                        if (inferredTypes.Any(type => type.IsGenericParameter))
                        {
                            return false;
                        }

                        typeArguments = inferredTypes;
                        return true;
                    }
                    return false;
                }

                return candidate.SourceType.GetGenericTypeDefinition().SatisfiesConstraints(typeArguments);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
