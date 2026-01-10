using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Defines a bindable list that can notify subscribers about list changes and supports modification operations.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public interface IBindableList<T> : IReadOnlyBindableList<T>
    {
        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        void Clear();

        /// <summary>
        /// Inserts an item into the list at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        void Insert(int index, T item);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>The zero-based index of the first occurrence of item within the list, if found; otherwise, -1.</returns>
        int IndexOf(T item);

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        void RemoveAt(int index);

        /// <summary>
        /// Determines whether the list contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>true if item is found in the list; otherwise, false.</returns>
        bool Contains(T item);

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        void SetItem(int index, T item);

        /// <summary>
        /// Sets the element at the specified index without raising events.
        /// </summary>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        void SetItemWithoutEvent(int index, T item);
    }
}
