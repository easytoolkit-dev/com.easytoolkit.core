using System;
using System.Diagnostics;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Represents the result of a type matching operation.
    /// </summary>
    /// <remarks>
    /// Type match results contain detailed information about a successful match,
    /// including the matched type, the targets that were matched, the rule that
    /// produced the match, and the index that was used.
    /// </remarks>
    [DebuggerDisplay("{MatchedType} - Rule: {MatchRule}")]
    public class TypeMatchResult
    {
        public TypeMatchCandidate Candidate { get; }

        /// <summary>
        /// Gets the actual type that was matched.
        /// </summary>
        /// <value>
        /// The concrete type that was constructed or matched. For generic types,
        /// this is the closed generic type, not the generic type definition.
        /// </value>
        public Type MatchedType { get; }

        /// <summary>
        /// Gets the target types that were matched against.
        /// </summary>
        /// <value>
        /// An array of target types that were used in the matching operation.
        /// </value>
        public Type[] Constraints { get; }

        /// <summary>
        /// Gets the match rule that produced this result.
        /// </summary>
        /// <value>
        /// The <see cref="ITypeMatchRule"/> that successfully matched
        /// the index against the targets.
        /// </value>
        public ITypeMatchRule MatchRule { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMatchResult"/> class.
        /// </summary>
        /// <param name="candidate">The match index that was used.</param>
        /// <param name="matchedType">The actual type that was matched.</param>
        /// <param name="criteria">The target types that were matched against.</param>
        /// <param name="matchRule">The match rule that produced this result.</param>
        public TypeMatchResult(TypeMatchCandidate candidate, Type matchedType, Type[] criteria,
            ITypeMatchRule matchRule)
        {
            Candidate = candidate ?? throw new ArgumentNullException(nameof(candidate));
            MatchedType = matchedType ?? throw new ArgumentNullException(nameof(matchedType));
            Constraints = criteria ?? Type.EmptyTypes;
            MatchRule = matchRule ?? throw new ArgumentNullException(nameof(matchRule));
        }
    }
}
