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
            if (!candidate.Constraints[0].IsGenericTypeDefinition) return false;

            var valueType = targets[0];
            var argType = candidate.Constraints[0];

            // If the argument is not a generic parameter and is a concrete type without generic parameters,
            // the handler's target type must exactly match the value type.
            if (!argType.IsGenericParameter && !argType.ContainsGenericParameters)
            {
                return argType == valueType;
            }

            var missingArgs = argType.ExtractGenericArgumentsFrom(valueType, true);
            if (missingArgs.Length == 0)
                return false;

            return candidate.SourceType.AreGenericConstraintsSatisfiedBy(missingArgs);
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

            var missingArgs = argType.ExtractGenericArgumentsFrom(valueType, true);
            return candidate.SourceType.MakeGenericType(missingArgs);
        }
    }
}
