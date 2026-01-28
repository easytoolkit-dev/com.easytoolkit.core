using System;
using EasyToolkit.Core.Textual;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Provides factory methods for creating expression evaluators.
    /// </summary>
    public static class ExpressionEvaluatorFactory
    {
        /// <summary>
        /// Creates an expression evaluator with the specified configuration.
        /// </summary>
        /// <param name="expressionPath">The expression path to evaluate.</param>
        /// <param name="sourceType">The type containing the member to evaluate.</param>
        /// <param name="requireExpressionFlag">
        /// Whether the expression must start with '@' to trigger evaluation.
        /// When true, expressions without '@' are treated as literal strings.
        /// </param>
        /// <returns>The configured expression evaluator instance.</returns>
        /// <remarks>
        /// <para>
        /// This method creates an expression evaluator based on the provided configuration.
        /// When <paramref name="requireExpressionFlag"/> is true, the expression must
        /// start with '@' to be evaluated as a dynamic expression; otherwise it's treated
        /// as a literal string value.
        /// </para>
        /// <para>
        /// The expression path syntax supports:
        /// <list type="bullet">
        /// <item><description>Direct member access: "PropertyName"</description></item>
        /// <item><description>Nested paths: "Parent.Child.Value"</description></item>
        /// <item><description>Method calls: "GetMethod()"</description></item>
        /// <item><description>Static members: "-t:TypeName -p:StaticProperty"</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="expressionPath"/> is null or whitespace while
        /// <paramref name="requireExpressionFlag"/> is true. Provide a valid expression path
        /// or a literal string value.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the expression path is invalid and cannot be parsed.
        /// This may occur when the path references non-existent members or has incorrect syntax.
        /// </exception>
        /// <example>
        /// <code>
        /// // Example 1: Always evaluate (no flag)
        /// var evaluator1 = ExpressionEvaluatorFactory.CreateEvaluator(
        ///     "Name", typeof(Player), requireExpressionFlag: false);
        /// string name = evaluator1.Evaluate&lt;string&gt;(player);
        ///
        /// // Example 2: With expression flag
        /// var evaluator2 = ExpressionEvaluatorFactory.CreateEvaluator(
        ///     "@Player.Name", typeof(Player), requireExpressionFlag: true);
        /// // Returns player.Name
        ///
        /// var evaluator3 = ExpressionEvaluatorFactory.CreateEvaluator(
        ///     "Static Text", typeof(Player), requireExpressionFlag: true);
        /// // Returns "Static Text" (no '@' prefix, treated as literal)
        /// </code>
        /// </example>
        [PublicAPI]
        public static IExpressionEvaluator CreateEvaluator(string expressionPath, Type sourceType, bool requireExpressionFlag = false)
        {
            // Handle null or whitespace expressions
            if (expressionPath.IsNullOrWhiteSpace())
            {
                if (requireExpressionFlag)
                {
                    throw new ArgumentException(
                        "Expression path cannot be null or whitespace when expression flag is required. " +
                        "Provide a valid expression path starting with '@' for dynamic evaluation, " +
                        "or set requireExpressionFlag to false to treat null/whitespace as a literal value.",
                        nameof(expressionPath));
                }
                return new Implementations.LiteralExpressionEvaluator(expressionPath);
            }

            // Handle expression flag
            if (requireExpressionFlag)
            {
                if (expressionPath.StartsWith("@"))
                {
                    // Remove '@' prefix and create dynamic evaluator
                    var path = expressionPath[1..];
                    return new Implementations.DynamicExpressionEvaluator(path, sourceType);
                }

                // Return literal value (no '@' prefix)
                return new Implementations.LiteralExpressionEvaluator(expressionPath);
            }

            // No expression flag - always evaluate
            return new Implementations.DynamicExpressionEvaluator(expressionPath, sourceType);
        }
    }
}
