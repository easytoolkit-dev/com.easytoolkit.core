namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Receives a callback after dependency injection has completed successfully.
    /// </summary>
    public interface IInjected
    {
        /// <summary>
        /// Called after all injectable members on the instance have been assigned.
        /// </summary>
        void OnInjected();
    }
}
