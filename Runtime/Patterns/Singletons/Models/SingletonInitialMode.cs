namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Defines how a Unity singleton was initialized.
    /// </summary>
    public enum SingletonInitialMode
    {
        /// <summary>
        /// Singleton was loaded from an existing scene object.
        /// </summary>
        Load,

        /// <summary>
        /// Singleton was created dynamically at runtime.
        /// </summary>
        Create
    }
}
