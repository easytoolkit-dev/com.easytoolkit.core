using UnityEngine;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Triggers callback execution when the GameObject is destroyed.
    /// </summary>
    /// <remarks>
    /// This component is typically added via <see cref="GameObjectCallbackExtensions.OnDestroy"/>
    /// to ensure cleanup callbacks are automatically executed when the associated GameObject is destroyed,
    /// preventing memory leaks from lingering resources.
    /// </remarks>
    public sealed class DestroyTrigger : CallbackTrigger
    {
        /// <summary>
        /// Called by Unity when the GameObject is destroyed.
        /// </summary>
        /// <remarks>
        /// This method executes all managed callbacks and clears the internal collection.
        /// </remarks>
        private void OnDestroy()
        {
            ExecuteCallbacks();
        }
    }
}
