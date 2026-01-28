using System;
using System.Threading;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Thread-safe singleton base class for standard C# classes.
    /// Uses Lazy{T} for thread-safe lazy initialization.
    /// </summary>
    /// <typeparam name="T">The singleton type, must derive from Singleton{T}</typeparam>
    /// <remarks>
    /// Usage:
    /// <code>
    /// public class GameManager : Singleton&lt;GameManager&gt;
    /// {
    ///     protected override void OnSingletonInitialize()
    ///     {
    ///         // Initialize your game manager here
    ///     }
    /// }
    ///
    /// GameManager.Instance.DoSomething();
    /// </code>
    /// </remarks>
    public abstract class Singleton<T> : ILifecycleSingleton where T : Singleton<T>
    {
        private static readonly Lazy<T> LazyInstance;
        private static readonly object InitLock = new();
        private static readonly ISingletonRegistry Registry = new Implementations.SingletonRegistry();

        static Singleton()
        {
            LazyInstance = new Lazy<T>(() =>
            {
                var instance = Implementations.SingletonFactory.Create<T>();

                lock (InitLock)
                {
                    instance.OnSingletonInitialize();
                    Registry.Register(instance, typeof(T));
                }

                return instance;
            }, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Gets the singleton instance, creating it on first access.
        /// </summary>
        public static T Instance => LazyInstance.Value;

        /// <summary>
        /// Gets whether the singleton instance has been created.
        /// </summary>
        public static bool IsInitialized => LazyInstance.IsValueCreated;

        /// <summary>
        /// Called after singleton construction for initialization.
        /// Override to provide custom initialization logic.
        /// </summary>
        protected virtual void OnSingletonInitialize()
        {
        }

        /// <summary>
        /// Called during disposal for cleanup.
        /// Override to perform cleanup operations.
        /// </summary>
        protected virtual void OnSingletonShutdown()
        {
        }

        void ILifecycleSingleton.OnSingletonInitialize() => OnSingletonInitialize();

        void ILifecycleSingleton.OnSingletonShutdown() => OnSingletonShutdown();

        void IDisposable.Dispose()
        {
            if (LazyInstance.IsValueCreated)
            {
                var instance = LazyInstance.Value;
                instance.OnSingletonShutdown();
                Registry.Unregister(instance);
            }
        }
    }
}
