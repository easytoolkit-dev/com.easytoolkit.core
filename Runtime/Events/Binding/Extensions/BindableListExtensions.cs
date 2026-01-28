using System;

namespace EasyToolkit.Core.Events
{
    /// <summary>
    /// Extension methods for BindableList.
    /// </summary>
    public static class BindableListExtensions
    {
        /// <summary>
        /// Adds an item to the end of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to add the item to.</param>
        /// <param name="value">The item to add.</param>
        public static void Add<T>(this IBindableList<T> list, T value)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            list.Insert(list.Count, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific item from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to remove the item from.</param>
        /// <param name="value">The item to remove.</param>
        /// <returns><c>true</c> if the item was successfully removed; otherwise, <c>false</c>.</returns>
        public static bool Remove<T>(this IBindableList<T> list, T value)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            int index = list.IndexOf(value);
            if (index >= 0)
            {
                list.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}
