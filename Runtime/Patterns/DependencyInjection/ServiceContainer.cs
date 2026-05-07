using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// Implementation of a dependency injection container.
    /// </summary>
    public sealed class ServiceContainer : IServiceContainer
    {
        private readonly ServiceRegistry _registry;
        private readonly Dictionary<Type, object> _singletonInstances = new();
        private readonly Dictionary<Type, object> _scopedInstances = new();
        private readonly object _syncLock = new();
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ServiceContainer(IEnumerable<ServiceDescriptor> descriptors)
        {
            _registry = new ServiceRegistry();

            foreach (var descriptor in descriptors ?? Enumerable.Empty<ServiceDescriptor>())
            {
                _registry.Add(descriptor);
            }
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (serviceType == typeof(IServiceContainer) ||
                serviceType == typeof(IServiceProvider))
            {
                return this;
            }

            var descriptor = _registry.GetDescriptor(serviceType);
            if (descriptor == null)
                return null;

            return ResolveInstance(descriptor, this);
        }

        /// <summary>
        /// Creates a new service scope.
        /// </summary>
        public IServiceScope CreateScope()
        {
            return new ServiceScope(this);
        }

        private object ResolveInstance(ServiceDescriptor descriptor, IServiceProvider provider)
        {
            switch (descriptor.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    return ResolveSingleton(descriptor, provider);
                case ServiceLifetime.Scoped:
                    return ResolveScoped(descriptor, provider);
                case ServiceLifetime.Transient:
                    return CreateInstance(descriptor, provider);
                default:
                    throw new DependencyResolutionException($"Unsupported lifetime: {descriptor.Lifetime}");
            }
        }

        private object ResolveSingleton(ServiceDescriptor descriptor, IServiceProvider provider)
        {
            lock (_syncLock)
            {
                if (_singletonInstances.TryGetValue(descriptor.ServiceType, out var instance))
                    return instance;

                instance = CreateInstance(descriptor, provider);
                _singletonInstances[descriptor.ServiceType] = instance;
                descriptor.ImplementationInstance = instance;
                return instance;
            }
        }

        private object ResolveScoped(ServiceDescriptor descriptor, IServiceProvider provider)
        {
            if (_scopedInstances.TryGetValue(descriptor.ServiceType, out var instance))
                return instance;

            instance = CreateInstance(descriptor, provider);
            _scopedInstances[descriptor.ServiceType] = instance;
            return instance;
        }

        private object CreateInstance(ServiceDescriptor descriptor, IServiceProvider provider)
        {
            if (descriptor.ImplementationInstance != null)
                return descriptor.ImplementationInstance;

            if (descriptor.ImplementationFactory != null)
                return descriptor.ImplementationFactory(provider);

            var type = descriptor.ImplementationType ?? descriptor.ServiceType;

            var cachedFactory = _registry.GetCachedFactory(type);
            if (cachedFactory != null)
                return cachedFactory(provider);

            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0)
                throw new DependencyResolutionException($"No public constructors found for type {type.FullName}");

            var constructor = constructors.OrderByDescending(c => c.GetParameters().Length).First();
            var parameters = constructor.GetParameters();
            var arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                arguments[i] = ResolveParameter(provider, parameter);
            }

            var instance = constructor.Invoke(arguments);

            var factory = BuildFactory(constructor, parameters);
            _registry.CacheFactory(type, factory);

            return instance;
        }

        private object ResolveParameter(IServiceProvider provider, ParameterInfo parameter)
        {
            return provider.GetService(parameter.ParameterType) ??
                   throw new DependencyResolutionException(
                       $"Unable to resolve parameter '{parameter.Name}' of type '{parameter.ParameterType.FullName}'");
        }

        private Func<IServiceProvider, object> BuildFactory(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            return provider =>
            {
                var arguments = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    arguments[i] = ResolveParameter(provider, parameters[i]);
                }

                return constructor.Invoke(arguments);
            };
        }

        /// <summary>
        /// Cleans up resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (_disposed)
                return;

            lock (_syncLock)
            {
                foreach (var instance in _singletonInstances.Values)
                {
                    if (instance is IDisposable disposable)
                        disposable.Dispose();
                }

                _singletonInstances.Clear();
                _scopedInstances.Clear();
                _disposed = true;
            }
        }

        /// <summary>
        /// Service scope implementation.
        /// </summary>
        private sealed class ServiceScope : IServiceScope
        {
            private readonly ServiceContainer _parent;
            private readonly Dictionary<Type, object> _scopedInstances = new Dictionary<Type, object>();
            private bool _disposed;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public ServiceScope(ServiceContainer parent)
            {
                _parent = parent ?? throw new ArgumentNullException(nameof(parent));
                ServiceProvider = new ScopedServiceProvider(this);
            }

            /// <summary>
            /// The service provider for this scope.
            /// </summary>
            public IServiceProvider ServiceProvider { get; }

            /// <summary>
            /// Gets a service instance from the scope.
            /// </summary>
            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IServiceScope))
                {
                    return this;
                }

                if (serviceType == typeof(IServiceProvider))
                {
                    return ServiceProvider;
                }

                var descriptor = _parent._registry.GetDescriptor(serviceType);
                if (descriptor == null)
                    return null;

                if (descriptor.Lifetime == ServiceLifetime.Scoped)
                {
                    if (_scopedInstances.TryGetValue(serviceType, out var instance))
                        return instance;

                    instance = _parent.CreateInstance(descriptor, ServiceProvider);
                    _scopedInstances[serviceType] = instance;
                    return instance;
                }

                return _parent.ResolveInstance(descriptor, ServiceProvider);
            }

            /// <summary>
            /// Disposes the scope and its scoped instances.
            /// </summary>
            void IDisposable.Dispose()
            {
                if (_disposed)
                    return;

                foreach (var instance in _scopedInstances.Values)
                {
                    if (instance is IDisposable disposable)
                        disposable.Dispose();
                }

                _scopedInstances.Clear();
                _disposed = true;
            }

            /// <summary>
            /// Scoped service provider implementation.
            /// </summary>
            private sealed class ScopedServiceProvider : IServiceProvider
            {
                private readonly ServiceScope _scope;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                public ScopedServiceProvider(ServiceScope scope)
                {
                    _scope = scope;
                }

                /// <summary>
                /// Gets the service object of the specified type.
                /// </summary>
                public object GetService(Type serviceType)
                {
                    return _scope.GetService(serviceType);
                }
            }
        }
    }
}
