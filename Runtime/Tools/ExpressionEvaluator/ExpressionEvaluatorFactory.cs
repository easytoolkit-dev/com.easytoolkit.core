using System;
using JetBrains.Annotations;

namespace EasyToolKit.Core
{
    /// <summary>
    /// Provides factory methods for creating expression evaluators.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This static class serves as the main entry point for creating expression evaluators.
    /// It provides simplified creation methods while hiding implementation details.
    /// </para>
    /// <para>
    /// <b>Usage Examples:</b>
    /// </para>
    /// <example>
    /// <code>
    /// // Example 1: Literal expression (static string)
    /// var evaluator1 = ExpressionEvaluatorFactory.Literal("Hello World");
    /// string result1 = evaluator1.Evaluate&lt;string&gt;(null); // Returns "Hello World"
    ///
    /// // Example 2: Simple property access
    /// var evaluator2 = ExpressionEvaluatorFactory
    ///     .Evaluate("Name", typeof(Player))
    ///     .Build();
    /// string name = evaluator2.Evaluate&lt;string&gt;(player); // Returns player.Name
    ///
    /// // Example 3: Nested expression with expression flag
    /// var evaluator3 = ExpressionEvaluatorFactory
    ///     .Evaluate(labelText, targetType)
    ///     .WithExpressionFlag()
    ///     .Build();
    ///
    /// // Usage in attribute:
    /// // [Label("Static Label")]    // Returns "Static Label"
    /// // [Label("@DynamicLabel")]   // Evaluates 'DynamicLabel' property
    ///
    /// // Example 4: Method call
    /// var evaluator4 = ExpressionEvaluatorFactory
    ///     .Evaluate("Inventory.GetCount('sword')", typeof(Player))
    ///     .Build();
    /// int swordCount = evaluator4.Evaluate&lt;int&gt;(player);
    ///
    /// // Example 5: Static member access
    /// var evaluator5 = ExpressionEvaluatorFactory
    ///     .Evaluate("-t:GameConfig -p:MaxLevel", null)
    ///     .Build();
    /// int maxLevel = evaluator5.Evaluate&lt;int&gt;(null);
    /// </code>
    /// </example>
    /// </remarks>
    public static class ExpressionEvaluatorFactory
    {
        /// <summary>
        /// Creates an evaluator for a literal (non-interpolated) string value.
        /// </summary>
        /// <param name="value">The literal string value.</param>
        /// <returns>An evaluator that returns the specified value.</returns>
        /// <remarks>
        /// <para>
        /// The literal evaluator returns the value as-is without any expression parsing
        /// or evaluation. This is useful for static text that should not be interpreted.
        /// </para>
        /// <para>
        /// Null values are converted to string.Empty.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var evaluator = ExpressionEvaluatorFactory.Literal("Hello World");
        /// string result = evaluator.Evaluate&lt;string&gt;(null); // Returns "Hello World"
        /// </code>
        /// </example>
        [PublicAPI]
        public static IExpressionEvaluator Literal([CanBeNull] string value)
        {
            return new Implementations.LiteralExpressionEvaluator(value);
        }

        /// <summary>
        /// Creates a builder for evaluating an expression path.
        /// </summary>
        /// <param name="expressionPath">The expression path to evaluate.</param>
        /// <param name="sourceType">The type containing the member to evaluate.</param>
        /// <returns>A configured evaluator builder.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when <paramref name="expressionPath"/> is null.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method returns a builder that can be configured with options such
        /// as expression flag requirement before building the final evaluator.
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
        /// <example>
        /// <code>
        /// var evaluator = ExpressionEvaluatorFactory
        ///     .Evaluate("Player.Name", typeof(Player))
        ///     .Build();
        ///
        /// // Type-safe evaluation using extension method
        /// string name = evaluator.Evaluate&lt;string&gt;(player);
        /// </code>
        /// </example>
        [PublicAPI]
        public static IExpressionEvaluatorBuilder Evaluate(string expressionPath, Type sourceType)
        {
            return new Implementations.ExpressionEvaluatorBuilder(expressionPath, sourceType);
        }
    }
}
