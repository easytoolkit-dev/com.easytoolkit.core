using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace EasyToolkit.Core.Reflection
{
    public static class TypeUtility
    {
        private static readonly ConcurrentDictionary<string, Type> TypeByNameCache = new();
        private static readonly ConcurrentDictionary<Type, string> TypeNameByTypeCache = new();
        private static readonly ConcurrentDictionary<Type, string> TypeCodeNameByTypeCache_None = new();
        private static readonly ConcurrentDictionary<Type, string> TypeCodeNameByTypeCache_UseTypeAliases = new();
        private static readonly ConcurrentDictionary<Type, string> TypeCodeNameByTypeCache_IncludeNamespace = new();
        private static readonly ConcurrentDictionary<Type, string> TypeCodeNameByTypeCache_Full = new();

        /// <summary>
        /// Mapping of C# built-in type aliases to their corresponding Type objects.
        /// Supports standard syntax sugar types like int, string, bool, etc.
        /// </summary>
        private static readonly Dictionary<string, Type> BuiltInTypeAliases = new()
        {
            { "sbyte", typeof(sbyte) },
            { "byte", typeof(byte) },
            { "short", typeof(short) },
            { "ushort", typeof(ushort) },
            { "int", typeof(int) },
            { "uint", typeof(uint) },
            { "long", typeof(long) },
            { "ulong", typeof(ulong) },
            { "char", typeof(char) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "bool", typeof(bool) },
            { "decimal", typeof(decimal) },
            { "string", typeof(string) },
            { "object", typeof(object) },
            { "void", typeof(void) }
        };

        /// <summary>
        /// Gets the type name for the specified type with caching.
        /// </summary>
        /// <param name="type">The type to get the name for.</param>
        /// <returns>
        /// The assembly-qualified name, full name, or simple name of the type.
        /// </returns>
        public static string GetTypeName([NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return TypeNameByTypeCache.GetOrAdd(type, GetTypeNameCore);
        }

        /// <summary>
        /// Gets the type name for the specified type with caching and custom formatting.
        /// </summary>
        /// <param name="type">The type to get the name for.</param>
        /// <param name="format">The formatting options to apply.</param>
        /// <returns>
        /// A code-style string representation of the type based on the specified format.
        /// </returns>
        /// <remarks>
        /// This method uses <see cref="TypeExtensions.ToCodeString(Type, TypeFormat)"/> internally
        /// to generate type names suitable for code generation scenarios.
        /// Results are cached per type/format combination for performance.
        /// </remarks>
        public static string GetTypeName([NotNull] Type type, TypeFormat format)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return format switch
            {
                TypeFormat.None => TypeCodeNameByTypeCache_None.GetOrAdd(type,
                    t => t.ToCodeString(TypeFormat.None)),
                TypeFormat.UseTypeAliases => TypeCodeNameByTypeCache_UseTypeAliases.GetOrAdd(type,
                    t => t.ToCodeString(TypeFormat.UseTypeAliases)),
                TypeFormat.IncludeNamespace => TypeCodeNameByTypeCache_IncludeNamespace.GetOrAdd(type,
                    t => t.ToCodeString(TypeFormat.IncludeNamespace)),
                TypeFormat.Full =>
                    TypeCodeNameByTypeCache_Full.GetOrAdd(type, t => t.ToCodeString(format)),
                _ => type.ToCodeString(format)
            };
        }

        /// <summary>
        /// Attempts to find a Type from the specified type name.
        /// </summary>
        /// <param name="typeName">
        /// The assembly-qualified name of the type to get.
        /// If the type is in the currently executing assembly or in Mscorlib.dll,
        /// it is sufficient to supply the type name qualified by its namespace.
        /// </param>
        /// <returns>The found Type object.</returns>
        /// <remarks>
        /// This method has enhanced support for generic types. Unity's Type.GetType
        /// has limited support for generics, so this method handles generic types
        /// by parsing the type name, recursively finding the generic type definition
        /// and type arguments, and using MakeGenericType to construct the final type.
        /// </remarks>
        [CanBeNull] public static Type FindType(string typeName)
        {
            ValidateTypeName(typeName);

            return TypeByNameCache.GetOrAdd(typeName, FindTypeCore);
        }

        /// <summary>
        /// Gets the type name for the specified type without caching.
        /// </summary>
        /// <param name="type">The type to get the name for.</param>
        /// <returns>
        /// The assembly-qualified name, full name, or simple name of the type.
        /// </returns>
        private static string GetTypeNameCore([NotNull] Type type)
        {
            return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
        }


        /// <summary>
        /// Attempts to find a Type from the specified type name without caching.
        /// </summary>
        /// <param name="typeName">
        /// The assembly-qualified name of the type to get.
        /// If the type is in the currently executing assembly or in Mscorlib.dll,
        /// it is sufficient to supply the type name qualified by its namespace.
        /// </param>
        /// <returns>The found Type object.</returns>
        /// <remarks>
        /// This method has enhanced support for generic types. Unity's Type.GetType
        /// has limited support for generics, so this method handles generic types
        /// by parsing the type name, recursively finding the generic type definition
        /// and type arguments, and using MakeGenericType to construct the final type.
        /// It also supports array types with alias element types like int[] and string[].
        /// </remarks>
        [CanBeNull] private static Type FindTypeCore(string typeName)
        {
            // Check built-in type aliases first (int, string, bool, etc.)
            if (BuiltInTypeAliases.TryGetValue(typeName, out var aliasType))
            {
                return aliasType;
            }

            // Handle array types with alias element types (e.g., int[], string[], int[,][,,])
            if (TryParseArrayTypeName(typeName, out var elementTypeName, out var ranks))
            {
                var elementType = FindType(elementTypeName);
                if (elementType != null)
                {
                    return BuildArrayType(elementType, ranks);
                }
            }

            // Try to parse and handle code-like generic types (e.g., List<String>)
            if (TryParseCodeLikeGenericTypeName(typeName, out var codeLikeDefinitionName,
                    out var codeLikeTypeArgumentNames))
            {
                var type = FindGenericType(codeLikeDefinitionName, codeLikeTypeArgumentNames);
                if (type != null)
                {
                    return type;
                }
            }

            // Try to parse and handle reflection-style generic types (e.g., List`1[[String]])
            // Unity's Type.GetType has limited support for generics, so we handle them explicitly
            if (TryParseGenericTypeName(typeName, out var genericDefinitionName, out var typeArgumentNames))
            {
                var type = FindGenericType(genericDefinitionName, typeArgumentNames);
                if (type != null)
                {
                    return type;
                }
            }

            // Fall back to standard search
            return FindTypeStandard(typeName);
        }

        /// <summary>
        /// Finds a generic type by its definition name and type arguments.
        /// </summary>
        /// <param name="genericDefinitionName">The name of the generic type definition.</param>
        /// <param name="typeArgumentNames">An array of type argument names.</param>
        /// <returns>
        /// The constructed generic type, or null if the definition or any type argument was not found.
        /// </returns>
        /// <remarks>
        /// This method recursively finds the generic type definition and all type arguments,
        /// then constructs the generic type using MakeGenericType.
        /// If any part cannot be found or MakeGenericType fails, returns null.
        /// </remarks>
        [CanBeNull] private static Type FindGenericType(string genericDefinitionName, string[] typeArgumentNames)
        {
            // Recursively find the generic type definition
            var genericDefinition = FindType(genericDefinitionName);
            if (genericDefinition == null)
            {
                return null;
            }

            // Recursively find all type arguments
            var typeArguments = new Type[typeArgumentNames.Length];
            for (int i = 0; i < typeArgumentNames.Length; i++)
            {
                typeArguments[i] = FindType(typeArgumentNames[i]);
                if (typeArguments[i] == null)
                {
                    return null;
                }
            }

            try
            {
                return genericDefinition.MakeGenericType(typeArguments);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds a type using standard search methods.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>
        /// The found Type object, or null if the type was not found.
        /// </returns>
        /// <remarks>
        /// This method first tries Type.GetType, which handles most common scenarios.
        /// If not found, it searches through all loaded assemblies using SearchAllAssemblies.
        /// </remarks>
        [CanBeNull] private static Type FindTypeStandard(string typeName)
        {
            // First, try Type.GetType which handles most common scenarios
            var type = Type.GetType(typeName, throwOnError: false);
            if (type != null)
            {
                return type;
            }

            // If not found, search through all loaded assemblies
            return SearchAllAssemblies(typeName);
        }

        /// <summary>
        /// Searches for a type across all currently loaded assemblies.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>
        /// The found Type object, or null if the type was not found.
        /// </returns>
        /// <remarks>
        /// This method searches through all loaded assemblies for the specified type.
        /// It first tries to match by assembly-qualified name, then by full name
        /// (namespace + type name), and finally by simple type name.
        /// </remarks>
        private static Type SearchAllAssemblies(string typeName)
        {
            var assemblies = AssemblyUtility.GetAllAssemblies();

            // First pass: exact match with assembly-qualified name
            foreach (var assembly in assemblies)
            {
                try
                {
                    Type type = assembly.GetType(typeName, throwOnError: false);
                    if (type != null)
                    {
                        return type;
                    }
                }
                catch (Exception)
                {
                    // Continue searching other assemblies
                    continue;
                }
            }

            // Second pass: match by full name (namespace + type name) or simple type name
            // Combined into a single pass to avoid duplicate GetTypes() calls
            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.FullName != null &&
                            string.Equals(type.FullName, typeName, StringComparison.Ordinal))
                        {
                            return type;
                        }

                        if (string.Equals(type.Name, typeName, StringComparison.Ordinal))
                        {
                            return type;
                        }
                    }
                }
                catch (Exception)
                {
                    // Some assemblies may not be accessible
                    continue;
                }
            }

            return null;
        }

        /// <summary>
        /// Regex pattern for matching generic type names with or without assembly-qualified arguments.
        /// Matches formats:
        /// - With assembly: TypeName`N[[Arg1, Assembly],[Arg2, Assembly]], Assembly
        /// - Without assembly: TypeName`N[[Arg1],[Arg2]]
        /// </summary>
        private static readonly Regex GenericTypeNamePattern = new(
            @"^([,\w\s\.]+`?\d*)(?:\s*,\s*([^\[\]]+))?\s*\[\[(.+)\]\](?:\s*,\s*([^\[\]]+))?$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Regex pattern for matching code-like generic type syntax (e.g., List&lt;T&gt;, Dictionary&lt;TKey, TValue&gt;).
        /// Matches formats:
        /// - Simple: List&lt;String&gt;
        /// - With namespace: System.Collections.Generic.List&lt;System.String&gt;
        /// - Multiple arguments: Dictionary&lt;String, Int32&gt;
        /// </summary>
        private static readonly Regex CodeLikeGenericPattern = new(
            @"^([,\w\s\.]+)(?:\s*,\s*([^\[\]<>]+))?\s*<(.+)>$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Regex pattern for matching array type syntax with support for mixed multi-dimensional arrays.
        /// Matches formats:
        /// - Single-dimensional: int[], string[]
        /// - Multi-dimensional: int[,], string[,], int[,,]
        /// - Mixed multi-dimensional: int[,][,][] (array of 3D arrays of 2D arrays)
        /// - Complex combinations: int[,,,][][,]
        /// The pattern ensures brackets contain only commas, distinguishing from generic types
        /// like List`1[[T]] which have nested brackets.
        /// </summary>
        private static readonly Regex ArrayTypeNamePattern = new(
            @"^\s*(?<element>[^\[\]]+?)\s*(?<arrayParts>(?:\[(?:,*)\]\s*)*)$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Regex pattern for matching individual array rank parts (e.g., [], [,], [,,]).
        /// </summary>
        private static readonly Regex ArrayRankPartPattern = new(
            @"\[(,*)\]",
            RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Attempts to parse a generic type name into its definition and type arguments.
        /// </summary>
        /// <param name="typeName">The generic type name to parse (with or without assembly info).</param>
        /// <param name="genericDefinitionName">
        /// When this method returns, contains the name of the generic type definition.
        /// </param>
        /// <param name="typeArgumentNames">
        /// When this method returns, contains an array of type argument names.
        /// </param>
        /// <returns>
        /// true if the type name was successfully parsed as a generic type; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method handles complex generic type names where Unity's Type.GetType may fail.
        /// Examples:
        /// - With assembly: System.Collections.Generic.Dictionary`2[[Key, Assembly],[Value, Assembly]], Assembly
        /// - Without assembly: System.Collections.Generic.Dictionary`2[[Key],[Value]]
        /// </remarks>
        private static bool TryParseGenericTypeName(
            string typeName,
            out string genericDefinitionName,
            out string[] typeArgumentNames)
        {
            genericDefinitionName = null;
            typeArgumentNames = null;

            var match = GenericTypeNamePattern.Match(typeName);
            if (!match.Success)
            {
                return false;
            }

            var typeNamePart = match.Groups[1].Value.TrimEnd(',');
            var typeArgumentsContent = match.Groups[3].Value;

            // Build the generic type definition name
            // Group 4: main assembly after ]], Group 2: assembly after type name (before [[)
            var mainAssembly = match.Groups[4].Value;
            if (string.IsNullOrEmpty(mainAssembly))
            {
                mainAssembly = match.Groups[2].Value;
            }

            if (!string.IsNullOrEmpty(mainAssembly))
            {
                mainAssembly = mainAssembly.Trim();
                genericDefinitionName = $"{typeNamePart}, {mainAssembly}";
            }
            else
            {
                genericDefinitionName = typeNamePart;
            }

            // Parse the type arguments
            typeArgumentNames = ParseTypeArguments(typeArgumentsContent);

            return typeArgumentNames != null && typeArgumentNames.Length > 0;
        }

        /// <summary>
        /// Attempts to parse a code-like generic type name into its definition and type arguments.
        /// </summary>
        /// <param name="typeName">The code-like generic type name to parse (e.g., List&lt;String&gt;).</param>
        /// <param name="genericDefinitionName">
        /// When this method returns, contains the name of the generic type definition.
        /// </param>
        /// <param name="typeArgumentNames">
        /// When this method returns, contains an array of type argument names.
        /// </param>
        /// <returns>
        /// true if the type name was successfully parsed as a code-like generic type; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method handles code-like generic type syntax (e.g., List&lt;String&gt;, Dictionary&lt;String, Int32&gt;).
        /// It converts the code-like syntax to the reflection format needed for type lookup.
        /// Examples:
        /// - Simple: List&lt;String&gt; → List`1[[System.String]]
        /// - With namespace: System.Collections.Generic.Dictionary&lt;System.String, System.Int32&gt;
        /// - Nested: List&lt;Dictionary&lt;String, Int32&gt;&gt;
        /// </remarks>
        private static bool TryParseCodeLikeGenericTypeName(
            string typeName,
            out string genericDefinitionName,
            out string[] typeArgumentNames)
        {
            genericDefinitionName = null;
            typeArgumentNames = null;

            var match = CodeLikeGenericPattern.Match(typeName);
            if (!match.Success)
            {
                return false;
            }

            var typeNamePart = match.Groups[1].Value.Trim();
            var typeArgumentsContent = match.Groups[3].Value;
            var mainAssembly = match.Groups[2].Value.Trim();

            // Build the generic type definition name with the arity count
            var argumentCount = CountGenericArguments(typeArgumentsContent);
            genericDefinitionName = $"{typeNamePart}`{argumentCount}";

            if (!string.IsNullOrEmpty(mainAssembly))
            {
                genericDefinitionName += $", {mainAssembly}";
            }

            // Parse the type arguments
            typeArgumentNames = ParseCodeLikeTypeArguments(typeArgumentsContent);

            return typeArgumentNames != null && typeArgumentNames.Length > 0;
        }

        /// <summary>
        /// Counts the number of top-level generic type arguments in the type arguments string.
        /// </summary>
        /// <param name="argumentsContent">The content between &lt; and &gt;.</param>
        /// <returns>The number of top-level type arguments.</returns>
        /// <remarks>
        /// This method counts comma-separated arguments while respecting nested generics.
        /// For example, "String, List&lt;Int32&gt;" has 2 arguments, not 3.
        /// </remarks>
        private static int CountGenericArguments(string argumentsContent)
        {
            int count = 1; // At least one argument
            int depth = 0;

            foreach (char c in argumentsContent)
            {
                switch (c)
                {
                    case '<':
                        depth++;
                        break;
                    case '>':
                        depth--;
                        break;
                    case ',':
                        if (depth == 0)
                        {
                            count++;
                        }

                        break;
                }
            }

            return count;
        }

        /// <summary>
        /// Parses type arguments from a code-like generic type name argument string.
        /// </summary>
        /// <param name="argumentsContent">The content between &lt; and &gt;.</param>
        /// <returns>
        /// An array of type argument names.
        /// </returns>
        /// <remarks>
        /// This method handles nested generic types by tracking bracket depth.
        /// Type arguments are separated by commas, but we must not split at commas
        /// that are part of nested generic type definitions.
        /// </remarks>
        private static string[] ParseCodeLikeTypeArguments(string argumentsContent)
        {
            var arguments = new List<string>();
            var currentArg = new System.Text.StringBuilder();
            int depth = 0;

            foreach (char c in argumentsContent)
            {
                switch (c)
                {
                    case '<':
                        depth++;
                        currentArg.Append(c);
                        break;

                    case '>':
                        depth--;
                        currentArg.Append(c);
                        break;

                    case ',':
                        if (depth == 0)
                        {
                            // Top-level comma - this is a separator
                            var arg = currentArg.ToString().Trim();
                            if (!string.IsNullOrEmpty(arg))
                            {
                                arguments.Add(arg);
                            }

                            currentArg.Clear();
                        }
                        else
                        {
                            currentArg.Append(c);
                        }

                        break;

                    default:
                        currentArg.Append(c);
                        break;
                }
            }

            // Add the last argument
            var lastArg = currentArg.ToString().Trim();
            if (!string.IsNullOrEmpty(lastArg))
            {
                arguments.Add(lastArg);
            }

            return arguments.ToArray();
        }

        /// <summary>
        /// Parses type arguments from a generic type name argument string.
        /// </summary>
        /// <param name="argumentsContent">The content between [[ and ]].</param>
        /// <returns>
        /// An array of assembly-qualified type argument names.
        /// </returns>
        /// <remarks>
        /// This method handles nested generic types by tracking bracket depth.
        /// Type arguments are separated by commas, but we must not split at commas
        /// that are part of nested generic type definitions.
        /// </remarks>
        private static string[] ParseTypeArguments(string argumentsContent)
        {
            var arguments = new List<string>();
            var currentArg = new System.Text.StringBuilder();
            int depth = 0;
            bool inAssembly = false;

            foreach (char c in argumentsContent)
            {
                switch (c)
                {
                    case '[':
                        depth++;
                        currentArg.Append(c);
                        break;

                    case ']':
                        depth--;
                        currentArg.Append(c);
                        inAssembly = depth == 0;
                        break;

                    case ',':
                        if (depth == 0 || (depth == 1 && inAssembly))
                        {
                            // Top-level comma or comma in assembly section - this is a separator
                            var arg = currentArg.ToString().Trim();
                            if (!string.IsNullOrEmpty(arg))
                            {
                                arguments.Add(arg);
                            }

                            currentArg.Clear();
                            inAssembly = false;
                        }
                        else
                        {
                            currentArg.Append(c);
                        }

                        break;

                    default:
                        currentArg.Append(c);
                        break;
                }
            }

            // Add the last argument
            var lastArg = currentArg.ToString().Trim();
            if (!string.IsNullOrEmpty(lastArg))
            {
                arguments.Add(lastArg);
            }

            return arguments.ToArray();
        }

        /// <summary>
        /// Validates that the provided type name is not null or empty.
        /// </summary>
        /// <param name="typeName">The type name to validate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="typeName"/> is null or empty.
        /// </exception>
        private static void ValidateTypeName(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentNullException(
                    nameof(typeName),
                    "Type name cannot be null or empty. " +
                    "Please provide a valid type name.");
            }
        }

        /// <summary>
        /// Attempts to parse an array type name into its element type name and array ranks.
        /// </summary>
        /// <param name="typeName">The type name to parse.</param>
        /// <param name="elementTypeName">
        /// When this method returns, contains the name of the element type.
        /// </param>
        /// <param name="ranks">
        /// When this method returns, contains the ranks (dimensionalities) of each array level.
        /// For mixed multi-dimensional arrays like int[,][,,][], returns [2, 3, 1].
        /// </param>
        /// <returns>
        /// true if the type name was successfully parsed as an array type; otherwise, false.
        /// </returns>
        /// <remarks>
        /// This method handles array type syntax including:
        /// - Single-dimensional: int[], string[] → ranks: [1]
        /// - Multi-dimensional: int[,], string[,,] → ranks: [2], [3]
        /// - Mixed multi-dimensional: int[,][,,][] → ranks: [2, 3, 1]
        /// The rank for each level is determined by counting the number of commas between the brackets.
        /// Empty brackets [] have rank 1, [,] has rank 2, [,,] has rank 3, etc.
        /// This method uses a regex pattern to distinguish array syntax from generic type syntax.
        /// Generic types like List`1[[T]] have nested brackets and will not match the array pattern.
        /// </remarks>
        private static bool TryParseArrayTypeName(
            string typeName,
            out string elementTypeName,
            out int[] ranks)
        {
            elementTypeName = null;
            ranks = null;

            var match = ArrayTypeNamePattern.Match(typeName);
            if (!match.Success)
            {
                return false;
            }

            elementTypeName = match.Groups["element"].Value.Trim();
            var arrayParts = match.Groups["arrayParts"].Value;

            // Parse each array rank part (e.g., [] -> rank 1, [,] -> rank 2, [,,] -> rank 3)
            var rankPartMatches = ArrayRankPartPattern.Matches(arrayParts);
            if (rankPartMatches.Count == 0)
            {
                return false;
            }

            ranks = new int[rankPartMatches.Count];
            for (int i = 0; i < rankPartMatches.Count; i++)
            {
                var commaContent = rankPartMatches[i].Groups[1].Value;
                // Rank is comma count + 1
                ranks[i] = commaContent.Length + 1;
            }

            return true;
        }

        /// <summary>
        /// Builds an array type from an element type and a list of array ranks.
        /// </summary>
        /// <param name="elementType">The element type of the array.</param>
        /// <param name="ranks">The ranks for each array level, applied left to right.</param>
        /// <returns>
        /// The constructed array type, or null if construction fails.
        /// </returns>
        /// <remarks>
        /// For mixed multi-dimensional arrays like int[,][,,][] (ranks: [2, 3, 1]):
        /// 1. Start with element type (int)
        /// 2. Apply rank 2: int[,]
        /// 3. Apply rank 3: int[,][,,]
        /// 4. Apply rank 1: int[,][,,][]
        /// Each rank is applied sequentially using Type.MakeArrayType(rank).
        /// </remarks>
        private static Type BuildArrayType(Type elementType, int[] ranks)
        {
            if (elementType == null || ranks == null || ranks.Length == 0)
            {
                return null;
            }

            Type currentType = elementType;
            foreach (var rank in ranks)
            {
                try
                {
                    currentType = rank == 1
                        ? currentType.MakeArrayType()
                        : currentType.MakeArrayType(rank);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return currentType;
        }
    }
}
