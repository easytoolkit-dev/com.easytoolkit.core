using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that matches generic type definitions against single generic target types.
    /// </summary>
    /// <remarks>
    /// This rule handles cases where a generic type definition should match against a single
    /// target type that has the same generic type definition. This is useful for matching
    /// handlers or serializers that operate on generic types with the same structure.
    /// </remarks>
    public sealed class GenericSingleTargetMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            if (!candidate.SourceType.IsGenericTypeDefinition) return false;
            if (targets.Length != 1) return false;
            if (!candidate.Constraints[0].IsGenericType || !targets[0].IsGenericType) return false;
            if (candidate.Constraints[0].GetGenericTypeDefinition() != targets[0].GetGenericTypeDefinition()) return false;

            var matchArgs = candidate.SourceType.GetGenericArguments();
            var matchTargetArgs = candidate.Constraints[0].GetGenericArguments();
            var targetArgs = targets[0].GetGenericArguments();

            if (matchArgs.Length != matchTargetArgs.Length || matchArgs.Length != targetArgs.Length) return false;

            if (!candidate.SourceType.SatisfiesConstraints(targetArgs)) return false;

            return true;
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            var targetArgs = targets[0].GetGenericArguments();
            return candidate.SourceType.MakeGenericType(targetArgs);
        }
    }
}
