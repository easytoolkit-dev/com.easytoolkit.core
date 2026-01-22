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
        private readonly List<ITypeMatchRule> _matchRules = new List<ITypeMatchRule>();

        private List<TypeMatchCandidate> _typeMatchCandidates;

        private readonly Dictionary<string, TypeMatchResult[]> _matchResultsCache =
            new Dictionary<string, TypeMatchResult[]>();

        private readonly Dictionary<string, TypeMatchResult[]> _mergedResultsCache =
            new Dictionary<string, TypeMatchResult[]>();

        public IReadOnlyList<TypeMatchCandidate> TypeMatchCandidates => _typeMatchCandidates;

        public void AddTypeMatchCandidates(IEnumerable<TypeMatchCandidate> matchCandidates)
        {
            _typeMatchCandidates.AddRange(matchCandidates);
            ClearCache();
        }

        public void SetTypeMatchCandidates(IEnumerable<TypeMatchCandidate> matchCandidates)
        {
            _typeMatchCandidates = matchCandidates.ToList();
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
        public void AddMatchRule(ITypeMatchRule rule)
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
        public void RemoveMatchRule(ITypeMatchRule rule)
        {
            _matchRules.Remove(rule);
            ClearCache();
        }

        /// <summary>
        /// Gets type matches for the specified target types.
        /// Results are cached based on the target types to improve performance.
        /// </summary>
        /// <param name="targetTypes">The target types to match against.</param>
        /// <returns>An array of type match results, ordered by priority (highest first).</returns>
        /// <remarks>
        /// This method evaluates all registered match indices against the specified
        /// target types using the registered match rules. Results are cached for
        /// performance, so subsequent calls with the same target types will return
        /// cached results.
        /// </remarks>
        public TypeMatchResult[] GetMatches(params Type[] targetTypes)
        {
            var key = TypeMatchCaches.ComputeKey(targetTypes);
            if (_matchResultsCache.TryGetValue(key, out var final))
            {
                return final;
            }

            var results = new List<TypeMatchResult>();

            foreach (var index in _typeMatchCandidates)
            {
                if (index.Constraints.Length != targetTypes.Length)
                    continue;

                var validRules = _matchRules
                    .Where(rule => rule.CanMatch(index, targetTypes))
                    .ToArray();
                foreach (var rule in validRules)
                {
                    var match = rule.Match(index, targetTypes);
                    results.Add(new TypeMatchResult(index, match, targetTypes, rule));
                }
            }

            final = results
                .OrderByDescending(result => result.Candidate.Priority)
                .ToArray();
            // _matchResultsCache[key] = final;
            return final;
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

            var key = TypeMatchCaches.ComputeKey(resultsList);
            if (_mergedResultsCache.TryGetValue(key, out var final))
            {
                return final;
            }

            final = resultsList
                .SelectMany(x => x)
                .OrderByDescending(result => result.Candidate.Priority)
                .Distinct()
                .ToArray();
            // _mergedResultsCache[key] = final;
            return final;
        }

        private void ClearCache()
        {
            _matchResultsCache.Clear();
            _mergedResultsCache.Clear();
        }
    }
}
