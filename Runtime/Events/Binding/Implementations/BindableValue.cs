using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// A value wrapper that raises events when the value changes.
    /// Supports cancellation of pending changes through Before event handlers.
    /// </summary>
    /// <typeparam name="T">The type of the bindable value.</typeparam>
    public class BindableValue<T> : IBindableValue<T>
    {
        private T _value;

        /// <summary>
        /// Initializes a new instance of the BindableValue class.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        public BindableValue(T defaultValue = default)
        {
            _value = defaultValue;
        }

        /// <inheritdoc />
        public T Value => _value;

        /// <inheritdoc />
        public event EventHandler<BindableValueChangedEventArgs<T>> ValueChanged;

        /// <inheritdoc />
        public void SetValue(T value)
        {
            if (value == null && _value == null) return;
            if (value != null && EqualityComparer<T>.Default.Equals(_value, value)) return;

            var oldValue = _value;

            // Raise before event and check for cancellation
            using var beforeArgs = BindableValueChangedEventArgs<T>.Create(oldValue, value, BindableValueChangedTiming.Before);
            OnValueChanged(beforeArgs);

            if (beforeArgs.Cancel)
            {
                // Change was cancelled, do not modify the value
                return;
            }

            SetValueWithoutEvent(value);

            // Raise after event
            using var afterArgs = BindableValueChangedEventArgs<T>.Create(oldValue, value, BindableValueChangedTiming.After);
            OnValueChanged(afterArgs);
        }

        /// <inheritdoc />
        public void SetValueWithoutEvent(T value)
        {
            _value = value;
        }

        private void OnValueChanged(BindableValueChangedEventArgs<T> args)
        {
            ValueChanged?.Invoke(this, args);
        }

        /// <inheritdoc />
        public override string ToString() => _value?.ToString() ?? string.Empty;
    }
}
