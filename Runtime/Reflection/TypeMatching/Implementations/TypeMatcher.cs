using System;
using System.Collections.Generic;
using System.Linq;
using EasyToolKit.Core;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides a flexible type matching system that can match types against target types using configurable rules.
    /// Supports exact matching, generic type matching, type inference, and custom matching rules.
    /// </summary>
    public sealed class TypeMatcher : ITypeMatcher
    {
        /// <summary>
        /// The collection of registered match rules.
        /// </summary>
        private readonly List<TypeMatchRule> _matchRules = new List<TypeMatchRule>();

        /// <summary>
        /// The collection of registered match indices.
        /// </summary>
        private List<TypeMatchIndex> _matchIndices;

        /// <summary>
        /// The cache for match results, keyed by target type hash code.
        /// </summary>
        private readonly Dictionary<int, TypeMatchResult[]> _matchResultsCache =
            new Dictionary<int, TypeMatchResult[]>();

        /// <summary>
        /// The cache for merged results, keyed by the hash of the input results list.
        /// </summary>
        private readonly Dictionary<int, TypeMatchResult[]> _mergedResultsCache =
            new Dictionary<int, TypeMatchResult[]>();

        /// <summary>
        /// Adds type match indices to the current collection.
        /// </summary>
        /// <param name="matchIndices">The type match indices to add.</param>
        /// <remarks>
        /// This method appends the specified indices to the existing collection.
        /// The cache is automatically cleared after this operation.
        /// </remarks>
        public void AddTypeMatchIndices(IEnumerable<TypeMatchIndex> matchIndices)
        {
            _matchIndices.AddRange(matchIndices);
            ClearCache();
        }

        /// <summary>
        /// Replaces the current type match indices with the specified collection.
        /// </summary>
        /// <param name="matchIndices">The new type match indices to use.</param>
        /// <remarks>
        /// This method replaces all existing indices with the specified collection.
        /// The cache is automatically cleared after this operation.
        /// </remarks>
        public void SetTypeMatchIndices(IEnumerable<TypeMatchIndex> matchIndices)
        {
            _matchIndices = matchIndices.ToList();
            ClearCache();
        }

        /// <summary>
        /// Adds a custom match rule to the type matcher.
        /// </summary>
        /// <param name="rule">The match rule to add.</param>
        /// <remarks>
        /// Rules are evaluated in the order they were added. The cache is automatically
        /// cleared after this operation.
        /// </remarks>
        public void AddMatchRule(TypeMatchRule rule)
        {
            _matchRules.Add(rule);
            ClearCache();
        }

        /// <summary>
        /// Removes a match rule from the type matcher.
        /// </summary>
        /// <param name="rule">The match rule to remove.</param>
        /// <remarks>
        /// The cache is automatically cleared after this operation.
        /// </remarks>
        public void RemoveMatchRule(TypeMatchRule rule)
        {
            _matchRules.Remove(rule);
            ClearCache();
        }

        /// <summary>
        /// Gets type matches for the specified target types.
        /// Results are cached based on the target types to improve performance.
        /// </summary>
        /// <param name="targets">The target types to match against.</param>
        /// <returns>An array of type match results, ordered by priority (highest first).</returns>
        /// <remarks>
        /// This method evaluates all registered match indices against the specified
        /// target types using the registered match rules. Results are cached for
        /// performance, so subsequent calls with the same target types will return
        /// cached results.
        /// </remarks>
        public TypeMatchResult[] GetMatches(params Type[] targets)
        {
            var hash = new HashCode();
            foreach (var target in targets)
            {
                hash.Add(target);
            }
            var hashCode = hash.ToHashCode();

            if (_matchResultsCache.TryGetValue(hashCode, out var ret))
            {
                return ret;
            }

            var results = new List<TypeMatchResult>();

            foreach (var index in _matchIndices)
            {
                if (index.Targets.Length != targets.Length)
                    continue;

                foreach (var rule in _matchRules)
                {
                    bool stop = false;
                    var match = rule(index, targets, ref stop);
                    if (match != null)
                        results.Add(new TypeMatchResult(index, match, targets, rule));

                    if (stop)
                        break;
                }
            }

            ret = results
                .OrderByDescending(result => result.MatchIndex.Priority)
                .ToArray();
            _matchResultsCache[hashCode] = ret;
            return ret;
        }

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
        public TypeMatchResult[] GetMergedResults(IReadOnlyList<TypeMatchResult[]> resultsList)
        {
            if (resultsList.Count == 0)
            {
                return Array.Empty<TypeMatchResult>();
            }

            if (resultsList.Count == 1)
            {
                return resultsList[0];
            }

            var hash = new HashCode();
            foreach (var value in resultsList)
            {
                hash.Add(value);
            }
            var hashCode = hash.ToHashCode();

            //TODO hashCode not good
            if (_mergedResultsCache.TryGetValue(hashCode, out var ret))
            {
                return ret;
            }

            ret = resultsList
                .SelectMany(x => x)
                .OrderByDescending(result => result.MatchIndex.Priority)
                .Distinct()
                .ToArray();
            _mergedResultsCache[hashCode] = ret;
            return ret;
        }

        private void ClearCache()
        {
            _matchResultsCache.Clear();
            _mergedResultsCache.Clear();
        }
    }
}
