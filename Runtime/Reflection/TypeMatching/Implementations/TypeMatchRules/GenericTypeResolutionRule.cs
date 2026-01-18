using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    public sealed class GenericTypeResolutionRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            if (targets.Length != 1) return false;

            var concreteType = targets[0];
            var openGenericType = candidate.Constraints[0];

            if (!openGenericType.IsGenericType || !concreteType.IsGenericType)
            {
                if (openGenericType.IsArray)
                {
                    if (!concreteType.IsArray)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            Type[] supplementaryTypeArguments;
            try
            {
                supplementaryTypeArguments = openGenericType.GetSupplementaryGenericTypeArguments(concreteType, true);
            }
            catch (Exception e)
            {
                return false;
            }

            if (supplementaryTypeArguments.Length == 0)
                return false;

            return candidate.SourceType.SatisfiesConstraints(supplementaryTypeArguments);
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            var valueType = targets[0];
            var argType = candidate.Constraints[0];

            // If the argument is not a generic parameter and is a concrete type without generic parameters,
            // the handler's target type must exactly match the value type.
            if (!argType.IsGenericParameter && !argType.ContainsGenericParameters)
            {
                return candidate.SourceType;
            }

            var supplementaryTypeArguments = argType.GetSupplementaryGenericTypeArguments(valueType, true);
            return candidate.SourceType.MakeGenericTypeExtended(supplementaryTypeArguments);
        }
    }
}
