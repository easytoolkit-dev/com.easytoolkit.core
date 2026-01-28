using System;

namespace EasyToolkit.Core.Collections
{
    /// <summary>
    /// Provides extension methods for array operations such as element removal and manipulation.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Creates a new array with the element at the specified index removed.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The source array.</param>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <returns>A new array with the specified element removed.</returns>
        /// <remarks>
        /// The original array is not modified. A new array is created with length one less than the original.
        /// </remarks>
        public static T[] WithRemoveAt<T>(this T[] array, int index)
        {
            if (index < 0 || index >= array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            T[] newArray = new T[array.Length - 1];

            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);

            return newArray;
        }
    }
}
