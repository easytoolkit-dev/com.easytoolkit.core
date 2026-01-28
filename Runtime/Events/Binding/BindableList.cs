using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// A list implementation that raises events when items are added, removed, or modified.
    /// Supports cancellation of pending changes through Before event handlers.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class BindableList<T> : IBindableList<T>
    {
        private readonly List<T> _list;

        /// <summary>
        /// Initializes a new instance of the BindableList class.
        /// </summary>
        public BindableList()
        {
            _list = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the BindableList class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the list.</param>
        public BindableList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <inheritdoc />
        public int Count => _list.Count;

        /// <inheritdoc />
        public event EventHandler<BindableListChangeEventArgs<T>> ItemChanged;

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Clear()
        {
            using var beforeArgs = BindableListChangeEventArgs<T>.CreateClear(BindableListChangeTiming.Before);
            OnItemChanged(beforeArgs);

            if (beforeArgs.Cancel)
                return;

            _list.Clear();
            using var afterArgs = BindableListChangeEventArgs<T>.CreateClear(BindableListChangeTiming.After);
            OnItemChanged(afterArgs);
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            using var beforeArgs = BindableListChangeEventArgs<T>.Create(BindableListChangeType.Insert, BindableListChangeTiming.Before, index, item);
            OnItemChanged(beforeArgs);

            if (beforeArgs.Cancel)
                return;

            _list.Insert(index, item);
            using var afterArgs = BindableListChangeEventArgs<T>.Create(BindableListChangeType.Insert, BindableListChangeTiming.After, index, item);
            OnItemChanged(afterArgs);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            var item = _list[index];
            using var beforeArgs = BindableListChangeEventArgs<T>.CreateRemove(BindableListChangeTiming.Before, index, item);
            OnItemChanged(beforeArgs);

            if (beforeArgs.Cancel)
                return;

            _list.RemoveAt(index);
            using var afterArgs = BindableListChangeEventArgs<T>.CreateRemove(BindableListChangeTiming.After, index, item);
            OnItemChanged(afterArgs);
        }

        /// <inheritdoc />
        public void SetItem(int index, T item)
        {
            var oldItem = _list[index];
            using var beforeArgs = BindableListChangeEventArgs<T>.CreateReplace(BindableListChangeTiming.Before, index, oldItem, item);
            OnItemChanged(beforeArgs);

            if (beforeArgs.Cancel)
                return;

            _list[index] = item;
            using var afterArgs = BindableListChangeEventArgs<T>.CreateReplace(BindableListChangeTiming.After, index, oldItem, item);
            OnItemChanged(afterArgs);
        }

        /// <inheritdoc />
        public void SetItemWithoutEvent(int index, T item)
        {
            _list[index] = item;
        }

        /// <inheritdoc />
        public T this[int index] => _list[index];

        private void OnItemChanged(BindableListChangeEventArgs<T> args) => ItemChanged?.Invoke(this, args);
    }
}
