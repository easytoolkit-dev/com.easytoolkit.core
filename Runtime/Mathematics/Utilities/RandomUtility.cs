using System;
using EasyToolkit.Core.ThirdParty.xxHash;

namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Provides deterministic random number generation utilities using hash-based algorithms.
    /// </summary>
    /// <remarks>
    /// Unlike standard random number generators that produce different sequences each time,
    /// these utilities generate consistent values for the same inputs, making them ideal for
    /// procedural generation that needs to be reproducible.
    /// </remarks>
    public static class RandomUtility
    {
        /// <summary>
        /// Generates a deterministic random integer within the specified range based on a string key.
        /// </summary>
        /// <param name="key">The string key used to seed the hash calculation. Same key always produces same result.</param>
        /// <param name="minInclusive">The inclusive lower bound of the range.</param>
        /// <param name="maxExclusive">The exclusive upper bound of the range (must be greater than minInclusive).</param>
        /// <param name="seed">An optional additional seed value to vary results for the same key.</param>
        /// <returns>A deterministic random integer between minInclusive (inclusive) and maxExclusive (exclusive).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when maxExclusive is less than or equal to minInclusive.</exception>
        /// <remarks>
        /// This method uses xxHash32 to compute a hash of the key string, then maps the hash value
        /// to the specified range. The same combination of key and seed will always produce the same result.
        /// </remarks>
        public static int DeterministicRange(string key, int minInclusive, int maxExclusive, uint seed = 0)
        {
            if (maxExclusive <= minInclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(maxExclusive),
                    $"maxExclusive '{maxExclusive}' must be greater than minInclusive '{minInclusive}'");
            }

            uint hash = xxHash32.ComputeHash(key, seed);
            return (int)(hash % (uint)(maxExclusive - minInclusive)) + minInclusive;
        }
    }
}
