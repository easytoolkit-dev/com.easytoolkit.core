using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolKit.Core.Reflection
{
    /// <summary>
    /// Represents comprehensive information about a generic type parameter, including its constraints and dependencies.
    /// </summary>
    public class GenericParameterInfo
    {
        /// <summary>
        /// Gets the name of the generic parameter (e.g., "T1", "TKey").
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the zero-based position of the generic parameter in the parameter list.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Gets the special constraints applied to this generic parameter.
        /// </summary>
        public GenericParameterSpecialConstraints SpecialConstraints { get; }

        /// <summary>
        /// Gets the type constraints (base class and interface constraints) applied to this generic parameter.
        /// </summary>
        public IReadOnlyList<Type> TypeConstraints { get; }

        /// <summary>
        /// Gets the generic parameter types that this parameter depends on.
        /// For example, if T1 has constraint List&lt;T2&gt;, then T1 depends on T2.
        /// </summary>
        public IReadOnlyList<Type> DependsOnParameters { get; }

        /// <summary>
        /// Gets the generic parameter types that depend on this parameter.
        /// For example, if T1 has constraint List&lt;T2&gt;, then T2 is depended on by T1.
        /// </summary>
        public IReadOnlyList<Type> DependedOnByParameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericParameterInfo"/> class.
        /// </summary>
        /// <param name="name">The name of the generic parameter.</param>
        /// <param name="position">The zero-based position in the parameter list.</param>
        /// <param name="specialConstraints">The special constraints.</param>
        /// <param name="typeConstraints">The type constraints.</param>
        /// <param name="dependsOnParameters">The parameter types this parameter depends on.</param>
        /// <param name="dependedOnByParameters">The parameter types that depend on this parameter.</param>
        public GenericParameterInfo(
            string name,
            int position,
            GenericParameterSpecialConstraints specialConstraints,
            IReadOnlyList<Type> typeConstraints,
            IReadOnlyList<Type> dependsOnParameters,
            IReadOnlyList<Type> dependedOnByParameters)
        {
            Name = name;
            Position = position;
            SpecialConstraints = specialConstraints;
            TypeConstraints = typeConstraints;
            DependsOnParameters = dependsOnParameters;
            DependedOnByParameters = dependedOnByParameters;
        }

        /// <summary>
        /// Determines whether this parameter has any constraints.
        /// </summary>
        public bool HasConstraints => SpecialConstraints != GenericParameterSpecialConstraints.None || TypeConstraints.Count > 0;

        /// <summary>
        /// Determines whether this parameter depends on any other parameters.
        /// </summary>
        public bool HasDependencies => DependsOnParameters.Count > 0;

        /// <summary>
        /// Determines whether this parameter is depended on by any other parameters.
        /// </summary>
        public bool IsDependencyForOthers => DependedOnByParameters.Count > 0;

        /// <summary>
        /// Returns a string representation of this parameter info.
        /// </summary>
        public override string ToString()
        {
            var parts = new List<string> { Name };

            if (SpecialConstraints != GenericParameterSpecialConstraints.None)
            {
                parts.Add($"({SpecialConstraints})");
            }

            if (TypeConstraints.Count > 0)
            {
                var constraintNames = TypeConstraints.Select(c => c.Name);
                parts.Add($"[{string.Join(", ", constraintNames)}]");
            }

            if (HasDependencies)
            {
                var dependencyNames = DependsOnParameters.Select(p => p.Name);
                parts.Add($"depends on: {string.Join(", ", dependencyNames)}");
            }

            return string.Join(" ", parts);
        }
    }
}
