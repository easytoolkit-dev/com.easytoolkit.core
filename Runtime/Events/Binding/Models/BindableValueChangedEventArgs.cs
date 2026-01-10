using System;
using EasyToolKit.Core.Pooling;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Specifies the timing of the value changed event.
    /// </summary>
    public enum BindableValueChangedTiming
    {
        /// <summary>
        /// The event is raised before the value is changed.
        /// </summary>
        Before,

        /// <summary>
        /// The event is raised after the value has been changed.
        /// </summary>
        After,
    }

    /// <summary>
    /// Provides data for the <see cref="IReadonlyBindableValue{T}.ValueChanged"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the bindable value.</typeparam>
    public sealed class BindableValueChangedEventArgs<T> : EventArgs, IPoolItem, IDisposable
    {
        /// <summary>
        /// Gets the previous value.
        /// </summary>
        public T OldValue { get; private set; }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public T NewValue { get; private set; }

        /// <summary>
        /// Gets the timing of the event (before or after the change).
        /// </summary>
        public BindableValueChangedTiming Timing { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value change should be cancelled.
        /// Only valid when <see cref="Timing"/> is <see cref="BindableValueChangedTiming.Before"/>.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="BindableValueChangedEventArgs{T}"/> class from the object pool.
        /// </summary>
        /// <param name="oldValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="timing">The timing of the event.</param>
        /// <returns>A new or reused instance of <see cref="BindableValueChangedEventArgs{T}"/>.</returns>
        public static BindableValueChangedEventArgs<T> Create(T oldValue, T newValue, BindableValueChangedTiming timing)
        {
            var args = PoolUtility.RentObject<BindableValueChangedEventArgs<T>>();
            args.OldValue = oldValue;
            args.NewValue = newValue;
            args.Timing = timing;
            args.Cancel = false;
            return args;
        }

        /// <summary>
        /// Releases the instance back to the object pool.
        /// </summary>
        public void Dispose()
        {
            PoolUtility.ReleaseObject(this);
        }

        void IPoolItem.Rent()
        {
        }

        void IPoolItem.Release()
        {
            OldValue = default;
            NewValue = default;
            Timing = default;
            Cancel = false;
        }
    }
}
