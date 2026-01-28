using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Reflection.Implementations
{
    public static class TypeMatchCaches
    {
        public static string ComputeKey(IReadOnlyList<Type> targetTypes)
        {
            if (targetTypes.Count == 0) return string.Empty;

            var keys = new List<string>();
            foreach (var type in targetTypes)
            {
                keys.Add(type.AssemblyQualifiedName ?? type.FullName);
            }
            return string.Join("+", keys);
        }

        public static string ComputeKey(IReadOnlyList<TypeMatchResult[]> typeMatchResultsList)
        {
            var keys = new List<string>();
            foreach (var typeMatchResults in typeMatchResultsList)
            {
                keys.Add(typeMatchResults.GetHashCode().ToString("X8"));
            }
            keys.Sort();
            return string.Join("+", keys);
        }
    }
}
