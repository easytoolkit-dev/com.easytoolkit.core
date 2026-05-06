using UnityEngine;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Triggers callback execution when the GameObject is disabled.
    /// </summary>
    /// <remarks>
    /// This component is typically added via <see cref="GameObjectCallbackExtensions.OnDisable"/>
    /// to ensure cleanup callbacks are automatically executed when the associated GameObject is disabled.
    /// This is useful for components that need to cleanup when deactivated
    /// (e.g., when switching scenes or disabling objects), preventing memory leaks from lingering resources.
    /// </remarks>
    public sealed class DisableTrigger : CallbackTrigger
    {
        /// <summary>
        /// Called by Unity when the GameObject is disabled.
        /// </summary>
        /// <remarks>
        /// This method executes all managed callbacks and clears the internal collection.
        /// This handles cases where the GameObject is deactivated via <see cref="GameObject.SetActive(bool)"/>.
        /// </remarks>
        private void OnDisable()
        {
            ExecuteCallbacks();
        }
    }
}
