using System;
using NUnit.Framework;

namespace EasyToolkit.Core.Pooling.Tests
{
    /// <summary>
    /// Unit tests for C# object pool behavior.
    /// </summary>
    [TestFixture]
    public class TestObjectPool
    {
        #region Creation Tests

        /// <summary>
        /// Verifies that CreatePool registers the pool in the manager.
        /// </summary>
        [Test]
        public void CreatePool_ValidName_RegistersPoolInManager()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();

            // Act
            var createdPool = manager.CreatePool<PooledDummy>("dummy_pool");
            var found = manager.TryGetPool<PooledDummy>("dummy_pool", out var resolvedPool);

            // Assert
            Assert.That(found, Is.True);
            Assert.That(resolvedPool, Is.SameAs(createdPool));
            Assert.That(createdPool.Name, Is.EqualTo("dummy_pool"));
            Assert.That(createdPool.ObjectType, Is.EqualTo(typeof(PooledDummy)));
        }

        /// <summary>
        /// Verifies that removing a pool allows the same name to be reused.
        /// </summary>
        [Test]
        public void RemovePool_ExistingPool_AllowsRecreatingPoolWithSameName()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            manager.CreatePool<PooledDummy>("dummy_pool");

            // Act
            var removed = manager.RemovePool("dummy_pool");
            var recreatedPool = manager.CreatePool<OtherPooledDummy>("dummy_pool");

            // Assert
            Assert.That(removed, Is.True);
            Assert.That(manager.TryGetPool<PooledDummy>("dummy_pool", out _), Is.False);
            Assert.That(manager.TryGetPool<OtherPooledDummy>("dummy_pool", out var resolvedPool), Is.True);
            Assert.That(resolvedPool, Is.SameAs(recreatedPool));
        }

        #endregion

        #region Allocation Tests

        /// <summary>
        /// Verifies that preallocation populates the idle stack when FastCache is disabled.
        /// </summary>
        [Test]
        public void CreatePool_PreallocationWithoutFastCache_InitializesIdleInstances()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            var pool = manager.CreatePool(
                "dummy_pool",
                new ObjectPoolConfiguration<PooledDummy>(
                    preallocationCount: 3,
                    useFastCache: false));

            // Assert
            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.IdleCount, Is.EqualTo(3));
            Assert.That(pool.Capacity, Is.Null);
        }

        /// <summary>
        /// Verifies that FastCache serves the first four rents before the allocator creates another object.
        /// </summary>
        [Test]
        public void Rent_WithFastCacheEnabled_UsesHotCacheBeforeAllocatingAdditionalInstances()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            var allocationCount = 0;
            var pool = manager.CreatePool(
                "dummy_pool",
                new ObjectPoolConfiguration<PooledDummy>(
                    allocator: () =>
                    {
                        allocationCount++;
                        return new PooledDummy();
                    }));

            // Act
            var first = pool.Rent();
            var second = pool.Rent();
            var third = pool.Rent();
            var fourth = pool.Rent();
            var fifth = pool.Rent();

            // Assert
            Assert.That(allocationCount, Is.EqualTo(5));
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(second, Is.Not.SameAs(third));
            Assert.That(third, Is.Not.SameAs(fourth));
            Assert.That(fourth, Is.Not.SameAs(fifth));
            Assert.That(pool.ActiveCount, Is.EqualTo(5));
            Assert.That(pool.IdleCount, Is.EqualTo(0));
        }

        #endregion

        #region Lifecycle Tests

        /// <summary>
        /// Verifies that pooled object callbacks and registered callbacks are invoked on rent and release.
        /// </summary>
        [Test]
        public void RentAndRelease_CallbacksEnabled_InvokesPoolObjectAndRegisteredCallbacks()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            var pool = manager.CreatePool<PooledCallbackDummy>("dummy_pool")
                .OnRent(instance => instance.ExternalRentCallbackCount++)
                .OnRelease(instance => instance.ExternalReleaseCallbackCount++);

            // Act
            var instance = pool.Rent();
            var released = pool.Release(instance);

            // Assert
            Assert.That(released, Is.True);
            Assert.That(instance.OnRentCount, Is.EqualTo(1));
            Assert.That(instance.OnReleaseCount, Is.EqualTo(1));
            Assert.That(instance.ExternalRentCallbackCount, Is.EqualTo(1));
            Assert.That(instance.ExternalReleaseCallbackCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Verifies that disabling pool item callbacks does not suppress manually registered callbacks.
        /// </summary>
        [Test]
        public void RentAndRelease_CallbacksDisabled_SkipsPoolObjectCallbacksButKeepsRegisteredCallbacks()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            var pool = manager.CreatePool(
                "dummy_pool",
                new ObjectPoolConfiguration<PooledCallbackDummy>(enablePoolItemCallbacks: false))
                .OnRent(instance => instance.ExternalRentCallbackCount++)
                .OnRelease(instance => instance.ExternalReleaseCallbackCount++);

            // Act
            var instance = pool.Rent();
            var released = pool.Release(instance);

            // Assert
            Assert.That(released, Is.True);
            Assert.That(instance.OnRentCount, Is.EqualTo(0));
            Assert.That(instance.OnReleaseCount, Is.EqualTo(0));
            Assert.That(instance.ExternalRentCallbackCount, Is.EqualTo(1));
            Assert.That(instance.ExternalReleaseCallbackCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Verifies that removing an active instance detaches it from pool tracking.
        /// </summary>
        [Test]
        public void Remove_ActiveInstance_StopsTrackingAndPreventsLaterRelease()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            var pool = manager.CreatePool(
                "dummy_pool",
                new ObjectPoolConfiguration<PooledDummy>(useFastCache: false));
            var instance = pool.Rent();

            // Act
            var removed = pool.Remove(instance);
            var released = pool.Release(instance);

            // Assert
            Assert.That(removed, Is.True);
            Assert.That(released, Is.False);
            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.IdleCount, Is.EqualTo(0));
        }

        #endregion

        #region Capacity Tests

        /// <summary>
        /// Verifies that shrinking capacity removes extra idle instances and enforces the new limit.
        /// </summary>
        [Test]
        public void SetCapacity_SmallerThanIdleCount_TrimsIdleInstancesAndEnforcesLimit()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            var pool = manager.CreatePool(
                "dummy_pool",
                new ObjectPoolConfiguration<PooledDummy>(
                    preallocationCount: 5,
                    useFastCache: false));

            // Act
            pool.SetCapacity(2);

            // Assert
            Assert.That(pool.Capacity, Is.EqualTo(2));
            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.IdleCount, Is.EqualTo(2));

            pool.Rent();
            pool.Rent();

            Assert.Throws<InvalidOperationException>(() => pool.Rent());
        }

        /// <summary>
        /// Verifies that capacity cannot be reduced below the active object count.
        /// </summary>
        [Test]
        public void SetCapacity_SmallerThanActiveCount_ThrowsInvalidOperationException()
        {
            // Arrange
            var manager = PoolManagerFactory.CreateObjectPoolManager();
            var pool = manager.CreatePool(
                "dummy_pool",
                new ObjectPoolConfiguration<PooledDummy>(useFastCache: false));

            pool.Rent();
            pool.Rent();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => pool.SetCapacity(1));
        }

        #endregion

        private sealed class PooledDummy
        {
        }

        private sealed class OtherPooledDummy
        {
        }

        private sealed class PooledCallbackDummy : IPoolObject
        {
            public int OnRentCount { get; private set; }

            public int OnReleaseCount { get; private set; }

            public int ExternalRentCallbackCount { get; set; }

            public int ExternalReleaseCallbackCount { get; set; }

            public void OnRent()
            {
                OnRentCount++;
            }

            public void OnRelease()
            {
                OnReleaseCount++;
            }
        }
    }
}
