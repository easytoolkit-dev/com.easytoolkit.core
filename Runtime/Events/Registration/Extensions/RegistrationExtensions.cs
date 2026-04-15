
using EasyToolkit.Core.Unity;
using UnityEngine;

namespace EasyToolkit.Core.Events
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
        public static TRegistration UnregisterWhenGameObjectDestroy<TRegistration>(this TRegistration registration, GameObject gameObject)
            where TRegistration : IRegistration
        {
            gameObject.OnDestroy(() => registration.Unregister());
            return registration;
        }

        /// <summary>
        /// Configures the registration to automatically unregister when the specified GameObject is disabled or destroyed.
        /// </summary>
        /// <typeparam name="TRegistration">The type of registration that implements <see cref="IRegistration"/>.</typeparam>
        /// <param name="registration">The registration to configure.</param>
        /// <param name="gameObject">The GameObject whose disable or destruction triggers unregistration.</param>
        /// <returns>The same registration instance for fluent chaining.</returns>
        public static TRegistration UnregisterWhenGameObjectDisable<TRegistration>(this TRegistration registration, GameObject gameObject)
            where TRegistration : IRegistration
        {
            gameObject.OnDisable(() => registration.Unregister());
            return registration;
        }
    }
}
