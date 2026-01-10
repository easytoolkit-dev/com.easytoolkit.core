namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Defines a bindable value that can notify subscribers about value changes and supports value modification.
    /// </summary>
    /// <typeparam name="T">The type of the bindable value.</typeparam>
    public interface IBindableValue<T> : IReadonlyBindableValue<T>
    {
        /// <summary>
        /// Sets the value and raises the ValueChanged event.
        /// </summary>
        /// <param name="value">The new value.</param>
        void SetValue(T value);

        /// <summary>
        /// Sets the value without raising the ValueChanged event.
        /// </summary>
        /// <param name="value">The new value.</param>
        void SetValueWithoutEvent(T value);
    }
}
