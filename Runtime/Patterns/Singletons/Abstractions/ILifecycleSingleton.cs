using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Defines the lifecycle contract for singleton instances.
    /// Provides hooks for initialization, dependency resolution, and shutdown.
    /// </summary>
    public interface ILifecycleSingleton : IDisposable
    {
        /// <summary>
        /// Called after singleton construction for initialization.
        /// </summary>
        void OnSingletonInitialize();

        /// <summary>
        /// Called during disposal for cleanup.
        /// </summary>
        void OnSingletonShutdown();
    }
}
