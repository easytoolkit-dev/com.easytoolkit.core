using System;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Extension methods for <see cref="IServiceProvider"/> providing generic typed APIs.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The service provider.</param>
        /// <returns>A service object of type <typeparamref name="T"/>, or null if there is no such service.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> is null.</exception>
        public static T GetService<T>(this IServiceProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            return (T)provider.GetService(typeof(T));
        }

        /// <summary>
        /// Injects services into fields marked with <see cref="InjectAttribute"/> on an existing target object.
        /// </summary>
        /// <param name="provider">The service provider used to resolve dependencies.</param>
        /// <param name="target">The object that receives field injection.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="provider"/> or <paramref name="target"/> is null.
        /// </exception>
        /// <exception cref="DependencyResolutionException">
        /// Thrown when a marked field is invalid or its dependency cannot be resolved.
        /// </exception>
        public static void Inject(this IServiceProvider provider, object target)
        {
            ServiceInjector.Inject(provider, target);
        }
    }
}
