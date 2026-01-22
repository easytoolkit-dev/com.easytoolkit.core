using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Defines a type matcher that finds matching types based on configurable rules.
    /// </summary>
    /// <remarks>
    /// Type matchers are used to find appropriate handler or serializer types for
    /// given target types. They support exact matching, generic type matching, type
    /// inference, and custom matching rules through a flexible rule-based system.
    /// </remarks>
    public interface ITypeMatcher
    {
        IReadOnlyList<TypeMatchCandidate> TypeMatchCandidates { get; }

        /// <summary>
        /// Adds type match candidates to the current collection.
        /// </summary>
        /// <param name="matchCandidates">The type match candidates to add.</param>
        /// <remarks>
        /// This method appends the specified candidates to the existing collection.
        /// The cache is automatically cleared after this operation.
        /// </remarks>
        void AddTypeMatchCandidates(IEnumerable<TypeMatchCandidate> matchCandidates);

        /// <summary>
        /// Replaces the current type match candidates with the specified collection.
        /// </summary>
        /// <param name="matchCandidates">The new type match candidates to use.</param>
        /// <remarks>
        /// This method replaces all existing candidates with the specified collection.
        /// The cache is automatically cleared after this operation.
        /// </remarks>
        void SetTypeMatchCandidates(IEnumerable<TypeMatchCandidate> matchCandidates);

        /// <summary>
        /// Adds a custom match rule to the type matcher.
        /// </summary>
        /// <param name="rule">The match rule to add.</param>
        /// <remarks>
        /// Rules are evaluated in the order they were added. The cache is automatically
        /// cleared after this operation.
        /// </remarks>
        void AddMatchRule(ITypeMatchRule rule);

        /// <summary>
        /// Removes a match rule from the type matcher.
        /// </summary>
        /// <param name="rule">The match rule to remove.</param>
        /// <remarks>
        /// The cache is automatically cleared after this operation.
        /// </remarks>
        void RemoveMatchRule(ITypeMatchRule rule);

        /// <summary>
        /// Gets type matches for the specified target types.
        /// Results are cached based on the target types to improve performance.
        /// </summary>
        /// <param name="targetTypes">The target types to match against.</param>
        /// <returns>An array of type match results, ordered by priority (highest first).</returns>
        /// <remarks>
        /// This method evaluates all registered match candidates against the specified
        /// target types using the registered match rules. Results are cached for
        /// performance, so subsequent calls with the same target types will return
        /// cached results.
        /// </remarks>
        TypeMatchResult[] GetMatches(params Type[] targetTypes);

        /// <summary>
        /// Gets merged results from multiple type match result arrays.
        /// Results are merged and cached based on the input arrays to improve performance.
        /// </summary>
        /// <param name="resultsList">The list of type match result arrays to merge.</param>
        /// <returns>A merged array of type match results, ordered by priority (highest first).</returns>
        /// <remarks>
        /// This method merges multiple result arrays into a single array, sorted by priority.
        /// The results are cached based on the hash of the input array list, so subsequent
        /// calls with the same inputs will return cached results. The cache is specific to
        /// this type matcher instance.
        /// </remarks>
        TypeMatchResult[] GetMergedResults(IReadOnlyList<TypeMatchResult[]> resultsList);
    }
}
