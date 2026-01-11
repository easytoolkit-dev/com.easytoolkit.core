using UnityEngine;

namespace EasyToolKit.Core.Patterns
{
    /// <summary>
    /// Extension methods for MonoBehaviour singletons.
    /// Provides Unity lifecycle helpers and state validation.
    /// </summary>
    public static class MonoSingletonUtility
    {
        /// <summary>
        /// Checks if a MonoBehaviour singleton is in a valid state.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if initialized and not destroyed, false otherwise.</returns>
        public static bool IsValid<T>() where T : MonoSingleton<T>
        {
            return MonoSingleton<T>.IsInitialized;
        }

        /// <summary>
        /// Tries to get a MonoBehaviour singleton instance.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <param name="instance">The retrieved instance, or null if not available.</param>
        /// <returns>True if instance was retrieved, false otherwise.</returns>
        public static bool TryGetInstance<T>(out T instance) where T : MonoSingleton<T>
        {
            if (MonoSingleton<T>.IsInitialized)
            {
                instance = MonoSingleton<T>.Instance;
                return true;
            }

            instance = null;
            return false;
        }

        /// <summary>
        /// Gets the current state of a MonoBehaviour singleton.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>The current state.</returns>
        public static MonoSingletonState GetState<T>() where T : MonoSingleton<T>
        {
            return MonoSingleton<T>.CurrentState;
        }

        /// <summary>
        /// Checks if a MonoBehaviour singleton is in the destroyed state.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if destroyed, false otherwise.</returns>
        public static bool IsDestroyed<T>() where T : MonoSingleton<T>
        {
            return MonoSingleton<T>.CurrentState == MonoSingletonState.Destroyed;
        }

        /// <summary>
        /// Checks if a MonoBehaviour singleton was created via factory.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if created via factory, false otherwise.</returns>
        public static bool IsCreatedViaFactory<T>() where T : MonoSingleton<T>
        {
            return MonoSingleton<T>.CurrentState == MonoSingletonState.CreatedViaFactory;
        }

        /// <summary>
        /// Checks if a MonoBehaviour singleton was initialized in Awake.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if initialized in Awake, false otherwise.</returns>
        public static bool IsInitializedInAwake<T>() where T : MonoSingleton<T>
        {
            return MonoSingleton<T>.CurrentState == MonoSingletonState.InitializedInAwake;
        }

        /// <summary>
        /// Safely destroys a MonoBehaviour singleton.
        /// </summary>
        /// <typeparam name="T">The singleton type.</typeparam>
        /// <returns>True if destroyed successfully, false if already destroyed or not initialized.</returns>
        public static bool DestroySingleton<T>() where T : MonoSingleton<T>
        {
            if (!MonoSingleton<T>.IsInitialized)
                return false;

            if (MonoSingleton<T>.CurrentState == MonoSingletonState.Destroyed)
                return false;

            var instance = MonoSingleton<T>.Instance;
            if (instance != null && instance.gameObject != null)
            {
                Object.Destroy(instance.gameObject);
                return true;
            }

            return false;
        }
    }
}
