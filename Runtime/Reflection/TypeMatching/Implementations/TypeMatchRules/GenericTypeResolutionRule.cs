using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that resolves generic type arguments for partially open generic types.
    /// </summary>
    /// <remarks>
    /// This rule handles complex scenarios where a handler/serializer's generic parameters correspond to
    /// specific type parameters within a partially concrete generic target type. It enables automatic
    /// instantiation of handlers with the correct type arguments based on the actual runtime type.
    /// </remarks>
    public sealed class GenericTypeResolutionRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            if (targets.Length != 1) return false;

            var concreteType = targets[0];
            var genericPatternType = candidate.Constraints[0];

            if (!genericPatternType.IsGenericParameter && !genericPatternType.ContainsGenericParameters)
            {
                return genericPatternType == concreteType;
            }

            var missingArgs = genericPatternType.ExtractGenericTypeArguments(concreteType, true);
            if (missingArgs.Length == 0)
                return false;

            return candidate.SourceType.SatisfiesConstraints(missingArgs);
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

            var missingArgs = argType.ExtractGenericTypeArguments(valueType, true);
            return candidate.SourceType.MakeGenericTypeExtended(missingArgs);
        }
    }
}
