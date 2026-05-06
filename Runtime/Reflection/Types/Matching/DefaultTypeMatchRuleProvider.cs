using System.Collections.Generic;

namespace EasyToolkit.Core.Reflection.Implementations
{
    /// <summary>
    /// Provides the default type matching rules.
    /// </summary>
    /// <remarks>
    /// This class provides all default type matching rules in the correct order.
    /// Rules are evaluated in the order returned by <see cref="GetRules"/>, so the
    /// order is significant. More specific rules should come before more general ones.
    /// </remarks>
    public sealed class DefaultTypeMatchRuleProvider
    {
        /// <summary>
        /// Gets all default type match rules in priority order.
        /// </summary>
        /// <returns>An enumerable of default match rules.</returns>
        public IEnumerable<ITypeMatchRule> GetRules()
        {
            yield return new ExactMatchRule();
            yield return new GenericParameterConstraintsMatchRule();
        }
    }
}
