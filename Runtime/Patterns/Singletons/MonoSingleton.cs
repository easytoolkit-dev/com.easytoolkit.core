using EasyToolkit.Core.Threading;
using UnityEngine;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// MonoBehaviour singleton base class for Unity components.
    /// Automatically manages singleton lifetime and provides DontDestroyOnLoad support.
    /// </summary>
    /// <typeparam name="T">The singleton type, must derive from MonoSingleton{T}</typeparam>
    /// <remarks>
    /// Usage:
    /// <code>
    /// [MonoSingletonConfiguration(MonoSingletonFlags.DontDestroyOnLoad)]
    /// public class AudioManager : MonoSingleton&lt;AudioManager&gt;
    /// {
    ///     protected override void OnSingletonInit(SingletonInitialMode mode)
    ///     {
    ///         if (mode == SingletonInitialMode.Create)
    ///             Debug.Log("AudioManager created");
    ///     }
    /// }
    ///
    /// AudioManager.Instance.PlaySound("explosion");
    /// </code>
    /// </remarks>
    public abstract class MonoSingleton<T> : MonoBehaviour, IUnitySingleton
        where T : MonoSingleton<T>
    {
        private static T s_instance;
        private static MonoSingletonState s_state = MonoSingletonState.NotInitialized;
        private static readonly object StateLock = new();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <exception cref="SingletonDestroyedException">Thrown when accessing after destruction.</exception>
        public static T Instance
        {
            get
            {
                ValidateMainThreadAccess();

                if (s_state == MonoSingletonState.Destroyed)
                {
                    throw new SingletonDestroyedException(
                        $"[MonoSingleton] InstanceDestroyed: Cannot access '{typeof(T).Name}' after destruction. " +
                        $"Access occurred after Unity OnDestroy. Check 'IsInitialized' before accessing.",
                        typeof(T));
                }

                if (s_instance == null && s_state == MonoSingletonState.NotInitialized)
                {
                    s_instance = Implementations.MonoSingletonFactory.FindOrCreateMonoSingleton<T>();
                    s_state = MonoSingletonState.CreatedViaFactory;
                }

                return s_instance;
            }
        }

        /// <summary>
        /// Gets whether the singleton instance has been initialized and not destroyed.
        /// </summary>
        public static bool IsInitialized => s_instance != null && s_state != MonoSingletonState.Destroyed;

        /// <summary>
        /// Gets the current initialization state of the singleton.
        /// </summary>
        public static MonoSingletonState CurrentState => s_state;

        /// <summary>
        /// Unity Awake method called when the script instance is being loaded.
        /// </summary>
        protected virtual void Awake()
        {
            lock (StateLock)
            {
                switch (s_state)
                {
                    case MonoSingletonState.NotInitialized:
                        // First initialization - this instance is from the scene
                        s_instance = (T)this;
                        s_state = MonoSingletonState.InitializedInAwake;
                        Implementations.MonoSingletonFactory.ApplyConfiguration(this);
                        break;

                    case MonoSingletonState.CreatedViaFactory:
                        // Factory already created this instance
                        Implementations.MonoSingletonFactory.ApplyConfiguration(this);
                        break;

                    case MonoSingletonState.InitializedInAwake:
                        // Duplicate instance detected in scene
                        Debug.LogWarning(
                            $"[MonoSingleton] DuplicateInstance: Destroying duplicate '{typeof(T).Name}'. " +
                            $"Only one instance should exist in the scene. " +
                            $"Keeping the first instance.");
                        Destroy(this);
                        break;

                    case MonoSingletonState.Destroyed:
                        Debug.LogWarning(
                            $"[MonoSingleton] AccessAfterDestroy: '{typeof(T).Name}' is being accessed after destruction. " +
                            $"Resetting state.");
                        s_instance = (T)this;
                        s_state = MonoSingletonState.InitializedInAwake;
                        Implementations.MonoSingletonFactory.ApplyConfiguration(this);
                        break;
                }
            }
        }

        /// <summary>
        /// Unity OnDestroy method called when the script instance is being destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            lock (StateLock)
            {
                if (s_instance == this)
                {
                    s_instance = null;
                    s_state = MonoSingletonState.Destroyed;
                }
            }
        }

        /// <summary>
        /// Called when the singleton is initialized.
        /// Override to provide custom initialization.
        /// </summary>
        /// <param name="mode">Whether the instance was loaded from scene or created dynamically.</param>
        protected virtual void OnSingletonInitialize(SingletonInitialMode mode)
        {
        }

        /// <summary>
        /// Explicit interface implementation.
        /// </summary>
        void IUnitySingleton.OnSingletonInitialize(SingletonInitialMode mode) => OnSingletonInitialize(mode);

        /// <summary>
        /// Validates that access is from the main thread (editor only).
        /// </summary>
        private static void ValidateMainThreadAccess()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (UnityMainThreadDispatcher.Instance.MainThreadId != null)
                {
                    if (System.Threading.Thread.CurrentThread.ManagedThreadId != UnityMainThreadDispatcher.Instance.MainThreadId)
                    {
                        Debug.LogWarning(
                            $"[MonoSingleton] WrongThread: '{typeof(T).Name}' accessed from background thread. " +
                            $"MonoSingleton must be accessed from Unity's main thread.");
                    }
                }
            }
#endif
        }
    }
}
