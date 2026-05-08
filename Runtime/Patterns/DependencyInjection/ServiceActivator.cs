using System;
using System.Linq;
using System.Reflection;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Creates objects by resolving constructor parameters from a service provider.
    /// </summary>
    internal static class ServiceActivator
    {
        /// <summary>
        /// Creates an instance of the specified implementation type.
        /// </summary>
        public static object CreateInstance(IServiceProvider provider, Type implementationType)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            var constructor = SelectConstructor(implementationType);
            var parameters = constructor.GetParameters();
            return InvokeConstructor(provider, constructor, parameters);
        }

        /// <summary>
        /// Selects the public constructor used for constructor injection.
        /// </summary>
        public static ConstructorInfo SelectConstructor(Type implementationType)
        {
            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            if (implementationType.IsAbstract || implementationType.IsInterface)
            {
                throw new DependencyResolutionException(
                    $"Type {implementationType.FullName} must be a concrete type to create an instance.");
            }

            var constructors = implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0)
                throw new DependencyResolutionException($"No public constructors found for type {implementationType.FullName}.");

            return constructors.OrderByDescending(c => c.GetParameters().Length).First();
        }

        /// <summary>
        /// Invokes the selected constructor with dependencies resolved from the provider.
        /// </summary>
        public static object InvokeConstructor(
            IServiceProvider provider,
            ConstructorInfo constructor,
            ParameterInfo[] parameters)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var arguments = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                arguments[i] = ResolveParameter(provider, parameters[i]);
            }

            return constructor.Invoke(arguments);
        }

        /// <summary>
        /// Builds a reusable constructor factory.
        /// </summary>
        public static Func<IServiceProvider, object> BuildFactory(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            if (constructor == null)
                throw new ArgumentNullException(nameof(constructor));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            return provider => InvokeConstructor(provider, constructor, parameters);
        }

        private static object ResolveParameter(IServiceProvider provider, ParameterInfo parameter)
        {
            return provider.GetService(parameter.ParameterType) ??
                   throw new DependencyResolutionException(
                       $"Unable to resolve parameter '{parameter.Name}' of type '{parameter.ParameterType.FullName}'.");
        }
    }
}