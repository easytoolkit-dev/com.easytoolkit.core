using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyToolKit.Core.Collections
{
    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> sequences with common operations
    /// including validation, iteration, and element checking.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Determines whether all elements in the sequence are equal using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="enumerator">The sequence to check.</param>
        /// <returns><c>true</c> if all elements are equal or the sequence is empty; otherwise, <c>false</c>.</returns>
        public static bool IsAllSame<T>(this IEnumerable<T> enumerator)
        {
            return enumerator.IsAllSame((a, b) => EqualityComparer<T>.Default.Equals(a, b));
        }

        /// <summary>
        /// Determines whether all elements in the sequence are equal using a custom comparison function.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="enumerator">The sequence to check.</param>
        /// <param name="comparison">The function to compare two elements for equality.</param>
        /// <returns><c>true</c> if all elements are equal according to the comparison function, or the sequence is empty; otherwise, <c>false</c>.</returns>
        public static bool IsAllSame<T>(this IEnumerable<T> enumerator, Func<T, T, bool> comparison)
        {
            if (enumerator == null)
                throw new ArgumentNullException(nameof(enumerator));

            T first = default;
            bool hasFirst = false;
            foreach (var value in enumerator)
            {
                if (!hasFirst)
                {
                    first = value;
                    hasFirst = true;
                }
                else
                {
                    if (!comparison(first, value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Performs the specified action on each element of the sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="enumerator">The sequence to iterate.</param>
        /// <param name="callback">The action to perform on each element.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerator, Action<T> callback)
        {
            foreach (var item in enumerator)
            {
                callback(item);
            }
        }

        /// <summary>
        /// Determines whether the sequence contains any duplicate elements based on a key selector function.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key to check for duplicates.</typeparam>
        /// <param name="enumerator">The sequence to check.</param>
        /// <param name="selector">The function to extract the key for each element.</param>
        /// <returns><c>true</c> if any duplicate keys are found; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method uses a HashSet for O(1) lookups, providing O(n) overall time complexity.
        /// </remarks>
        public static bool HasDuplicate<T, TKey>(this IEnumerable<T> enumerator, Func<T, TKey> selector)
        {
            var set = new HashSet<TKey>();

            foreach (var e in enumerator)
            {
                if (!set.Add(selector(e)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the sequence contains any duplicate elements using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="enumerator">The sequence to check.</param>
        /// <returns><c>true</c> if any duplicate elements are found; otherwise, <c>false</c>.</returns>
        public static bool HasDuplicate<T>(this IEnumerable<T> enumerator)
        {
            return enumerator.HasDuplicate(item => item);
        }

        /// <summary>
        /// Determines whether the sequence is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence to check.</param>
        /// <returns><c>true</c> if the sequence is null or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Determines whether the sequence is not null and contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence to check.</param>
        /// <returns><c>true</c> if the sequence is not null and contains at least one element; otherwise, <c>false</c>.</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return !source.IsNullOrEmpty();
        }
    }
}
