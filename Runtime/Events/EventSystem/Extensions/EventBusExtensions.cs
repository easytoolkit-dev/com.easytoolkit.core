using System;
using System.Reflection;
using UnityEngine;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Extension methods for the event bus providing Unity integration and fluent API.
    /// </summary>
    public static class EventBusExtensions
    {
        /// <summary>
        /// Subscribes a method by name using reflection.
        /// The method must accept a single parameter of the specified event type.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of event to subscribe to.</typeparam>
        /// <param name="eventBus">The event bus instance.</param>
        /// <param name="component">The Component containing the method.</param>
        /// <param name="methodName">The name of the method to subscribe.</param>
        /// <returns>A subscription token that can be used to unsubscribe.</returns>
        /// <exception cref="ArgumentNullException">Thrown when component or methodName is null.</exception>
        /// <exception cref="MissingMethodException">Thrown when the method is not found or has an invalid signature.</exception>
        public static IEventSubscription SubscribeFromMethod<TEventArgs>(this IEventBus eventBus, Component component,
            string methodName)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component));
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            var method = component.GetType().GetMethod(methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                throw new MissingMethodException(
                    $"Method '{methodName}' not found in type '{component.GetType().Name}'.");

            var parameters = method.GetParameters();
            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(TEventArgs))
                throw new MissingMethodException(
                    $"Method '{methodName}' must have a single parameter of type '{typeof(TEventArgs).Name}'.");

            Action<TEventArgs> handler = eventArgs =>
            {
                try
                {
                    method.Invoke(component, new object[] { eventArgs });
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"[EventBusExtensions] MethodInvokeFailed: Method '{methodName}' on '{component.GetType().Name}' threw exception: {ex.Message}");
                }
            };

            return eventBus.Subscribe(handler);
        }
    }
}
