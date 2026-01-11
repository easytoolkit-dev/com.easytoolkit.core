using System;

namespace EasyToolKit.Core.Patterns
{
    /// <summary>
    /// Configures ScriptableObject singleton loading behavior.
    /// </summary>
    /// <remarks>
    /// Usage example:
    /// <code>
    /// [ScriptableObjectSingletonConfiguration("Resources/Configs", ScriptableObjectLoadMode.Asset)]
    /// public class GameConfig : ScriptableObjectSingleton&lt;GameConfig&gt; { }
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ScriptableObjectSingletonConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Gets the load mode for this singleton.
        /// </summary>
        public ScriptableObjectLoadMode LoadMode { get; }

        /// <summary>
        /// Gets or sets the optional custom asset name.
        /// If null or empty, the type name will be used.
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Gets the asset directory path for this singleton.
        /// </summary>
        public string AssetDirectory { get; }

        /// <summary>
        /// Initializes a new instance with Asset load mode.
        /// </summary>
        /// <param name="assetDirectory">The directory path for the asset.</param>
        public ScriptableObjectSingletonConfigurationAttribute(string assetDirectory)
            : this(assetDirectory, ScriptableObjectLoadMode.Asset) { }

        /// <summary>
        /// Initializes a new instance with the specified load mode.
        /// </summary>
        /// <param name="assetDirectory">The directory path for the asset.</param>
        /// <param name="loadMode">The load mode to use.</param>
        public ScriptableObjectSingletonConfigurationAttribute(
            string assetDirectory,
            ScriptableObjectLoadMode loadMode)
        {
            AssetDirectory = NormalizePath(assetDirectory);
            LoadMode = loadMode;
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
