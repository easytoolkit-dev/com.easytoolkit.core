using System;
using EasyToolKit.Core.Textual;
using JetBrains.Annotations;

namespace EasyToolKit.Core.Reflection.Implementations
{
    /// <summary>
    /// Evaluates dynamic expression paths using runtime reflection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This evaluator parses expression paths and creates compiled getter functions
    /// for efficient runtime value retrieval. It supports:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Direct member access (fields, properties, methods)</description></item>
    /// <item><description>Nested paths using dot notation</description></item>
    /// <item><description>Array and indexer access</description></item>
    /// <item><description>Static member access using <c>-t:</c> and <c>-p:</c> syntax</description></item>
    /// </list>
    /// </remarks>
    public sealed class DynamicExpressionEvaluator : ExpressionEvaluatorBase
    {
        [CanBeNull] private readonly Type _sourceType;
        private Func<object, object> _compiledGetter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicExpressionEvaluator"/> class.
        /// </summary>
        /// <param name="expressionPath">The expression path to evaluate.</param>
        /// <param name="sourceType">The type containing the member to evaluate (null for static members).</param>
        public DynamicExpressionEvaluator(string expressionPath, [CanBeNull] Type sourceType)
            : base(expressionPath)
        {
            _sourceType = sourceType;
        }

        /// <summary>
        /// Evaluates the expression against the specified context object.
        /// </summary>
        /// <param name="context">The context object to evaluate against.</param>
        /// <returns>The evaluated value as an untyped object.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the evaluator has a validation error.
        /// </exception>
        public override object Evaluate(object context)
        {
            if (TryGetError(out var error))
            {
                throw new InvalidOperationException($"Expression evaluation failed: {error}");
            }

            return _compiledGetter(context);
        }

        /// <summary>
        /// Performs validation and compiles the expression getter.
        /// </summary>
        /// <remarks>
        /// This method parses the expression path, analyzes its structure,
        /// and creates a compiled getter function for efficient evaluation.
        /// The result is cached for subsequent calls.
        /// </remarks>
        protected override void PerformValidation()
        {
            if (ExpressionPath.IsNullOrWhiteSpace())
            {
                SetError("Expression path cannot be null or empty.");
                return;
            }

            if (!TryAnalyseCode(ExpressionPath, _sourceType, out var rootType, out var path, out var error))
            {
                SetError(error);
                return;
            }

            try
            {
                _compiledGetter = CreateCompiledGetter(rootType, _sourceType, path);
                SetError(null); // Success
            }
            catch (Exception e)
            {
                // Try with method call syntax
                try
                {
                    _compiledGetter = CreateCompiledGetter(rootType, _sourceType, path + "()");
                    SetError(null); // Success
                }
                catch (Exception)
                {
                    SetError($"Failed to create value getter: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Creates a compiled getter function for the expression path.
        /// </summary>
        private Func<object, object> CreateCompiledGetter([CanBeNull] Type rootType, [CanBeNull] Type sourceType, string path)
        {
            if (rootType != null)
            {
                // Static member access
                var getter = ReflectionFactory.CreateAccessor(path).BuildStaticGetter(sourceType);
                return o => getter();
            }

            // Instance member access
            try
            {
                var getter = ReflectionFactory.CreateAccessor(path).BuildInstanceGetter(sourceType);
                return o => getter(ref o);
            }
            catch (ArgumentException e)
            {
                // Fallback to static member
                try
                {
                    var getter = ReflectionFactory.CreateAccessor(path).BuildStaticGetter(sourceType);
                    return o => getter();
                }
                catch (Exception)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Analyzes the expression path to extract type and path information.
        /// </summary>
        private static bool TryAnalyseCode(string expressionPath, [CanBeNull] Type sourceType,
            out Type rootType, out string path, out string error)
        {
            rootType = null;
            path = null;
            error = null;

            // Try to extract -t:TypeName argument
            if (TryGetArgument(expressionPath, "-t:", out var rootTypeText))
            {
                try
                {
                    rootType = TypeResolver.ResolveType(rootTypeText);
                }
                catch (Exception e)
                {
                    error = $"Failed to bind type '{rootTypeText}': {e.Message}";
                    return false;
                }
            }
            else
            {
                if (sourceType == null)
                {
                    error = "Source type cannot be null when argument '-t:' is not specified.";
                    return false;
                }
            }

            // Try to extract -p:Path argument
            if (!TryGetArgument(expressionPath, "-p:", out path))
            {
                if (rootType != null)
                {
                    error = "Path argument '-p:' is required when '-t:' is specified.";
                    return false;
                }
                else
                {
                    path = expressionPath;
                }
            }

            return true;
        }

        /// <summary>
        /// Extracts an argument value from the expression path.
        /// </summary>
        private static bool TryGetArgument(string expressionPath, string argumentPrefix, out string argumentValue)
        {
            var index = expressionPath.IndexOf(argumentPrefix, StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                index += argumentPrefix.Length;
                var rest = expressionPath.Substring(index);
                argumentValue = rest.Split(' ')[0];
                return true;
            }

            argumentValue = null;
            return false;
        }
    }

}
