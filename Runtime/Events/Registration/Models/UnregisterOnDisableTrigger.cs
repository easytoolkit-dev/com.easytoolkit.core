using UnityEngine;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Triggers automatic event unregistration when the GameObject is disabled.
    /// </summary>
    /// <remarks>
    /// This component is typically added via <see cref="RegistrationExtensions.UnregisterWhenGameObjectDisable{TRegistration}"/>
    /// to ensure event handlers are automatically cleaned up when the associated GameObject is disabled.
    /// This is useful for components that need to unsubscribe when deactivated
    /// (e.g., when switching scenes or disabling objects), preventing memory leaks from lingering subscriptions.
    /// </remarks>
    public class UnregisterOnDisableTrigger : UnregisterTrigger
    {
        /// <summary>
        /// Called by Unity when the GameObject is disabled.
        /// </summary>
        /// <remarks>
        /// This method unregisters all event registrations managed by this trigger.
        /// This handles cases where the GameObject is deactivated via <see cref="GameObject.SetActive(bool)"/>.
        /// </remarks>
        private void OnDisable()
        {
            Unregister();
        }
    }
}
