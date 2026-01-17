using System;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a type matching rule that validates generic parameter constraints.
    /// </summary>
    /// <remarks>
    /// This rule handles cases where all target types are generic parameters and
    /// must satisfy the generic constraints of the match index type. This is
    /// useful for validating that generic type arguments meet the required
    /// constraints (class, struct, new(), base class, interface, etc.).
    /// </remarks>
    public sealed class GenericConstraintsMatchRule : TypeMatchRuleBase
    {
        /// <inheritdoc/>
        public override bool CanMatch(TypeMatchCandidate candidate, Type[] targets)
        {
            for (int i = 0; i < candidate.Constraints.Length; i++)
            {
                if (!candidate.Constraints[i].IsGenericParameter) return false;
            }

            return candidate.SourceType.IsGenericType && candidate.SourceType.AreGenericConstraintsSatisfiedBy(targets);
        }

        /// <inheritdoc/>
        public override Type Match(TypeMatchCandidate candidate, Type[] targets)
        {
            return candidate.SourceType.MakeGenericType(targets);
        }
    }
}
