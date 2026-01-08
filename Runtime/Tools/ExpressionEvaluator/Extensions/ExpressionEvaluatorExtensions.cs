using System;

namespace EasyToolKit.Core
{

    /// <summary>
    /// Provides extension methods for type-safe expression evaluation.
    /// </summary>
    /// <remarks>
    /// This static class contains extension methods that provide type-safe evaluation
    /// for <see cref="IExpressionEvaluator"/> instances. Use these methods when you
    /// know the expected result type at compile time.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Create an evaluator
    /// var evaluator = ExpressionEvaluatorFactory
    ///     .Evaluate("Player.Name", typeof(Player))
    ///     .Build();
    ///
    /// // Type-safe evaluation using extension method
    /// string name = evaluator.Evaluate&lt;string&gt;(player);
    ///
    /// // Or use the non-generic method
    /// object value = evaluator.Evaluate(player);
    /// </code>
    /// </example>
    public static class ExpressionEvaluatorExtensions
    {
        /// <summary>
        /// Evaluates the expression against the specified context object with type-safe return.
        /// </summary>
        /// <typeparam name="TResult">The type of value to evaluate.</typeparam>
        /// <param name="evaluator">The expression evaluator.</param>
        /// <param name="context">The context object to evaluate against.</param>
        /// <returns>The evaluated value as the specified type.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the evaluator has a validation error.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown when the evaluated value cannot be cast to <typeparamref name="TResult"/>.
        /// </exception>
        /// <remarks>
        /// This extension method provides a type-safe way to evaluate expressions
        /// when the result type is known at compile time.
        /// </remarks>
        public static TResult Evaluate<TResult>(this IExpressionEvaluator evaluator, object context)
        {
            object result = evaluator.Evaluate(context);
            if (result == null && default(TResult) == null)
            {
                return default;
            }

            return (TResult)result;
        }
    }
}
