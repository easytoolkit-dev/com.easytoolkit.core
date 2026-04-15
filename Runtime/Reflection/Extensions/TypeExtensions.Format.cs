using System;
using System.Collections.Generic;
using EasyToolkit.Core.Textual;

namespace EasyToolkit.Core.Reflection
{
    /// <summary>
    /// Defines formatting options for converting type names to code strings.
    /// </summary>
    [Flags]
    public enum TypeFormat
    {
        /// <summary>
        /// No special formatting applied. Uses simple type names without aliases or namespaces.
        /// </summary>
        None = 0,

        /// <summary>
        /// Use C# type aliases instead of full type names (e.g., "int" instead of "System.Int32").
        /// </summary>
        UseTypeAliases = 1 << 0,

        /// <summary>
        /// Include full namespace in type name (e.g., "System.Collections.Generic.List&lt;T&gt;" instead of "List&lt;T&gt;").
        /// </summary>
        IncludeNamespace = 1 << 1,

        Default = UseTypeAliases,

        Full = UseTypeAliases | IncludeNamespace
    }

    public static partial class TypeExtensions
    {
        private static readonly Dictionary<Type, string> TypeAliasesByType = new Dictionary<Type, string>
        {
            { typeof(void), "void" },
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(object), "object" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(string), "string" }
        };

        /// <summary>
        /// Gets the friendly name alias for a type using default formatting (with type aliases, without namespaces).
        /// </summary>
        /// <param name="type">The type to get the alias for.</param>
        /// <returns>The friendly type name (e.g., "int", "string", "List&lt;T&gt;").</returns>
        public static string ToCodeString(this Type type)
        {
            return ToCodeString(type, TypeFormat.Default);
        }

        /// <summary>
        /// Converts a type to its code representation with specified formatting options.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A string representation of the type based on the specified format.</returns>
        /// <remarks>
        /// <para>When <see cref="TypeFormat.UseTypeAliases"/> is set, primitive types use C# aliases (int, string, etc.).</para>
        /// <para>When <see cref="TypeFormat.IncludeNamespace"/> is set, the full namespace path is included.</para>
        /// <para>Example results for <see cref="List{T}"/> with <c>int</c> as type argument:</para>
        /// <list type="bullet">
        /// <item><description><c>None</c>: "List&lt;Int32&gt;"</description></item>
        /// <item><description><c>UseTypeAliases</c>: "List&lt;int&gt;"</description></item>
        /// <item><description><c>IncludeNamespace</c>: "System.Collections.Generic.List&lt;System.Int32&gt;"</description></item>
        /// <item><description><c>Full</c>: "System.Collections.Generic.List&lt;int&gt;"</description></item>
        /// </list>
        /// </remarks>
        public static string ToCodeString(this Type type, TypeFormat format)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var useAliases = (format & TypeFormat.UseTypeAliases) != 0;
            var includeNamespace = (format & TypeFormat.IncludeNamespace) != 0;

            // Handle type aliases if requested
            if (useAliases && TypeAliasesByType.TryGetValue(type, out string alias))
            {
                return alias;
            }

            // Handle generic type parameters (T, TKey, etc. from open generics)
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            // Handle generic types
            if (type.IsGenericType)
            {
                return GetGenericTypeName(type, format);
            }

            // Handle array types
            if (type.IsArray && type.GetElementType() != null)
            {
                var elementType = type.GetElementType().ToCodeString(format);
                var rank = type.GetArrayRank();
                return rank == 1 ? $"{elementType}[]" : $"{elementType}[{new string(',', rank - 1)}]";
            }

            // Build the type name
            var typeName = includeNamespace ? type.FullName ?? type.Name : type.Name;

            // Handle nested types
            if (type.IsNested && !includeNamespace)
            {
                var declaringType = type.DeclaringType;
                if (declaringType != null)
                {
                    var declaringName = declaringType.ToCodeString(format & ~TypeFormat.UseTypeAliases);
                    typeName = $"{declaringName}.{type.Name}";
                }
            }

            return typeName;
        }


        /// <summary>
        /// Gets the generic type name with proper formatting based on the specified format options.
        /// </summary>
        /// <param name="type">The generic type to format.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>A formatted generic type name string.</returns>
        private static string GetGenericTypeName(this Type type, TypeFormat format)
        {
            var genericArguments = type.GetGenericArguments();
            var useAliases = (format & TypeFormat.UseTypeAliases) != 0;
            var includeNamespace = (format & TypeFormat.IncludeNamespace) != 0;

            // Get the base type name (without the `n suffix)
            var typeName = type.Name;
            var genericPartIndex = typeName.IndexOf('`');
            if (genericPartIndex > -1)
            {
                typeName = typeName.Substring(0, genericPartIndex);
            }

            // Build the namespace prefix if requested
            var namespacePrefix = string.Empty;
            if (includeNamespace)
            {
                if (type.Namespace.IsNotNullOrEmpty())
                {
                    namespacePrefix = $"{type.Namespace}.";
                }
            }

            // Handle nested types
            if (type.IsNested && !includeNamespace)
            {
                var declaringType = type.DeclaringType;
                if (declaringType != null)
                {
                    var declaringName = declaringType.ToCodeString(format & ~TypeFormat.UseTypeAliases);
                    typeName = $"{declaringName}.{typeName}";
                }
            }

            // Format generic arguments
            var genericArgumentString = string.Join(", ", Array.ConvertAll(genericArguments, t => t.ToCodeString(format)));
            return $"{namespacePrefix}{typeName}<{genericArgumentString}>";
        }
    }
}
