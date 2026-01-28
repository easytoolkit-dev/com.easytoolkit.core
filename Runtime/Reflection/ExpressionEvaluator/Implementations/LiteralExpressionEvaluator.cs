using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection.Implementations
{
    /// <summary>
    /// Evaluates literal (non-interpolated) values.
    /// </summary>
    /// <remarks>
    /// This evaluator returns the value as-is without any expression parsing
    /// or evaluation. It's used for static text that should not be interpreted as
    /// an expression path.
    /// </remarks>
    public sealed class LiteralExpressionEvaluator : ExpressionEvaluatorBase
    {
        private readonly string _literalValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="value">The literal value to return.</param>
        public LiteralExpressionEvaluator(string value)
            : base(value)
        {
            _literalValue = value;
        }

        /// <summary>
        /// Evaluates the literal value (returns the stored value).
        /// </summary>
        /// <param name="context">The context object (ignored for literal evaluators).</param>
        /// <returns>The literal value.</returns>
        /// <remarks>
        /// This method ignores the context parameter and always returns the
        /// literal value provided during construction.
        /// </remarks>
        public override object Evaluate(object context)
        {
            return _literalValue;
        }

        /// <summary>
        /// Performs validation (literal expressions are always valid).
        /// </summary>
        /// <remarks>
        /// Literal expressions have no validation requirements - they are always valid.
        /// </remarks>
        protected override void PerformValidation()
        {
            SetError(null);
            base.PerformValidation();
        }

        /// <summary>
        /// Gets whether this evaluator has a validation error.
        /// </summary>
        /// <param name="errorMessage">Always set to <c>null</c>.</param>
        /// <returns>Always <c>false</c> (literal expressions are always valid).</returns>
        public override bool TryGetError(out string errorMessage)
        {
            errorMessage = null;
            return false;
        }
    }
}
