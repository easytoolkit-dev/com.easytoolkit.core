using System;
using System.Linq;

namespace EasyToolKit.Core.Reflection.Implementations
{
    public sealed class GenericConstraintsMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            var typeArguments = candidate.SourceType.GetMergedGenericArguments(targets);

            try
            {
                var genericType = candidate.SourceType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
                if (genericType.ContainsGenericParameters)
                {
                    if (!genericType.TryInferTypeArguments(out var inferredTypes))
                    {
                        return false;
                    }

                    if (inferredTypes.Any(type => type.IsGenericParameter))
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            var typeArguments = candidate.SourceType.GetMergedGenericArguments(targets);
            var genericType = candidate.SourceType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
            if (genericType.ContainsGenericParameters)
            {
                genericType.TryInferTypeArguments(out var inferredTypes);
                return genericType.GetGenericTypeDefinition().MakeGenericType(inferredTypes);
            }

            return genericType;
        }
    }
}
