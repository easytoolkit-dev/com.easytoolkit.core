using System;
using System.Diagnostics;

namespace EasyToolKit.Core.Reflection
{
    [DebuggerDisplay("{SourceType}")]
    public class TypeMatchCandidate
    {
        /// <summary>
        /// Gets the type to be matched.
        /// </summary>
        /// <value>
        /// The candidate type (e.g., a handler or serializer type) that may match
        /// against the specified target types.
        /// </value>
        public Type SourceType { get; }

        /// <summary>
        /// Gets the priority of this match index (higher values have higher priority).
        /// </summary>
        /// <value>
        /// The priority value used to order match results. When multiple matches are found,
        /// results are sorted by priority in descending order.
        /// </value>
        public int Priority { get; }

        /// <summary>
        /// Gets the target types that this index should match against.
        /// </summary>
        /// <value>
        /// An array of types that define the matching criteria. For example, a generic
        /// handler might specify generic parameter types as targets.
        /// </value>
        public Type[] Constraints { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMatchCandidate"/> class.
        /// </summary>
        /// <param name="sourceType">The type to be matched.</param>
        /// <param name="priority">The priority of this match index.</param>
        /// <param name="constraints">The target types to match against.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="constraints"/> is null.
        /// </exception>
        public TypeMatchCandidate(Type sourceType, int priority, Type[] constraints)
        {
            SourceType = sourceType;
            Priority = priority;
            Constraints = constraints ?? Type.EmptyTypes;
        }
    }
}
