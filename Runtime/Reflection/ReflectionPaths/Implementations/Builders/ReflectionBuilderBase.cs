using System;
using System.Collections;
using System.Reflection;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Base class for builders that operate on member paths.
    /// Provides common functionality for parsing and navigating member paths.
    /// </summary>
    public abstract class ReflectionBuilderBase : IReflectionBuilder
    {
        private readonly string _memberPath;

        /// <summary>
        /// Initializes a new instance of the ReflectionBuilderBase.
        /// </summary>
        /// <param name="memberPath">The path to the member (e.g., "Field", "Property", "Nested.Method()").</param>
        protected ReflectionBuilderBase(string memberPath)
        {
            _memberPath = memberPath ?? throw new ArgumentNullException(nameof(memberPath));
        }

        /// <summary>
        /// Gets the member path this builder operates on.
        /// </summary>
        public string MemberPath => _memberPath;
    }
}
