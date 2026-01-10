
using EasyToolKit.Core.Unity;
using UnityEngine;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Provides extension methods for configuring automatic event unregistration behavior.
    /// </summary>
    public static class RegistrationExtensions
    {
        /// <summary>
        /// Configures the registration to automatically unregister when the specified GameObject is destroyed.
        /// </summary>
        /// <typeparam name="TRegistration">The type of registration that implements <see cref="IRegistration"/>.</typeparam>
        /// <param name="registration">The registration to configure.</param>
        /// <param name="gameObject">The GameObject whose destruction triggers unregistration.</param>
        /// <returns>The same registration instance for fluent chaining.</returns>
        /// <remarks>
        /// This method adds an <see cref="UnregisterOnDestroyTrigger"/> component to the GameObject
        /// if one does not already exist. The registration will be automatically unregistered when
        /// the GameObject is destroyed, preventing memory leaks from lingering event subscriptions.
        /// </remarks>
        public static TRegistration UnregisterWhenGameObjectDestroy<TRegistration>(this TRegistration registration, GameObject gameObject)
            where TRegistration : IRegistration
        {
            var trigger = gameObject.GetOrAddComponent<UnregisterOnDestroyTrigger>();
            trigger.AddUnregister(registration);
            return registration;
        }

        /// <summary>
        /// Configures the registration to automatically unregister when the specified GameObject is disabled or destroyed.
        /// </summary>
        /// <typeparam name="TRegistration">The type of registration that implements <see cref="IRegistration"/>.</typeparam>
        /// <param name="registration">The registration to configure.</param>
        /// <param name="gameObject">The GameObject whose disable or destruction triggers unregistration.</param>
        /// <returns>The same registration instance for fluent chaining.</returns>
        /// <remarks>
        /// This method adds an <see cref="UnregisterOnDisableTrigger"/> component to the GameObject
        /// if one does not already exist. The registration will be automatically unregistered when
        /// the GameObject is disabled (via <see cref="GameObject.SetActive"/>) or destroyed,
        /// preventing memory leaks from lingering event subscriptions.
        /// </remarks>
        public static TRegistration UnregisterWhenGameObjectDisable<TRegistration>(this TRegistration registration, GameObject gameObject)
            where TRegistration : IRegistration
        {
            var trigger = gameObject.GetOrAddComponent<UnregisterOnDisableTrigger>();
            trigger.AddUnregister(registration);
            return registration;
        }
    }
}
