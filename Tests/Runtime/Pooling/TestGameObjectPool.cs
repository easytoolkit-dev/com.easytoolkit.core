using System;
using NUnit.Framework;
using UnityEngine;

namespace EasyToolkit.Core.Pooling.Tests
{
    /// <summary>
    /// Unit tests for GameObject pool behavior.
    /// </summary>
    [TestFixture]
    public class TestGameObjectPool
    {
        private GameObject _managerRootGameObject;
        private IGameObjectPoolManager _manager;
        private GameObject _prefab;

        [SetUp]
        public void SetUp()
        {
            _managerRootGameObject = new GameObject(nameof(TestGameObjectPool));
            _manager = PoolManagerFactory.CreateGameObjectPoolManager(_managerRootGameObject.transform);

            _prefab = new GameObject("PoolPrefab");
            _prefab.AddComponent<TestPoolItem>();
            _prefab.SetActive(false);
        }

        [TearDown]
        public void TearDown()
        {
            if (_managerRootGameObject != null)
            {
                UnityEngine.Object.DestroyImmediate(_managerRootGameObject);
            }

            if (_prefab != null)
            {
                UnityEngine.Object.DestroyImmediate(_prefab);
            }
        }

        #region Creation Tests

        /// <summary>
        /// Verifies that CreatePool registers the pool and creates a dedicated root under the manager transform.
        /// </summary>
        [Test]
        public void CreatePool_ValidName_RegistersPoolAndCreatesRootHierarchy()
        {
            // Arrange & Act
            var pool = _manager.CreatePool("game_object_pool", _prefab);

            // Assert
            Assert.That(_manager.HasPool("game_object_pool"), Is.True);
            Assert.That(_manager.TryGetPool("game_object_pool", out var resolvedPool), Is.True);
            Assert.That(resolvedPool, Is.SameAs(pool));
            Assert.That(pool.Original, Is.SameAs(_prefab));
            Assert.That(pool.Transform.parent, Is.SameAs(_manager.Transform));
            Assert.That(pool.Transform.name, Is.EqualTo("game_object_pool"));
        }

        /// <summary>
        /// Verifies that preallocation creates idle instances under the pool root.
        /// </summary>
        [Test]
        public void CreatePool_Preallocation_InitializesIdleInstances()
        {
            // Arrange & Act
            var pool = _manager.CreatePool(
                "game_object_pool",
                _prefab,
                new GameObjectPoolConfiguration(preallocationCount: 2));

            // Assert
            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.IdleCount, Is.EqualTo(2));
            Assert.That(pool.Transform.childCount, Is.EqualTo(2));
        }

        #endregion

        #region Lifecycle Tests

        /// <summary>
        /// Verifies that releasing an instance returns it to the pool root and updates pool counts.
        /// </summary>
        [Test]
        public void RentAndRelease_ManagedInstance_ReparentsToPoolRootAndUpdatesCounts()
        {
            // Arrange
            var pool = _manager.CreatePool("game_object_pool", _prefab);
            var externalParent = new GameObject("ExternalParent");

            try
            {
                // Act
                var instance = pool.Rent();
                instance.transform.SetParent(externalParent.transform, false);

                var released = pool.Release(instance);

                // Assert
                Assert.That(released, Is.True);
                Assert.That(pool.ActiveCount, Is.EqualTo(0));
                Assert.That(pool.IdleCount, Is.EqualTo(1));
                Assert.That(instance.transform.parent, Is.SameAs(pool.Transform));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(externalParent);
            }
        }

        /// <summary>
        /// Verifies that enabling pool item callbacks notifies components implementing <see cref="IPoolObject"/>.
        /// </summary>
        [Test]
        public void RentAndRelease_CallbacksEnabled_InvokesPoolItemCallbacks()
        {
            // Arrange
            var pool = _manager.CreatePool("game_object_pool", _prefab);

            // Act
            var instance = pool.Rent();
            var poolItem = instance.GetComponent<TestPoolItem>();
            var released = pool.Release(instance);

            // Assert
            Assert.That(released, Is.True);
            Assert.That(poolItem.RentCount, Is.EqualTo(1));
            Assert.That(poolItem.ReleaseCount, Is.EqualTo(1));
        }

        /// <summary>
        /// Verifies that disabling pool item callbacks suppresses <see cref="IPoolObject"/> notifications.
        /// </summary>
        [Test]
        public void RentAndRelease_CallbacksDisabled_SkipsPoolItemCallbacks()
        {
            // Arrange
            var pool = _manager.CreatePool(
                "game_object_pool",
                _prefab,
                new GameObjectPoolConfiguration(enablePoolItemCallbacks: false));

            // Act
            var instance = pool.Rent();
            var poolItem = instance.GetComponent<TestPoolItem>();
            var released = pool.Release(instance);

            // Assert
            Assert.That(released, Is.True);
            Assert.That(poolItem.RentCount, Is.EqualTo(0));
            Assert.That(poolItem.ReleaseCount, Is.EqualTo(0));
        }

        /// <summary>
        /// Verifies that Clear destroys tracked instances and keeps the pool reusable.
        /// </summary>
        [Test]
        public void Clear_WithManagedInstances_DestroysInstancesAndKeepsPoolReusable()
        {
            // Arrange
            var pool = _manager.CreatePool("game_object_pool", _prefab);
            var idleInstance = pool.Rent();
            var activeInstance = pool.Rent();
            Assert.That(pool.Release(idleInstance), Is.True);

            // Act
            pool.Clear();

            // Assert
            Assert.That(pool.ActiveCount, Is.EqualTo(0));
            Assert.That(pool.IdleCount, Is.EqualTo(0));
            Assert.That(pool.Transform, Is.Not.Null);
            Assert.That(idleInstance == null, Is.True);
            Assert.That(activeInstance == null, Is.True);

            var nextInstance = pool.Rent();

            Assert.That(nextInstance, Is.Not.Null);
            Assert.That(pool.ActiveCount, Is.EqualTo(1));
            Assert.That(pool.IdleCount, Is.EqualTo(0));
        }

        #endregion

        #region Lifetime Tests

        /// <summary>
        /// Verifies that lifetime accessors expose and update the managed instance lifetime values.
        /// </summary>
        [Test]
        public void GetLifetimeAccessor_ManagedInstance_AllowsReadingAndUpdatingLifetimeValues()
        {
            // Arrange
            var pool = _manager.CreatePool(
                "game_object_pool",
                _prefab,
                new GameObjectPoolConfiguration(activeLifetime: 1f, idleLifetime: 2f));
            var instance = pool.Rent();

            // Act
            var accessor = pool.GetLifetimeAccessor(instance);
            accessor.ActiveLifetime = 3f;
            accessor.IdleLifetime = 4f;
            accessor.ElapsedTime = 0.75f;

            // Assert
            Assert.That(accessor.ActiveLifetime, Is.EqualTo(3f));
            Assert.That(accessor.IdleLifetime, Is.EqualTo(4f));
            Assert.That(accessor.ElapsedTime, Is.EqualTo(0.75f));

            Assert.That(pool.TryGetLifetimeAccessor(instance, out var resolvedAccessor), Is.True);
            Assert.That(resolvedAccessor.ActiveLifetime, Is.EqualTo(3f));
            Assert.That(resolvedAccessor.IdleLifetime, Is.EqualTo(4f));
            Assert.That(resolvedAccessor.ElapsedTime, Is.EqualTo(0.75f));
        }

        /// <summary>
        /// Verifies that unmanaged instances do not expose lifetime accessors.
        /// </summary>
        [Test]
        public void TryGetLifetimeAccessor_UnmanagedInstance_ReturnsFalse()
        {
            // Arrange
            var pool = _manager.CreatePool("game_object_pool", _prefab);
            var externalInstance = new GameObject("ExternalInstance");

            try
            {
                // Act
                var found = pool.TryGetLifetimeAccessor(externalInstance, out var accessor);

                // Assert
                Assert.That(found, Is.False);
                Assert.That(accessor, Is.Null);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(externalInstance);
            }
        }

        #endregion

        #region Manager Tests

        /// <summary>
        /// Verifies that removing a pool destroys its hierarchy and allows recreating the same pool name.
        /// </summary>
        [Test]
        public void RemovePool_ExistingPool_DestroysHierarchyAndAllowsRecreation()
        {
            // Arrange
            var pool = _manager.CreatePool("game_object_pool", _prefab);
            var instance = pool.Rent();

            // Act
            var removed = _manager.RemovePool("game_object_pool");
            var recreatedPool = _manager.CreatePool("game_object_pool", _prefab);

            // Assert
            Assert.That(removed, Is.True);
            Assert.That(_manager.HasPool("game_object_pool"), Is.True);
            Assert.That(_manager.GetPool("game_object_pool"), Is.SameAs(recreatedPool));
            Assert.That(pool.Transform == null, Is.True);
            Assert.That(instance == null, Is.True);
        }

        #endregion

        private sealed class TestPoolItem : MonoBehaviour, IPoolObject
        {
            public int RentCount { get; private set; }

            public int ReleaseCount { get; private set; }

            public void OnRent()
            {
                RentCount++;
            }

            public void OnRelease()
            {
                ReleaseCount++;
            }
        }
    }
}
