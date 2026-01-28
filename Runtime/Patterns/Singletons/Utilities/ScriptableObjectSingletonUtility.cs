#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Extension methods for ScriptableObject singletons.
    /// Provides asset management helpers and editor utilities.
    /// </summary>
    public static class ScriptableObjectSingletonUtility
    {
        /// <summary>
        /// Gets the asset path of a ScriptableObject singleton.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>The asset path, or null if not found.</returns>
        public static string GetAssetPath<T>() where T : ScriptableObjectSingleton<T>
        {
            //TODO: according attribute
            var foundAssets = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            if (foundAssets.Length != 0)
            {
                return AssetDatabase.GUIDToAssetPath(foundAssets[0]);
            }
            return null;
        }

        /// <summary>
        /// Checks if a ScriptableObject singleton asset exists.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if the asset exists, false otherwise.</returns>
        public static bool AssetExists<T>() where T : ScriptableObjectSingleton<T>
        {
            return GetAssetPath<T>() != null;
        }

        /// <summary>
        /// Forces a ScriptableObject singleton to reload from asset.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        public static void ReloadAsset<T>() where T : ScriptableObjectSingleton<T>
        {
            var path = GetAssetPath<T>();
            if (path != null)
            {
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        /// <summary>
        /// Selects a ScriptableObject singleton asset in the Unity editor.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if the asset was selected, false if not found.</returns>
        public static bool SelectAsset<T>() where T : ScriptableObjectSingleton<T>
        {
            var path = GetAssetPath<T>();
            if (path != null)
            {
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<T>(path);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ping a ScriptableObject singleton asset in the Unity editor project window.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if the asset was pinged, false if not found.</returns>
        public static bool PingAsset<T>() where T : ScriptableObjectSingleton<T>
        {
            var path = GetAssetPath<T>();
            if (path != null)
            {
                var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null)
                {
                    EditorGUIUtility.PingObject(obj);
                    return true;
                }
            }
            return false;
        }
    }
}
#endif
