using System;
using UnityEngine;
using EasyToolkit.Core.Unity;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Provides extension methods for Unity GameObject to register lifecycle callbacks.
    /// </summary>
    public static class GameObjectCallbackExtensions
    {
        /// <summary>
        /// Registers a callback to be executed when the GameObject is destroyed.
        /// </summary>
        /// <param name="gameObject">The target GameObject.</param>
        /// <param name="callback">The callback to execute on destroy.</param>
        /// <returns>The trigger component for advanced configuration.</returns>
        /// <remarks>
        /// This method adds a <see cref="DestroyTrigger"/> component to the GameObject
        /// if one does not already exist. The callback will be automatically executed when
        /// the GameObject is destroyed, preventing memory leaks from lingering resources.
        /// Multiple callbacks can be registered on the same GameObject.
        /// </remarks>
        /// <example>
        /// <code>
        /// gameObject.OnDestroy(() => Debug.Log("Destroyed"));
        /// </code>
        /// </example>
        public static GameObject OnDestroy(
            this GameObject gameObject,
            Action callback)
        {
            var trigger = gameObject.GetOrAddComponent<DestroyTrigger>();
            trigger.AddCallback(callback);
            return gameObject;
        }

        /// <summary>
        /// Registers a callback to be executed when the GameObject is disabled.
        /// </summary>
        /// <param name="gameObject">The target GameObject.</param>
        /// <param name="callback">The callback to execute on disable.</param>
        /// <returns>The trigger component for advanced configuration.</returns>
        /// <remarks>
        /// This method adds a <see cref="DisableTrigger"/> component to the GameObject
        /// if one does not already exist. The callback will be automatically executed when
        /// the GameObject is disabled (via <see cref="GameObject.SetActive"/>), preventing memory leaks.
        /// Multiple callbacks can be registered on the same GameObject.
        /// </remarks>
        /// <example>
        /// <code>
        /// gameObject.OnDisable(() => Debug.Log("Disabled"));
        /// </code>
        /// </example>
        public static GameObject OnDisable(
            this GameObject gameObject,
            Action callback)
        {
            var trigger = gameObject.GetOrAddComponent<DisableTrigger>();
            trigger.AddCallback(callback);
            return gameObject;
        }
    }
}
