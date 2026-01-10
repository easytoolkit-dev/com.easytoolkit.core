using System;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Default implementation of event registration that manages unsubscription logic.
    /// </summary>
    public class Registration : IRegistration
    {
        private readonly Action _unregister;

        /// <inheritdoc />
        public bool IsActive { get; private set; }

        /// <summary>
        /// Initializes a new instance with the specified unregistration action.
        /// </summary>
        /// <param name="unregister">The action to execute when unregistering the event handler.</param>
        public Registration(Action unregister)
        {
            _unregister = unregister;
            IsActive = true;
        }

        /// <summary>
        /// Unsubscribes the event handler by invoking the registered unregistration action.
        /// </summary>
        /// <remarks>
        /// This method is idempotent - calling it multiple times has no additional effect
        /// beyond the first call. After unregistration, <see cref="IsActive"/> returns false.
        /// </remarks>
        public void Unregister()
        {
            if (!IsActive)
                return;

            _unregister?.Invoke();
            IsActive = false;
        }
    }
}
