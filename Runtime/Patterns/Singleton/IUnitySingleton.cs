namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Defines the Unity-specific singleton contract with initialization mode tracking.
    /// </summary>
    public interface IUnitySingleton
    {
        /// <summary>
        /// Called when the singleton is initialized.
        /// </summary>
        /// <param name="mode">Whether the instance was loaded from scene or created dynamically.</param>
        void OnSingletonInitialize(SingletonInitialMode mode);
    }
}
