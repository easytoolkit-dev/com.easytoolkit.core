using System;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Defines a read-only bindable value that can notify subscribers about value changes.
    /// </summary>
    /// <typeparam name="T">The type of the bindable value.</typeparam>
    public interface IReadonlyBindableValue<T>
    {
        /// <summary>
        /// Gets the current value.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Occurs when the value has changed.
        /// </summary>
        event EventHandler<BindableValueChangedEventArgs<T>> ValueChanged;
    }
}
