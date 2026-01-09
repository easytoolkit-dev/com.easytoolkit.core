using System;
using EasyToolKit.Core.Common;
using JetBrains.Annotations;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Default implementation of <see cref="IExpressionEvaluatorBuilder"/>.
    /// </summary>
    /// <remarks>
    /// This builder provides a fluent API for configuring expression evaluators
    /// with options such as expression flag requirement.
    /// </remarks>
    public sealed class ExpressionEvaluatorBuilder : IExpressionEvaluatorBuilder
    {
        private readonly string _expressionPath;
        private readonly Type _sourceType;
        private bool _requireExpressionFlag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEvaluatorBuilder"/> class.
        /// </summary>
        /// <param name="expressionPath">The expression path to evaluate.</param>
        /// <param name="sourceType">The type containing the member to evaluate.</param>
        internal ExpressionEvaluatorBuilder(string expressionPath, Type sourceType)
        {
            _expressionPath = expressionPath;
            _sourceType = sourceType;
            _requireExpressionFlag = false;
        }

        /// <summary>
        /// Configures the evaluator to require an expression flag.
        /// </summary>
        /// <returns>The builder instance for method chaining.</returns>
        /// <remarks>
        /// <para>
        /// When enabled, the expression must start with '@' to trigger evaluation.
        /// Otherwise, it's treated as a primitive string value.
        /// </para>
        /// <para>
        /// This is useful for conditional interpolation where the same attribute
        /// can accept both static strings and dynamic expressions.
        /// </para>
        /// </remarks>
        public IExpressionEvaluatorBuilder WithExpressionFlag()
        {
            _requireExpressionFlag = true;
            return this;
        }

        /// <summary>
        /// Builds the configured expression evaluator.
        /// </summary>
        /// <returns>The configured expression evaluator instance.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when using expression flag with non-string result types.
        /// </exception>
        /// <remarks>
        /// This method determines whether to create a literal or dynamic evaluator
        /// based on the builder configuration and expression path content.
        /// </remarks>
        public IExpressionEvaluator Build()
        {
            // Handle null or whitespace expressions
            if (_expressionPath.IsNullOrWhiteSpace() || _sourceType == null)
            {
                return new LiteralExpressionEvaluator(_expressionPath);
            }

            // Handle expression flag
            if (_requireExpressionFlag)
            {
                if (_expressionPath.StartsWith("@"))
                {
                    // Remove '@' prefix and create dynamic evaluator
                    var path = _expressionPath.Substring(1);
                    return new DynamicExpressionEvaluator(path, _sourceType);
                }

                // Return literal value (no '@' prefix)
                return new LiteralExpressionEvaluator(_expressionPath);
            }

            // No expression flag - always evaluate
            return new DynamicExpressionEvaluator(_expressionPath, _sourceType);
        }
    }
}
