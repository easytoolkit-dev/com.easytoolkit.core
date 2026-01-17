using JetBrains.Annotations;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Provides factory methods for creating type matchers.
    /// </summary>
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
    }
}
