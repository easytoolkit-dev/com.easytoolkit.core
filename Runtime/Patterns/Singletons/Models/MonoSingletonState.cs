namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Represents the current state of a MonoBehaviour singleton.
    /// </summary>
    public enum MonoSingletonState
    {
        /// <summary>
        /// The singleton has not been initialized.
        /// </summary>
        NotInitialized,

        /// <summary>
        /// The singleton was created dynamically by the factory.
        /// </summary>
        CreatedViaFactory,

        /// <summary>
        /// The singleton was pre-existing in the scene and initialized in Awake.
        /// </summary>
        InitializedInAwake,

        /// <summary>
        /// The singleton has been destroyed by Unity.
        /// </summary>
        Destroyed
    }
}
