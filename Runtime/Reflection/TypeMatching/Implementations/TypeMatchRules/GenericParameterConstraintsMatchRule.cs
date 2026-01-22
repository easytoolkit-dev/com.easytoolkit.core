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
            if (targets.Length != candidate.Constraints.Length)
            {
                return false;
            }

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
            var substitutedTypeByParameter = new Dictionary<Type, Type>();
            for (int i = 0; i < candidate.Constraints.Length; i++)
            {
                var dict = GetSubstitutedTypeByParameter(candidate.Constraints[i], targets[i]);
                if (dict == null)
                {
                    continue;
                }

                foreach (var kvp in dict)
                {
                    substitutedTypeByParameter.Add(kvp.Key, kvp.Value);
                }
            }

            typeArguments = candidate.SourceType.GetGenericArguments();
            for (var i = 0; i < typeArguments.Length; i++)
            {
                if (substitutedTypeByParameter.TryGetValue(typeArguments[i], out var substitutedType))
                {
                    typeArguments[i] = substitutedType;
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

        private Dictionary<Type, Type> GetSubstitutedTypeByParameter(Type constraint, Type targetType)
        {
            var result = new Dictionary<Type, Type>();
            if (constraint.IsGenericParameter)
            {
                if (!constraint.SatisfiesGenericParameterConstraints(targetType))
                {
                    return null;
                }
                result.Add(constraint, targetType);
                return result;
            }

            if (constraint.ContainsGenericParameters)
            {
                try
                {
                    var completedArguments = constraint.GetCompletedGenericArguments(targetType, true);
                    if (completedArguments.Length == 0)
                    {
                        return null;
                    }

                    var originalArguments = constraint.GetGenericArguments();
                    var completedArgumentsIndex = 0;
                    foreach (var originalArgument in originalArguments)
                    {
                        if (originalArgument.IsGenericParameter)
                        {
                            result.Add(originalArgument, completedArguments[completedArgumentsIndex]);
                            completedArgumentsIndex++;
                        }
                    }

                    return result;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return null;
        }
    }
}
