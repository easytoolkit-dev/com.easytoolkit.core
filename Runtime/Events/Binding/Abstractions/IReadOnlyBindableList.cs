using System;
using System.Collections.Generic;

namespace EasyToolKit.Core.Events
{
    /// <summary>
    /// Defines a read-only bindable list that can notify subscribers about list changes.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public interface IReadOnlyBindableList<T> : IReadOnlyList<T>
    {
        /// <summary>
        /// Occurs when an item in the list is changed, added, or removed.
        /// </summary>
        event EventHandler<BindableListChangeEventArgs<T>> ItemChanged;
    }
}
