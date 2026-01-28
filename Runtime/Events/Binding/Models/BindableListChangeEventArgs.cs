using System;
using EasyToolkit.Core.Pooling;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Specifies the type of list change operation.
    /// </summary>
    public enum BindableListChangeType
    {
        /// <summary>
        /// An element is being inserted (includes both Add and Insert operations).
        /// </summary>
        Insert,

        /// <summary>
        /// An element is being removed from the list.
        /// </summary>
        Remove,

        /// <summary>
        /// An element is being replaced with a new value.
        /// </summary>
        Replace,

        /// <summary>
        /// The list is being cleared.
        /// </summary>
        Clear,
    }

    /// <summary>
    /// Specifies the timing of the list change event.
    /// </summary>
    public enum BindableListChangeTiming
    {
        /// <summary>
        /// The event is raised before the list is modified.
        /// </summary>
        Before,

        /// <summary>
        /// The event is raised after the list has been modified.
        /// </summary>
        After,
    }

    /// <summary>
    /// Provides data for the <see cref="IReadOnlyBindableList{T}.ItemChanged"/> event.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public sealed class BindableListChangeEventArgs<T> : EventArgs, IPoolItem, IDisposable
    {
        /// <summary>
        /// Gets the type of change operation.
        /// </summary>
        public BindableListChangeType ChangeType { get; private set; }

        /// <summary>
        /// Gets the timing of the event (before or after the change).
        /// </summary>
        public BindableListChangeTiming Timing { get; private set; }

        /// <summary>
        /// Gets the index affected by the operation.
        /// Returns -1 for operations that don't involve a specific index (e.g., Clear).
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the element affected by the operation.
        /// Returns default value for <see cref="BindableListChangeType.Clear"/> operations.
        /// For <see cref="BindableListChangeType.Replace"/> operations, this is the new element.
        /// </summary>
        public T Element { get; private set; }

        /// <summary>
        /// Gets the old element being replaced.
        /// Only valid for <see cref="BindableListChangeType.Replace"/> operations.
        /// </summary>
        public T OldElement { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list change should be cancelled.
        /// Only valid when <see cref="Timing"/> is <see cref="BindableListChangeTiming.Before"/>.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Creates a new instance for Insert operations (includes both Add and Insert).
        /// </summary>
        /// <param name="changeType">The type of change operation.</param>
        /// <param name="timing">The timing of the event.</param>
        /// <param name="index">The index affected by the operation.</param>
        /// <param name="element">The element affected by the operation.</param>
        /// <returns>A new or reused instance of <see cref="BindableListChangeEventArgs{T}"/>.</returns>
        public static BindableListChangeEventArgs<T> Create(BindableListChangeType changeType, BindableListChangeTiming timing, int index, T element)
        {
            var args = PoolUtility.RentObject<BindableListChangeEventArgs<T>>();
            args.ChangeType = changeType;
            args.Timing = timing;
            args.Index = index;
            args.Element = element;
            args.OldElement = default;
            args.Cancel = false;
            return args;
        }

        /// <summary>
        /// Creates a new instance for Replace operations.
        /// </summary>
        /// <param name="timing">The timing of the event.</param>
        /// <param name="index">The index of the element being replaced.</param>
        /// <param name="oldElement">The old element being replaced.</param>
        /// <param name="newElement">The new element.</param>
        /// <returns>A new or reused instance of <see cref="BindableListChangeEventArgs{T}"/>.</returns>
        public static BindableListChangeEventArgs<T> CreateReplace(BindableListChangeTiming timing, int index, T oldElement, T newElement)
        {
            var args = Create(BindableListChangeType.Replace, timing, index, newElement);
            args.OldElement = oldElement;
            return args;
        }

        /// <summary>
        /// Creates a new instance for Remove operations.
        /// </summary>
        /// <param name="timing">The timing of the event.</param>
        /// <param name="index">The index of the element being removed.</param>
        /// <param name="element">The element being removed.</param>
        /// <returns>A new or reused instance of <see cref="BindableListChangeEventArgs{T}"/>.</returns>
        public static BindableListChangeEventArgs<T> CreateRemove(BindableListChangeTiming timing, int index, T element)
        {
            return Create(BindableListChangeType.Remove, timing, index, element);
        }

        /// <summary>
        /// Creates a new instance for Clear operations.
        /// </summary>
        /// <param name="timing">The timing of the event.</param>
        /// <returns>A new or reused instance of <see cref="BindableListChangeEventArgs{T}"/>.</returns>
        public static BindableListChangeEventArgs<T> CreateClear(BindableListChangeTiming timing)
        {
            return Create(BindableListChangeType.Clear, timing, -1, default);
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
            ChangeType = default;
            Timing = default;
            Index = 0;
            Element = default;
            OldElement = default;
            Cancel = false;
        }
    }
}
