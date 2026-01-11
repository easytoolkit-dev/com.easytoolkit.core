using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyToolKit.Core.Patterns.Implementations
{
    /// <summary>
    /// Factory for creating and loading ScriptableObject singleton instances.
    /// </summary>
    internal static class ScriptableObjectFactory
    {
        /// <summary>
        /// Creates an in-memory ScriptableObject singleton instance.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>The created instance.</returns>
        public static T CreateInMemory<T>() where T : ScriptableObject, IUnitySingleton
        {
            var instance = ScriptableObject.CreateInstance<T>();
            instance.name = typeof(T).Name;
            instance.OnSingletonInitialize(SingletonInitialMode.Create);
            return instance;
        }

        /// <summary>
        /// Loads or creates a ScriptableObject singleton based on the fallback mode.
        /// Supports both Editor (with auto-asset creation) and Runtime loading.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <param name="directory">The asset directory.</param>
        /// <param name="assetName">The asset name.</param>
        /// <param name="fallbackToMemory">Whether to fallback to in-memory creation if asset not found.</param>
        /// <returns>The loaded or created instance.</returns>
        public static T LoadAssetOrFallback<T>(string directory, string assetName, bool fallbackToMemory)
            where T : ScriptableObject, IUnitySingleton
        {
#if UNITY_EDITOR
            return LoadAssetOrFallbackInEditor<T>(directory, assetName, fallbackToMemory);
#else
            return LoadAssetInRuntime<T>(directory, assetName, fallbackToMemory);
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor-specific loading with auto-asset creation support.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <param name="directory">The asset directory.</param>
        /// <param name="assetName">The asset name.</param>
        /// <param name="fallbackToMemory">Whether to fallback to in-memory creation.</param>
        /// <returns>The loaded or created instance.</returns>
        private static T LoadAssetOrFallbackInEditor<T>(string directory, string assetName, bool fallbackToMemory)
            where T : ScriptableObject, IUnitySingleton
        {
            // If directory is empty, try to find manually created instance
            if (string.IsNullOrEmpty(directory))
            {
                var foundAssets = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                if (foundAssets.Length != 0)
                {
                    var instance = AssetDatabase.LoadAssetAtPath<T>(
                        AssetDatabase.GUIDToAssetPath(foundAssets[0]));
                    if (instance != null)
                    {
                        instance.OnSingletonInitialize(SingletonInitialMode.Load);
                        return instance;
                    }
                }

                if (!fallbackToMemory)
                {
                    throw new SingletonInitializationException(
                        $"[ScriptableObjectSingleton] AssetNotFound: No manually created asset found for '{typeof(T).Name}'. " +
                        $"Please create a '{typeof(T).Name}' asset in your project or set the asset directory path.",
                        typeof(T));
                }

                return CreateInMemory<T>();
            }

            {
                // Directory is specified, try to load from path
                if (!directory.StartsWith("Assets/"))
                {
                    directory = "Assets/" + directory;
                }

                var name = string.IsNullOrEmpty(assetName) ? typeof(T).Name : assetName;
                var assetFilePath = directory.TrimEnd('/') + "/" + name + ".asset";

                var instance = AssetDatabase.LoadAssetAtPath<T>(assetFilePath);

                if (instance != null)
                {
                    instance.OnSingletonInitialize(SingletonInitialMode.Load);
                    return instance;
                }

                // Try to find relocated asset
                var relocatedAssets = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                if (relocatedAssets.Length != 0)
                {
                    instance = AssetDatabase.LoadAssetAtPath<T>(
                        AssetDatabase.GUIDToAssetPath(relocatedAssets[0]));
                    if (instance != null)
                    {
                        instance.OnSingletonInitialize(SingletonInitialMode.Load);
                        return instance;
                    }
                }

                // Asset not found - create it in editor
                instance = ScriptableObject.CreateInstance<T>();

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(new DirectoryInfo(directory).FullName);
                    AssetDatabase.Refresh();
                }

                AssetDatabase.CreateAsset(instance, assetFilePath);
                AssetDatabase.SaveAssets();
                EditorUtility.SetDirty(instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                instance.OnSingletonInitialize(SingletonInitialMode.Create);
                return instance;
            }
        }
#endif

        /// <summary>
        /// Runtime loading using Resources.Load.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <param name="directory">The asset directory.</param>
        /// <param name="assetName">The asset name.</param>
        /// <param name="fallbackToMemory">Whether to fallback to in-memory creation.</param>
        /// <returns>The loaded or created instance.</returns>
        private static T LoadAssetInRuntime<T>(string directory, string assetName, bool fallbackToMemory)
            where T : ScriptableObject, IUnitySingleton
        {
            var (fullPath, resourcesPath) = ScriptableObjectPathResolver.ResolvePath(
                directory,
                assetName,
                typeof(T));

            var name = string.IsNullOrEmpty(assetName) ? typeof(T).Name : assetName;
            var loadPath = string.IsNullOrEmpty(resourcesPath) ? name : resourcesPath + "/" + name;
            var instance = Resources.Load<T>(loadPath);

            if (instance == null)
            {
                if (fallbackToMemory)
                {
                    return CreateInMemory<T>();
                }

                throw new SingletonInitializationException(
                    $"[ScriptableObjectSingleton] AssetNotFound: Cannot load '{typeof(T).Name}' from path '{fullPath}'. " +
                    $"Ensure the asset exists at '{loadPath}' or use LoadMode.Memory for in-memory creation.",
                    typeof(T));
            }

            instance.OnSingletonInitialize(SingletonInitialMode.Load);
            return instance;
        }
    }
}
