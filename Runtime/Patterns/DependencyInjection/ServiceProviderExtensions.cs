using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Extension methods for <see cref="IServiceResolver"/> providing generic typed APIs.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="resolver">The service provider.</param>
        /// <returns>A service object of type <typeparamref name="T"/>, or null if there is no such service.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="resolver"/> is null.</exception>
        public static T GetService<T>(this IServiceResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));

            return (T)resolver.GetService(typeof(T));
        }
    }
}
