using System.Reflection;
using UnityEngine;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// ScriptableObject singleton base class with configurable loading modes.
    /// Supports in-memory creation, asset loading, and fallback strategies.
    /// </summary>
    /// <typeparam name="T">The singleton type, must derive from ScriptableObjectSingleton{T}</typeparam>
    /// <remarks>
    /// Usage:
    /// <code>
    /// [ScriptableObjectSingletonConfiguration("Resources/Configs", ScriptableObjectLoadMode.Asset)]
    /// public class GameConfig : ScriptableObjectSingleton&lt;GameConfig&gt;
    /// {
    ///     [SerializeField] private int _maxPlayers = 4;
    ///     public int MaxPlayers => _maxPlayers;
    /// }
    ///
    /// int maxPlayers = GameConfig.Instance.MaxPlayers;
    /// </code>
    /// </remarks>
    public class ScriptableObjectSingleton<T> : ScriptableObject, IUnitySingleton
        where T : ScriptableObjectSingleton<T>
    {
        private static readonly object InitLock = new();
        private static T s_instance;
        private static ScriptableObjectSingletonConfigurationAttribute s_configurationAttribute;

        /// <summary>
        /// Gets the configuration attribute for this singleton type.
        /// </summary>
        /// <exception cref="SingletonInitializationException">Thrown if the attribute is not defined.</exception>
        public static ScriptableObjectSingletonConfigurationAttribute ConfigurationAttribute
        {
            get
            {
                if (s_configurationAttribute == null)
                {
                    s_configurationAttribute =
                        typeof(T).GetCustomAttribute<ScriptableObjectSingletonConfigurationAttribute>();
                    if (s_configurationAttribute == null)
                    {
                        throw new SingletonInitializationException(
                            $"[ScriptableObjectSingleton] MissingConfiguration: Type '{typeof(T).Name}' must define " +
                            $"'{nameof(ScriptableObjectSingletonConfigurationAttribute)}' attribute. " +
                            $"Example: [ScriptableObjectSingletonConfiguration(\"Resources/Configs\", ScriptableObjectLoadMode.Asset)]",
                            typeof(T));
                    }
                }

                return s_configurationAttribute;
            }
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <exception cref="SingletonInitializationException">Thrown if initialization fails.</exception>
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    lock (InitLock)
                    {
                        if (s_instance != null)
                            return s_instance;

                        var config = ConfigurationAttribute;
                        s_instance = config.LoadMode switch
                        {
                            ScriptableObjectLoadMode.Memory => Implementations.ScriptableObjectFactory.CreateInMemory<T>(),
                            ScriptableObjectLoadMode.Asset => Implementations.ScriptableObjectFactory.LoadAssetOrFallback<T>(
                                config.AssetDirectory,
                                config.AssetName,
                                fallbackToMemory: false),
                            ScriptableObjectLoadMode.TryLoadAssetOrFallback => Implementations.ScriptableObjectFactory.LoadAssetOrFallback<T>(
                                config.AssetDirectory,
                                config.AssetName,
                                fallbackToMemory: true),
                            _ => throw new SingletonInitializationException(
                                $"[ScriptableObjectSingleton] InvalidLoadMode: Unsupported load mode '{config.LoadMode}'",
                                typeof(T))
                        };
                    }
                }

                return s_instance;
            }
        }

        /// <summary>
        /// Called when the singleton is initialized.
        /// Override to provide custom initialization.
        /// </summary>
        /// <param name="mode">Whether the instance was loaded from asset or created in memory.</param>
        protected virtual void OnSingletonInitialize(SingletonInitialMode mode)
        {
        }

        /// <summary>
        /// Explicit interface implementation.
        /// </summary>
        void IUnitySingleton.OnSingletonInitialize(SingletonInitialMode mode) => OnSingletonInitialize(mode);
    }
}
