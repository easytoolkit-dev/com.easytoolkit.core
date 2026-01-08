using System;

namespace EasyToolKit.Core.Implementations
{
    /// <summary>
    /// Provides a base implementation for expression evaluators.
    /// </summary>
    /// <remarks>
    /// This abstract class implements common functionality for all expression evaluators,
    /// including error validation and deferred evaluation pattern. Subclasses must
    /// implement the actual evaluation logic.
    /// </remarks>
    public abstract class ExpressionEvaluatorBase : IExpressionEvaluator
    {
        /// <summary>
        /// The expression path to evaluate.
        /// </summary>
        protected readonly string ExpressionPath;

        private string _errorMessage;
        private bool _isValidated;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEvaluatorBase"/> class.
        /// </summary>
        /// <param name="expressionPath">The expression path to evaluate.</param>
        protected ExpressionEvaluatorBase(string expressionPath)
        {
            ExpressionPath = expressionPath;
        }

        /// <summary>
        /// Gets whether this evaluator has a validation error.
        /// </summary>
        /// <param name="errorMessage">The error message, if any.</param>
        /// <returns>
        /// <c>true</c> if there is a validation error; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method uses deferred validation - the actual validation is performed
        /// on the first call. Subsequent calls return the cached result.
        /// </remarks>
        public virtual bool TryGetError(out string errorMessage)
        {
            if (!_isValidated)
            {
                PerformValidation();
                _isValidated = true;
            }

            errorMessage = _errorMessage;
            return _errorMessage != null;
        }

        /// <summary>
        /// Evaluates the expression against the specified context object.
        /// </summary>
        /// <param name="context">The context object to evaluate against.</param>
        /// <returns>The evaluated value as an untyped object.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the evaluator has a validation error.
        /// </exception>
        public abstract object Evaluate(object context);

        /// <summary>
        /// Performs validation logic for the expression.
        /// </summary>
        /// <remarks>
        /// Subclasses can override this method to provide custom validation logic.
        /// The default implementation performs no validation.
        /// </remarks>
        protected virtual void PerformValidation()
        {
            // Default implementation: no validation
            _errorMessage = null;
        }

        /// <summary>
        /// Sets the validation error message.
        /// </summary>
        /// <param name="errorMessage">The error message to set.</param>
        protected void SetError(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Checks whether the evaluator has been validated.
        /// </summary>
        /// <returns>
        /// <c>true</c> if validation has been performed; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsValidated() => _isValidated;
    }

}
