using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Configuration flags for MonoBehaviour singleton behavior.
    /// </summary>
    [Flags]
    public enum MonoSingletonFlags
    {
        /// <summary>
        /// No special behavior.
        /// </summary>
        None = 0,

        /// <summary>
        /// Marks the singleton GameObject with DontDestroyOnLoad.
        /// </summary>
        DontDestroyOnLoad = 1 << 0
    }
}
