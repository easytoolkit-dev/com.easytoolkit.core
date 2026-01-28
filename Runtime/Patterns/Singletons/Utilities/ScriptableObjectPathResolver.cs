using System;
using UnityEngine;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Provides path resolution and validation for ScriptableObject singleton loading.
    /// </summary>
    internal static class ScriptableObjectPathResolver
    {
        /// <summary>
        /// Resolves the full and Resources paths for a ScriptableObject asset.
        /// </summary>
        /// <param name="directory">The asset directory.</param>
        /// <param name="assetName">The asset name.</param>
        /// <param name="type">The singleton type.</param>
        /// <returns>A tuple of full path and Resources path.</returns>
        public static (string fullPath, string resourcesPath) ResolvePath(
            string directory,
            string assetName,
            Type type)
        {
            var name = string.IsNullOrEmpty(assetName) ? type.Name : assetName;
            var normalizedDir = NormalizePath(directory);

            if (IsResourcesPath(normalizedDir))
            {
                var resourcesPath = ExtractResourcesPath(normalizedDir);
                var fullPath = $"Assets/{normalizedDir}/{name}.asset";
                return (fullPath, resourcesPath);
            }

            if (IsEditorPath(normalizedDir))
            {
#if UNITY_EDITOR
                var fullPath = $"Assets/{normalizedDir}/{name}.asset";
                return (fullPath, normalizedDir);
#else
                throw new SingletonInitializationException(
                    $"[ScriptableObjectPathResolver] EditorPathInBuild: Cannot load '{type.Name}' from editor-only path " +
                    $"'{normalizedDir}' in a build. Use a Resources path or change LoadMode to Memory.",
                    type);
#endif
            }

            // Default: treat as Resources path
            var resourcesPathDefault = normalizedDir;
            var fullPathDefault = $"Assets/{normalizedDir}/{name}.asset";
            return (fullPathDefault, resourcesPathDefault);
        }

        /// <summary>
        /// Extracts the Resources-relative path from a full path.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns>The Resources-relative path.</returns>
        public static string ExtractResourcesPath(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
                return string.Empty;

            var resourcesIndex = fullPath.IndexOf("Resources", StringComparison.OrdinalIgnoreCase);
            if (resourcesIndex < 0)
                return fullPath;

            var afterResources = fullPath.Substring(resourcesIndex + "Resources".Length);
            return afterResources.Trim('/', '\\');
        }

        /// <summary>
        /// Validates that a path exists for the given type.
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <param name="type">The singleton type.</param>
        /// <exception cref="SingletonInitializationException">Thrown if the path is invalid.</exception>
        public static void ValidatePath(string path, Type type)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new SingletonInitializationException(
                    $"[ScriptableObjectPathResolver] EmptyPath: Path for '{type.Name}' cannot be null or empty. " +
                    $"Provide a valid directory path.",
                    type);
            }
        }

        /// <summary>
        /// Checks if a path is within a Resources folder.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path contains 'Resources', false otherwise.</returns>
        public static bool IsResourcesPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) &&
                   path.IndexOf("Resources", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Checks if a path is an editor-only path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path is in an Editor folder, false otherwise.</returns>
        public static bool IsEditorPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) &&
                   path.IndexOf("Editor", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Normalizes a path to use forward slashes and removes trailing slashes.
        /// </summary>
        /// <param name="path">The path to normalize.</param>
        /// <returns>The normalized path.</returns>
        private static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            return path.Trim()
                .Trim('/', '\\')
                .Replace('\\', '/')
                .TrimEnd('/');
        }
    }
}
