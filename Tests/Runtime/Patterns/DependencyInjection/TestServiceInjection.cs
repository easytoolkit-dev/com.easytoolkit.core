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
        #region Automatic Injection Tests

        /// <summary>
        /// Verifies that the container injects private fields marked with InjectAttribute.
        /// </summary>
        [Test]
        public void GetService_MarkedPrivateField_InjectsDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Transient<FieldInjectedService, FieldInjectedService>());

            // Act
            var service = container.GetService<FieldInjectedService>();

            // Assert
            Assert.AreSame(dependency, service.Dependency);
        }

        /// <summary>
        /// Verifies that cached constructor factories still inject marked fields.
        /// </summary>
        [Test]
        public void GetService_CachedFactory_InjectsDependencyForEveryInstance()
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
            Assert.AreSame(dependency, first.Dependency);
            Assert.AreSame(dependency, second.Dependency);
        }

        /// <summary>
        /// Verifies that inherited private marked fields are injected.
        /// </summary>
        [Test]
        public void GetService_InheritedMarkedField_InjectsDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Transient<DerivedInjectedService, DerivedInjectedService>());

            // Act
            var service = container.GetService<DerivedInjectedService>();

            // Assert
            Assert.AreSame(dependency, service.BaseDependency);
        }

        /// <summary>
        /// Verifies that factory-created services receive field injection.
        /// </summary>
        [Test]
        public void GetService_FactoryCreatedService_InjectsDependency()
        {
            // Arrange
            var dependency = new TestDependency();
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(dependency),
                ServiceDescriptor.Transient(_ => new FieldInjectedService()));

            // Act
            var service = container.GetService<FieldInjectedService>();

            // Assert
            Assert.AreSame(dependency, service.Dependency);
        }

        /// <summary>
        /// Verifies that existing singleton instances receive field injection on first resolution.
        /// </summary>
        [Test]
        public void GetService_ExistingSingletonInstance_InjectsDependency()
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
            Assert.AreSame(dependency, resolved.Dependency);
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
        public void GetService_ScopedResolver_InjectsDependencyFromActiveScope()
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
        public void GetService_MissingInjectedDependency_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Transient<FieldInjectedService, FieldInjectedService>());

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.GetService<FieldInjectedService>());
        }

        /// <summary>
        /// Verifies that marked static fields throw a resolution exception.
        /// </summary>
        [Test]
        public void GetService_StaticMarkedField_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(new TestDependency()),
                ServiceDescriptor.Transient<StaticFieldInjectedService, StaticFieldInjectedService>());

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.GetService<StaticFieldInjectedService>());
        }

        /// <summary>
        /// Verifies that marked readonly fields throw a resolution exception.
        /// </summary>
        [Test]
        public void GetService_ReadonlyMarkedField_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton<ITestDependency>(new TestDependency()),
                ServiceDescriptor.Transient<ReadonlyFieldInjectedService, ReadonlyFieldInjectedService>());

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.GetService<ReadonlyFieldInjectedService>());
        }

        /// <summary>
        /// Verifies that incompatible resolved services throw a resolution exception.
        /// </summary>
        [Test]
        public void GetService_IncompatibleInjectedDependency_ThrowsDependencyResolutionException()
        {
            // Arrange
            var container = ServiceContainerBuilder.Build(
                ServiceDescriptor.Singleton(typeof(ITestDependency), typeof(IncompatibleDependency)),
                ServiceDescriptor.Transient<FieldInjectedService, FieldInjectedService>());

            // Act & Assert
            Assert.Throws<DependencyResolutionException>(() => container.GetService<FieldInjectedService>());
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

        private sealed class ScopedDependency
        {
            public ScopedDependency()
            {
            }
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
