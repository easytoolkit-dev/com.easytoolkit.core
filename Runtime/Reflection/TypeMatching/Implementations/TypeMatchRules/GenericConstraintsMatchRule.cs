using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolKit.Core.Reflection.Implementations
{
    public sealed class GenericConstraintsMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            var targetIndexByConstraint = new Dictionary<Type, int>();

            {
                var targetIndex = 0;
                for (int i = 0; i < candidate.Constraints.Length; i++)
                {
                    if (!candidate.Constraints[i].SatisfiesGenericParameterConstraints(targets[targetIndex]))
                    {
                        return false;
                    }

                    targetIndexByConstraint.Add(candidate.Constraints[i], targetIndex);
                    targetIndex++;
                }
            }

            var typeArguments = candidate.SourceType.GetGenericArguments();
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

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            var targetIndexByConstraint = new Dictionary<Type, int>();

            {
                var targetIndex = 0;
                for (int i = 0; i < candidate.Constraints.Length; i++)
                {
                    targetIndexByConstraint.Add(candidate.Constraints[i], targetIndex);
                    targetIndex++;
                }
            }

            var typeArguments = candidate.SourceType.GetGenericArguments();
            for (var i = 0; i < typeArguments.Length; i++)
            {
                if (typeArguments[i].IsGenericParameter &&
                    targetIndexByConstraint.TryGetValue(typeArguments[i], out var targetIndex))
                {
                    typeArguments[i] = targets[targetIndex];
                }
            }

            if (typeArguments.Any(type => type.IsGenericParameter))
            {
                candidate.SourceType.GetGenericTypeDefinition()
                    .TryInferTypeArguments(typeArguments, out var inferredTypes);
                typeArguments = inferredTypes;
            }

            return candidate.SourceType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
        }
    }
}
