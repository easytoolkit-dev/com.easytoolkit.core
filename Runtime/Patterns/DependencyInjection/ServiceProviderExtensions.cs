using System;
using System.Linq;
using System.Reflection;

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
        /// Creates an instance of the specified implementation type and resolves constructor parameters from the provider.
        /// </summary>
        /// <param name="provider">The service provider used to resolve constructor dependencies.</param>
        /// <param name="implementationType">The concrete type to instantiate.</param>
        /// <returns>A new object created from <paramref name="implementationType"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="provider"/> or <paramref name="implementationType"/> is null.
        /// </exception>
        /// <exception cref="DependencyResolutionException">
        /// Thrown when the type cannot be constructed or one of its constructor dependencies cannot be resolved.
        /// </exception>
        public static object CreateInstance(this IServiceProvider provider, Type implementationType)
        {
            return ServiceActivator.CreateInstance(provider, implementationType);
        }

        /// <summary>
        /// Creates an instance of the specified implementation type and resolves constructor parameters from the provider.
        /// </summary>
        /// <typeparam name="T">The concrete type to instantiate.</typeparam>
        /// <param name="provider">The service provider used to resolve constructor dependencies.</param>
        /// <returns>A new object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> is null.</exception>
        /// <exception cref="DependencyResolutionException">
        /// Thrown when the type cannot be constructed or one of its constructor dependencies cannot be resolved.
        /// </exception>
        public static T CreateInstance<T>(this IServiceProvider provider)
        {
            return (T)provider.CreateInstance(typeof(T));
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
