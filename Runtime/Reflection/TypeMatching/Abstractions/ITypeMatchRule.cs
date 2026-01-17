using System;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Defines a contract for type matching rules that determine if a type matches against target types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Type match rules are used to implement custom matching logic. Each rule provides
    /// two methods: <see cref="CanMatch"/> for checking if a match is possible, and
    /// <see cref="Match"/> for performing the actual matching and returning the matched type.
    /// </para>
    /// <para>
    /// The <see cref="CanMatch"/> method separates the matching condition logic from the
    /// result construction logic in <see cref="Match"/>.
    /// </para>
    /// </remarks>
    public interface ITypeMatchRule
    {
        /// <summary>
        /// Gets the name of this rule for debugging purposes.
        /// </summary>
        /// <value>
        /// A human-readable name that identifies this rule. This name is used in
        /// logging and debugging output to help identify which rule produced a match.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Determines whether the specified match candidate can match against the target types.
        /// </summary>
        /// <param name="candidate">The type match candidate to evaluate.</param>
        /// <param name="targets">The target types to match against.</param>
        /// <returns>
        /// <c>true</c> if this rule can match the candidate against the targets; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method should contain only the conditional logic that determines whether
        /// a match is possible, without performing the actual type construction.
        /// </remarks>
        bool CanMatch(TypeMatchCandidate candidate, Type[] targets);

        /// <summary>
        /// Performs the matching operation and returns the matched type.
        /// </summary>
        /// <param name="candidate">The type match candidate to evaluate.</param>
        /// <param name="targets">The target types to match against.</param>
        /// <returns>
        /// The matched type.
        /// </returns>
        /// <remarks>
        /// This method is called only when <see cref="CanMatch"/> returns <c>true</c>.
        /// Implementations can assume the preconditions for matching have been satisfied.
        /// </remarks>
        Type Match(TypeMatchCandidate candidate, Type[] targets);
    }
}
