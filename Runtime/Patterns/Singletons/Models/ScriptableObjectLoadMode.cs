namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Defines how a ScriptableObject singleton is loaded.
    /// </summary>
    public enum ScriptableObjectLoadMode
    {
        /// <summary>
        /// Creates an in-memory instance without loading from asset file.
        /// </summary>
        Memory,

        /// <summary>
        /// Must load from actual resource/asset file. Throws exception if not found.
        /// </summary>
        Asset,

        /// <summary>
        /// Tries to load from asset file, falls back to in-memory creation if not found.
        /// </summary>
        TryLoadAssetOrFallback
    }
}
