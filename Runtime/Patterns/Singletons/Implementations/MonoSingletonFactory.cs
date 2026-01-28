using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EasyToolkit.Core.Patterns.Implementations
{
    /// <summary>
    /// Factory for creating MonoBehaviour singleton instances.
    /// </summary>
    internal static class MonoSingletonFactory
    {
        /// <summary>
        /// Finds an existing singleton instance in the scene or creates a new one.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>The found or created singleton instance.</returns>
        /// <exception cref="SingletonInitializationException">Thrown if multiple instances exist.</exception>
        public static T FindOrCreateMonoSingleton<T>() where T : Component, IUnitySingleton
        {
            if (!Application.isPlaying)
                return null;

            T instance;
            var instances = Object.FindObjectsOfType<T>(true);

            if (instances.Length > 1)
            {
                throw new SingletonInitializationException(
                    $"[MonoSingleton] MultipleInstances: '{typeof(T).Name}' has {instances.Length} instances in the scene. " +
                    $"Only one instance is allowed. Destroy duplicate instances.",
                    typeof(T));
            }

            if (instances.Length == 1)
            {
                instance = instances[0];
                instance.OnSingletonInitialize(SingletonInitialMode.Load);
            }
            else
            {
                var gameObject = new GameObject(typeof(T).Name);
                instance = gameObject.AddComponent<T>();
                ApplyConfiguration(instance);
                instance.OnSingletonInitialize(SingletonInitialMode.Create);
            }

            return instance;
        }

        /// <summary>
        /// Applies configuration attributes to a MonoBehaviour singleton.
        /// </summary>
        /// <param name="component">The component to configure.</param>
        internal static void ApplyConfiguration(Component component)
        {
            var config = component.GetType().GetCustomAttribute<MonoSingletonConfigurationAttribute>();
            if (config != null && config.Flags.HasFlag(MonoSingletonFlags.DontDestroyOnLoad))
            {
                Object.DontDestroyOnLoad(component);
            }
        }
    }
}
