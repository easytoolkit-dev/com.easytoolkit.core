using UnityEngine;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Triggers automatic event unregistration when the GameObject is destroyed.
    /// </summary>
    /// <remarks>
    /// This component is typically added via <see cref="RegistrationExtensions.UnregisterWhenGameObjectDestroy{TRegistration}"/>
    /// to ensure event handlers are automatically cleaned up when the associated GameObject is destroyed,
    /// preventing memory leaks from lingering subscriptions.
    /// </remarks>
    public class UnregisterOnDestroyTrigger : UnregisterTrigger
    {
        /// <summary>
        /// Called by Unity when the GameObject is destroyed.
        /// </summary>
        /// <remarks>
        /// This method unregisters all event registrations managed by this trigger.
        /// </remarks>
        private void OnDestroy()
        {
            Unregister();
        }
    }
}
