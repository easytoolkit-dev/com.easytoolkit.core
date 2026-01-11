using System;

namespace EasyToolKit.Core.Patterns
{
    /// <summary>
    /// Configures MonoBehaviour singleton behavior via attribute.
    /// </summary>
    /// <remarks>
    /// Usage example:
    /// <code>
    /// [MonoSingletonConfiguration(MonoSingletonFlags.DontDestroyOnLoad)]
    /// public class AudioManager : MonoSingleton&lt;AudioManager&gt; { }
    /// </code>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MonoSingletonConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Gets the configuration flags for this singleton.
        /// </summary>
        public MonoSingletonFlags Flags { get; }

        /// <summary>
        /// Initializes a new instance with the specified flags.
        /// </summary>
        /// <param name="flags">The configuration flags.</param>
        public MonoSingletonConfigurationAttribute(MonoSingletonFlags flags)
        {
            Flags = flags;
        }
    }
}
