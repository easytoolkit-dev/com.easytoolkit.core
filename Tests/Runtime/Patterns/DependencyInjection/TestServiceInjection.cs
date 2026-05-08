using System;
using EasyToolkit.Core.Patterns;
using NUnit.Framework;

#pragma warning disable 0649
#pragma warning disable 0169

namespace EasyToolkit.Core.Patterns.Tests
{
    /// <summary>
    /// Unit tests for dependency injection field injection.
    /// </summary>
    [TestFixture]
    public class TestServiceInjection
    {
        #region Creation Tests

        /// <summary>
        /// Verifies that service creation does not inject private fields marked with InjectAttribute.
        /// </summary>
        [Test]
        public void GetService_MarkedPrivateField_DoesNotInjectDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Transient<FieldInjectedService, FieldInjectedService>());

            // Act
            var service = container.GetService<FieldInjectedService>();

            // Assert
            Assert.IsNull(service.Dependency);
        }

        /// <summary>
        /// Verifies that cached constructor factories do not inject marked fields.
        /// </summary>
        [Test]
        public void GetService_CachedFactory_DoesNotInjectDependencyForEveryInstance()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Transient<FieldInjectedService, FieldInjectedService>());

            // Act
            var first = container.GetService<FieldInjectedService>();
            var second = container.GetService<FieldInjectedService>();

            // Assert
            Assert.AreNotSame(first, second);
            Assert.IsNull(first.Dependency);
            Assert.IsNull(second.Dependency);
        }

        /// <summary>
        /// Verifies that inherited private marked fields are not injected during service creation.
        /// </summary>
        [Test]
        public void GetService_InheritedMarkedField_DoesNotInjectDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Transient<DerivedInjectedService, DerivedInjectedService>());

            // Act
            var service = container.GetService<DerivedInjectedService>();

            // Assert
            Assert.IsNull(service.BaseDependency);
        }

        /// <summary>
        /// Verifies that factory-created services do not receive field injection during service creation.
        /// </summary>
        [Test]
        public void GetService_FactoryCreatedService_DoesNotInjectDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Transient(_ => new FieldInjectedService()));

            // Act
            var service = container.GetService<FieldInjectedService>();

            // Assert
            Assert.IsNull(service.Dependency);
        }

        /// <summary>
        /// Verifies that existing singleton instances do not receive field injection on first resolution.
        /// </summary>
        [Test]
        public void GetService_ExistingSingletonInstance_DoesNotInjectDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var service = new FieldInjectedService();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Singleton(service));

            // Act
            var resolved = container.GetService<FieldInjectedService>();

            // Assert
            Assert.AreSame(service, resolved);
            Assert.IsNull(resolved.Dependency);
        }

        #endregion

        #region Instance Creation Tests

        /// <summary>
        /// Verifies that unregistered concrete objects can be created with constructor injection.
        /// </summary>
        [Test]
        public void CreateInstance_UnregisteredType_InjectsConstructorDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(ServiceDescriptor.Singleton<ITestDependency>(dependency));

            // Act
            var service = container.CreateInstance<ConstructorInjectedService>();

            // Assert
            Assert.AreSame(dependency, service.Dependency);
        }

        /// <summary>
        /// Verifies that created instances are not stored as registered singleton instances.
        /// </summary>
        [Test]
        public void CreateInstance_RegisteredSingletonType_DoesNotStoreCreatedInstance()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Singleton<ConstructorInjectedService, ConstructorInjectedService>());

            // Act
            var created = container.CreateInstance<ConstructorInjectedService>();
            var resolved = container.GetService<ConstructorInjectedService>();

            // Assert
            Assert.AreSame(dependency, created.Dependency);
            Assert.AreSame(dependency, resolved.Dependency);
            Assert.AreNotSame(created, resolved);
        }

        /// <summary>
        /// Verifies that scoped dependencies are resolved from the active scope during instance creation.
        /// </summary>
        [Test]
        public void CreateInstance_ScopedResolver_InjectsConstructorDependencyFromActiveScope()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(ServiceDescriptor.Scoped<ScopedDependency, ScopedDependency>());

            using var firstScope = container.CreateScope();
            using var secondScope = container.CreateScope();

            // Act
            var firstService = firstScope.ServiceProvider.CreateInstance<ScopedConstructorInjectedService>();
            var firstDependency = firstScope.ServiceProvider.GetService<ScopedDependency>();
            var secondService = secondScope.ServiceProvider.CreateInstance<ScopedConstructorInjectedService>();
            var secondDependency = secondScope.ServiceProvider.GetService<ScopedDependency>();

            // Assert
            Assert.AreSame(firstDependency, firstService.Dependency);
            Assert.AreSame(secondDependency, secondService.Dependency);
            Assert.AreNotSame(firstDependency, secondDependency);
        }

        /// <summary>
        /// Verifies that missing constructor dependencies throw a resolution exception during instance creation.
        /// </summary>
        [Test]
        public void CreateInstance_MissingConstructorDependency_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build();

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.CreateInstance<ConstructorInjectedService>());
        }

        #endregion

        #region Manual Injection Tests

        /// <summary>
        /// Verifies that externally created objects can receive field injection.
        /// </summary>
        [Test]
        public void Inject_ExternalTarget_InjectsDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var target = new FieldInjectedService();
            var container = ServiceContainerBuilder.Build(ServiceDescriptor.Singleton<ITestDependency>(dependency));

            // Act
            container.Inject(target);

            // Assert
            Assert.AreSame(dependency, target.Dependency);
        }

        /// <summary>
        /// Verifies that scoped dependencies are resolved from the active scope during field injection.
        /// </summary>
        [Test]
        public void Inject_ScopedResolver_InjectsDependencyFromActiveScope()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Scoped<ScopedDependency, ScopedDependency>(),
                ServiceDescriptor.Transient<ScopedFieldInjectedService, ScopedFieldInjectedService>());

            using var firstScope = container.CreateScope();
            using var secondScope = container.CreateScope();

            // Act
            var firstService = firstScope.ServiceProvider.GetService<ScopedFieldInjectedService>();
            var firstDependency = firstScope.ServiceProvider.GetService<ScopedDependency>();
            var secondService = secondScope.ServiceProvider.GetService<ScopedFieldInjectedService>();
            var secondDependency = secondScope.ServiceProvider.GetService<ScopedDependency>();
            firstScope.ServiceProvider.Inject(firstService);
            secondScope.ServiceProvider.Inject(secondService);

            // Assert
            Assert.AreSame(firstDependency, firstService.Dependency);
            Assert.AreSame(secondDependency, secondService.Dependency);
            Assert.AreNotSame(firstDependency, secondDependency);
        }

        #endregion

        #region Failure Tests

        /// <summary>
        /// Verifies that missing marked field dependencies throw a resolution exception.
        /// </summary>
        [Test]
        public void Inject_MissingInjectedDependency_ThrowsDependencyResolutionException()
        {
            // Arrange
            var target = new FieldInjectedService();
            var container = ServiceContainerBuilder.Build();

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.Inject(target));
        }

        /// <summary>
        /// Verifies that marked static fields throw a resolution exception.
        /// </summary>
        [Test]
        public void Inject_StaticMarkedField_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(new TestDependency()));
            var target = new StaticFieldInjectedService();

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.Inject(target));
        }

        /// <summary>
        /// Verifies that marked readonly fields throw a resolution exception.
        /// </summary>
        [Test]
        public void Inject_ReadonlyMarkedField_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(new TestDependency()));
            var target = new ReadonlyFieldInjectedService();

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.Inject(target));
        }

        /// <summary>
        /// Verifies that incompatible resolved services throw a resolution exception.
        /// </summary>
        [Test]
        public void Inject_IncompatibleInjectedDependency_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton(typeof(ITestDependency), typeof(IncompatibleDependency)));
            var target = new FieldInjectedService();

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.Inject(target));
        }

        #endregion

        private interface ITestDependency
        {
            Guid Id { get; }
        }

        private sealed class TestDependency : ITestDependency
        {
            public TestDependency()
            {
                Id = Guid.NewGuid();
            }

            public Guid Id { get; }
        }

        private sealed class IncompatibleDependency
        {
            public IncompatibleDependency()
            {
            }
        }

        private class BaseInjectedService
        {
            [Inject]
            private ITestDependency _baseDependency;

            public BaseInjectedService()
            {
            }

            public ITestDependency BaseDependency => _baseDependency;
        }

        private sealed class DerivedInjectedService : BaseInjectedService
        {
            public DerivedInjectedService()
            {
            }
        }

        private sealed class FieldInjectedService
        {
            [Inject]
            private ITestDependency _dependency;

            public FieldInjectedService()
            {
            }

            public ITestDependency Dependency => _dependency;
        }

        private sealed class ConstructorInjectedService
        {
            public ConstructorInjectedService(ITestDependency dependency)
            {
                Dependency = dependency;
            }

            public ITestDependency Dependency { get; }
        }

        private sealed class ScopedDependency
        {
            public ScopedDependency()
            {
            }
        }

        private sealed class ScopedConstructorInjectedService
        {
            public ScopedConstructorInjectedService(ScopedDependency dependency)
            {
                Dependency = dependency;
            }

            public ScopedDependency Dependency { get; }
        }

        private sealed class ScopedFieldInjectedService
        {
            [Inject]
            private ScopedDependency _dependency;

            public ScopedFieldInjectedService()
            {
            }

            public ScopedDependency Dependency => _dependency;
        }

        private sealed class StaticFieldInjectedService
        {
            [Inject]
            private static ITestDependency s_dependency;

            public StaticFieldInjectedService()
            {
            }
        }

        private sealed class ReadonlyFieldInjectedService
        {
            [Inject]
            private readonly ITestDependency _dependency;

            public ReadonlyFieldInjectedService()
            {
            }
        }
    }
}

#pragma warning restore 0169
#pragma warning restore 0649
