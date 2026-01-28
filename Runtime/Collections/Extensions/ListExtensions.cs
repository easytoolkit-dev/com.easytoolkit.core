using System;
using System.Collections.Generic;

namespace EasyToolkit.Core.Collections
{
    /// <summary>
    /// Provides extension methods for list and collection operations including conversion and resizing.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Converts a one-dimensional list to a two-dimensional array with the specified dimensions.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The source list to convert.</param>
        /// <param name="rows">The number of rows in the resulting 2D array.</param>
        /// <param name="columns">The number of columns in the resulting 2D array.</param>
        /// <returns>A two-dimensional array with the specified dimensions filled row by row from the source.</returns>
        /// <remarks>
        /// The source list must contain exactly <c>rows * columns</c> elements.
        /// Elements are filled row by row in row-major order.
        /// </remarks>
        public static T[,] To2dArray<T>(this IList<T> source, int rows, int columns)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source),
                    "Source list cannot be null. Provide a valid list to convert.");
            }

            if (rows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), rows,
                    "Row count cannot be negative. Provide a non-negative value for the number of rows.");
            }

            if (columns < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), columns,
                    "Column count cannot be negative. Provide a non-negative value for the number of columns.");
            }

            if (rows == 0 || columns == 0)
            {
                if (source.Count != 0)
                {
                    throw new ArgumentException(
                        $"Source list contains {source.Count} elements but cannot be converted to a 2D array with {rows} rows and {columns} columns. " +
                        $"Either provide a non-zero dimension or an empty source list.",
                        nameof(source));
                }

                return new T[rows, columns];
            }

            if (source.Count != rows * columns)
            {
                throw new ArgumentException(
                    $"Source list contains {source.Count} elements but requires exactly {rows * columns} elements for a {rows}x{columns} 2D array. " +
                    $"Adjust the source list size or the target dimensions.",
                    nameof(source));
            }

            var result = new T[rows, columns];

            for (int i = 0; i < source.Count; i++)
            {
                int row = i / columns;
                int col = i % columns;
                result[row, col] = source[i];
            }

            return result;
        }

        /// <summary>
        /// Converts a one-dimensional list to a jagged array with the specified dimensions.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The source list to convert.</param>
        /// <param name="rows">The number of rows in the resulting jagged array.</param>
        /// <param name="columns">The number of columns in each row of the resulting jagged array.</param>
        /// <returns>A jagged array with the specified dimensions filled row by row from the source.</returns>
        /// <remarks>
        /// The source list must contain exactly <c>rows * columns</c> elements.
        /// Each row in the resulting array is a separate array instance.
        /// </remarks>
        public static T[][] ToJaggedArray<T>(this IList<T> source, int rows, int columns)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source),
                    "Source list cannot be null. Provide a valid list to convert.");
            }

            if (rows < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows), rows,
                    "Row count cannot be negative. Provide a non-negative value for the number of rows.");
            }

            if (columns < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), columns,
                    "Column count cannot be negative. Provide a non-negative value for the number of columns.");
            }

            if (rows == 0 || columns == 0)
            {
                if (source.Count != 0)
                {
                    throw new ArgumentException(
                        $"Source list contains {source.Count} elements but cannot be converted to a jagged array with {rows} rows and {columns} columns. " +
                        $"Either provide a non-zero dimension or an empty source list.",
                        nameof(source));
                }

                return new T[rows][];
            }

            if (source.Count != rows * columns)
            {
                throw new ArgumentException(
                    $"Source list contains {source.Count} elements but requires exactly {rows * columns} elements for a {rows}x{columns} jagged array. " +
                    $"Adjust the source list size or the target dimensions.",
                    nameof(source));
            }

            var result = new T[rows][];

            for (int row = 0; row < rows; row++)
            {
                result[row] = new T[columns];
            }

            for (int i = 0; i < source.Count; i++)
            {
                int row = i / columns;
                int col = i % columns;
                result[row][col] = source[i];
            }

            return result;
        }

        /// <summary>
        /// Resizes the list to the specified length by adding default values or removing elements from the end.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to resize.</param>
        /// <param name="length">The target length of the list.</param>
        /// <remarks>
        /// When the target length is greater than the current count, default values are appended.
        /// When the target length is less than the current count, elements are removed from the end.
        /// This method cannot be used on arrays - use <see cref="Array.Resize{T}(ref T[], int)"/> instead.
        /// </remarks>
        public static void Resize<T>(this IList<T> list, int length)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list),
                    "List cannot be null. Provide a valid list to resize.");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length,
                    "Length cannot be negative. Provide a non-negative value for the target length.");
            }

            if (list.GetType().IsArray)
            {
                throw new InvalidOperationException(
                    "Cannot resize arrays using this extension method. Use Array.Resize<T>(ref T[] array, int newSize) instead for array resizing.");
            }

            while (list.Count < length)
            {
                list.Add(default);
            }

            while (list.Count > length)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
