using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// <item><description>Method calls with parameters (e.g., <c>Calculate(5, 3)</c>)</description></item>
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
                // Try to parse as method call with parameters
                if (TryParseMethodCall(path, out var methodName, out var arguments))
                {
                    _compiledGetter = CreateMethodInvoker(rootType, _sourceType, methodName, arguments);
                    SetError(null); // Success
                    return;
                }

                _compiledGetter = CreateCompiledGetter(rootType, _sourceType, path);
                SetError(null); // Success
            }
            catch (Exception e)
            {
                // Try with method call syntax (parameterless method)
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
                var getter = ReflectionPathFactory.BuildAccessor(path).BuildStaticGetter(rootType);
                return o => getter();
            }

            // Instance member access
            try
            {
                var getter = ReflectionPathFactory.BuildAccessor(path).BuildInstanceGetter(sourceType);
                return o => getter(o);
            }
            catch (ArgumentException e)
            {
                // Fallback to static member
                try
                {
                    var getter = ReflectionPathFactory.BuildAccessor(path).BuildStaticGetter(sourceType);
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
                rootType = TypeResolver.FindType(rootTypeText);
                if (rootType == null)
                {
                    error = $"Failed to bind type '{rootTypeText}': Invalid type name.";
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

                // Find the end of the argument value
                // It ends at the next argument prefix (starts with '-') or end of string
                var nextArgIndex = rest.IndexOf(" -", StringComparison.Ordinal);
                if (nextArgIndex >= 0)
                {
                    argumentValue = rest.Substring(0, nextArgIndex).Trim();
                }
                else
                {
                    argumentValue = rest.Trim();
                }

                return true;
            }

            argumentValue = null;
            return false;
        }

        /// <summary>
        /// Parses a method call expression to extract the method name and arguments.
        /// </summary>
        /// <param name="path">The expression path to parse.</param>
        /// <param name="methodName">The extracted method name (without parameters).</param>
        /// <param name="arguments">The parsed argument values.</param>
        /// <returns>True if the path is a method call with parameters; otherwise, false.</returns>
        private static bool TryParseMethodCall(string path, out string methodName, out object[] arguments)
        {
            methodName = null;
            arguments = null;

            // Check if this is a method call with parameters (e.g., "Calculate(5, 3)" or "Nested.Calculate(5, 3)")
            var lastDotIndex = path.LastIndexOf('.');
            var openParenIndex = path.LastIndexOf('(');
            var closeParenIndex = path.LastIndexOf(')');

            // Must have matching parentheses and at least one character inside
            if (openParenIndex < 0 || closeParenIndex < 0 || closeParenIndex <= openParenIndex + 1)
            {
                return false;
            }

            // Ensure parentheses are at the end or after the last dot
            if (lastDotIndex > openParenIndex)
            {
                return false;
            }

            // Extract method name
            methodName = path[..openParenIndex];

            // Extract and parse arguments
            var argsString = path[(openParenIndex + 1)..closeParenIndex];
            if (argsString.IsNullOrWhiteSpace())
            {
                return false;
            }

            // Split by comma and parse each argument
            var argStrings = argsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var parsedArgs = new List<object>();

            foreach (var argString in argStrings)
            {
                var trimmed = argString.Trim();
                if (trimmed.IsNullOrWhiteSpace())
                {
                    continue;
                }

                // Try to parse as integer
                if (int.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    parsedArgs.Add(intValue);
                }
                // Try to parse as float
                else if (float.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatValue))
                {
                    parsedArgs.Add(floatValue);
                }
                // Try to parse as double
                else if (double.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    parsedArgs.Add(doubleValue);
                }
                // Try to parse as boolean
                else if (bool.TryParse(trimmed, out var boolValue))
                {
                    parsedArgs.Add(boolValue);
                }
                // Handle string literals (quoted)
                else if ((trimmed.StartsWith("\"") && trimmed.EndsWith("\"")) ||
                         (trimmed.StartsWith("'") && trimmed.EndsWith("'")))
                {
                    parsedArgs.Add(trimmed[1..^1]);
                }
                else
                {
                    // Unknown argument type
                    return false;
                }
            }

            arguments = parsedArgs.ToArray();
            return true;
        }

        /// <summary>
        /// Creates a compiled getter for a method call with parameters.
        /// </summary>
        private Func<object, object> CreateMethodInvoker([CanBeNull] Type rootType, [CanBeNull] Type sourceType,
            string methodName, object[] arguments)
        {
            // Infer parameter types from argument values
            var parameterTypes = new Type[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == null)
                {
                    parameterTypes[i] = typeof(object);
                }
                else
                {
                    parameterTypes[i] = arguments[i].GetType();
                }
            }

            if (rootType != null)
            {
                // Static method call
                var invoker = ReflectionPathFactory.BuildInvoker(methodName).BuildStaticFunc(rootType, parameterTypes);
                return o => invoker(arguments);
            }

            // Instance method call
            try
            {
                var invoker = ReflectionPathFactory.BuildInvoker(methodName).BuildInstanceFunc(sourceType, parameterTypes);
                return o => invoker(o, arguments);
            }
            catch (ArgumentException e)
            {
                // Fallback to static method
                try
                {
                    var invoker = ReflectionPathFactory.BuildInvoker(methodName).BuildStaticFunc(sourceType, parameterTypes);
                    return o => invoker(arguments);
                }
                catch (Exception)
                {
                    throw e;
                }
            }
        }
    }

}
