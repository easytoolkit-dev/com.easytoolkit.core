using System.Collections.Generic;
using UnityEngine;

namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Provides extension methods for random value generation operations.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Picks a random element from the read-only list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The read-only list to pick from.</param>
        /// <returns>A randomly selected element from the list.</returns>
        /// <remarks>
        /// Uses Unity's Random.Range to select a random index.
        /// Returns the element at the randomly chosen index.
        /// </remarks>
        public static T PickRandom<T>(this IReadOnlyList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Samples a random color from the gradient.
        /// </summary>
        /// <param name="gradient">The gradient to sample from.</param>
        /// <returns>A color evaluated at a random position between 0 and 1 on the gradient.</returns>
        /// <remarks>
        /// The sampling position is uniformly distributed across the entire gradient range [0, 1].
        /// </remarks>
        public static Color SampleRandom(this Gradient gradient)
        {
            return gradient.Evaluate(Random.Range(0f, 1f));
        }
    }
}
 