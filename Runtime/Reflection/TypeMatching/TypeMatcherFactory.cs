using JetBrains.Annotations;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Provides factory methods for creating type matchers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This static class serves as the main entry point for creating type matcher instances.
    /// It provides simplified creation methods while hiding implementation details.
    /// </para>
    /// <para>
    /// <b>Usage Examples:</b>
    /// </para>
    /// <example>
    /// <code>
    /// // Example 1: Create with default rules
    /// var matcher1 = TypeMatcherFactory.CreateDefault();
    /// matcher1.SetTypeMatchIndices(indices);
    ///
    /// // Example 2: Create without rules (add custom rules)
    /// var matcher2 = TypeMatcherFactory.CreateEmpty();
    /// matcher2.AddMatchRule(MyCustomRule);
    ///
    /// // Example 3: Create with custom rules
    /// var matcher3 = TypeMatcherFactory.CreateWithRules(
    ///     MyRule1,
    ///     MyRule2
    /// );
    /// </code>
    /// </example>
    /// </remarks>
    public static class TypeMatcherFactory
    {
        /// <summary>
        /// Creates a type matcher with default match rules.
        /// </summary>
        /// <returns>A configured type matcher instance.</returns>
        public static ITypeMatcher CreateDefault()
        {
            var typeMatcher = new Implementations.TypeMatcher();
            var provider = new Implementations.DefaultTypeMatchRuleProvider();
            foreach (var rule in provider.GetRules())
            {
                typeMatcher.AddMatchRule(rule);
            }

            return typeMatcher;
        }

        /// <summary>
        /// Creates a type matcher without default match rules.
        /// </summary>
        /// <returns>An empty type matcher instance.</returns>
        /// <remarks>
        /// Use this method when you want to add custom rules without the default rules.
        /// </remarks>
        public static ITypeMatcher CreateEmpty()
        {
            return new Implementations.TypeMatcher();
        }
    }
}
