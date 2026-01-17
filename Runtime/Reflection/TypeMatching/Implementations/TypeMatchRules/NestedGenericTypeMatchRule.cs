using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that matches nested types within the same generic type definition.
    /// </summary>
    /// <remarks>
    /// This rule handles cases where nested types are declared within generic types.
    /// It matches when the declaring types of both the match index and the target have
    /// the same generic type definition. This is useful for matching handlers or serializers
    /// for nested types like <c>MyGenericClass&lt;T&gt;.NestedClass</c>.
    /// </remarks>
    public sealed class NestedGenericTypeMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            if (targets.Length != 1) return false;

            var target = targets[0];

            if (!candidate.SourceType.IsNested || !candidate.Constraints[0].IsNested || !target.IsNested) return false;

            if (!candidate.SourceType.DeclaringType.IsGenericType ||
                !candidate.Constraints[0].DeclaringType.IsGenericType ||
                !target.DeclaringType.IsGenericType)
            {
                return false;
            }

            if (candidate.SourceType.DeclaringType.GetGenericTypeDefinition() != candidate.Constraints[0].DeclaringType.GetGenericTypeDefinition() ||
                candidate.SourceType.DeclaringType.GetGenericTypeDefinition() != target.DeclaringType.GetGenericTypeDefinition()) return false;

            var args = target.GetGenericArguments();

            return candidate.SourceType.SatisfiesConstraints(args);
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            var args = targets[0].GetGenericArguments();
            return candidate.SourceType.MakeGenericType(args);
        }
    }
}
