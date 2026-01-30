using System;
using NUnit.Framework;

namespace EasyToolkit.Core.Patterns.Tests
{
    /// <summary>
    /// Test enum for ChainableState unit tests.
    /// </summary>
    internal enum ChainableTestState
    {
        StateA,
        StateB
    }

    /// <summary>
    /// Unit tests for ChainableState functionality.
    /// </summary>
    [TestFixture]
    public class TestChainableState
    {
        #region OnEnter Tests

        /// <summary>
        /// Verifies that OnEnter callback is invoked when state is entered.
        /// </summary>
        [Test]
        public void OnEnter_CallbackSet_InvokesCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            bool callbackInvoked = false;
            state.OnEnter(() => callbackInvoked = true);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter();

            // Assert
            Assert.IsTrue(callbackInvoked);
        }

        /// <summary>
        /// Verifies that OnEnter can be chained with other methods.
        /// </summary>
        [Test]
        public void OnEnter_Chaining_ReturnsSameInstance()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();

            // Act
            var result = state.OnEnter(() => { });

            // Assert
            Assert.AreSame(state, result);
        }

        /// <summary>
        /// Verifies that OnEnter callback can be replaced.
        /// </summary>
        [Test]
        public void OnEnter_CallbackReplaced_UsesNewCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            int callCount = 0;
            state.OnEnter(() => callCount += 1);
            state.OnEnter(() => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter();

            // Assert
            Assert.AreEqual(10, callCount);
        }

        /// <summary>
        /// Verifies that OnEnter with null callback does not throw when invoked.
        /// </summary>
        [Test]
        public void OnEnter_NullCallback_DoesNotThrow()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            state.OnEnter(null);

            // Act & Assert
            var iState = (IState<ChainableTestState>)state;
            Assert.DoesNotThrow(() => iState.OnEnter());
        }

        #endregion

        #region OnExit Tests

        /// <summary>
        /// Verifies that OnExit callback is invoked when state is exited.
        /// </summary>
        [Test]
        public void OnExit_CallbackSet_InvokesCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            bool callbackInvoked = false;
            state.OnExit(() => callbackInvoked = true);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnExit();

            // Assert
            Assert.IsTrue(callbackInvoked);
        }

        /// <summary>
        /// Verifies that OnExit can be chained with other methods.
        /// </summary>
        [Test]
        public void OnExit_Chaining_ReturnsSameInstance()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();

            // Act
            var result = state.OnExit(() => { });

            // Assert
            Assert.AreSame(state, result);
        }

        /// <summary>
        /// Verifies that OnExit callback can be replaced.
        /// </summary>
        [Test]
        public void OnExit_CallbackReplaced_UsesNewCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            int callCount = 0;
            state.OnExit(() => callCount += 1);
            state.OnExit(() => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnExit();

            // Assert
            Assert.AreEqual(10, callCount);
        }

        /// <summary>
        /// Verifies that OnExit with null callback does not throw when invoked.
        /// </summary>
        [Test]
        public void OnExit_NullCallback_DoesNotThrow()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            state.OnExit(null);

            // Act & Assert
            var iState = (IState<ChainableTestState>)state;
            Assert.DoesNotThrow(() => iState.OnExit());
        }

        #endregion

        #region OnUpdate Tests

        /// <summary>
        /// Verifies that OnUpdate callback is invoked during update.
        /// </summary>
        [Test]
        public void OnUpdate_CallbackSet_InvokesCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            bool callbackInvoked = false;
            state.OnUpdate(() => callbackInvoked = true);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnUpdate();

            // Assert
            Assert.IsTrue(callbackInvoked);
        }

        /// <summary>
        /// Verifies that OnUpdate can be chained with other methods.
        /// </summary>
        [Test]
        public void OnUpdate_Chaining_ReturnsSameInstance()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();

            // Act
            var result = state.OnUpdate(() => { });

            // Assert
            Assert.AreSame(state, result);
        }

        /// <summary>
        /// Verifies that OnUpdate callback can be replaced.
        /// </summary>
        [Test]
        public void OnUpdate_CallbackReplaced_UsesNewCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            int callCount = 0;
            state.OnUpdate(() => callCount += 1);
            state.OnUpdate(() => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnUpdate();

            // Assert
            Assert.AreEqual(10, callCount);
        }

        /// <summary>
        /// Verifies that OnUpdate with null callback does not throw when invoked.
        /// </summary>
        [Test]
        public void OnUpdate_NullCallback_DoesNotThrow()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            state.OnUpdate(null);

            // Act & Assert
            var iState = (IState<ChainableTestState>)state;
            Assert.DoesNotThrow(() => iState.OnUpdate());
        }

        #endregion

        #region OnFixedUpdate Tests

        /// <summary>
        /// Verifies that OnFixedUpdate callback is invoked during fixed update.
        /// </summary>
        [Test]
        public void OnFixedUpdate_CallbackSet_InvokesCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            bool callbackInvoked = false;
            state.OnFixedUpdate(() => callbackInvoked = true);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnFixedUpdate();

            // Assert
            Assert.IsTrue(callbackInvoked);
        }

        /// <summary>
        /// Verifies that OnFixedUpdate can be chained with other methods.
        /// </summary>
        [Test]
        public void OnFixedUpdate_Chaining_ReturnsSameInstance()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();

            // Act
            var result = state.OnFixedUpdate(() => { });

            // Assert
            Assert.AreSame(state, result);
        }

        /// <summary>
        /// Verifies that OnFixedUpdate callback can be replaced.
        /// </summary>
        [Test]
        public void OnFixedUpdate_CallbackReplaced_UsesNewCallback()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            int callCount = 0;
            state.OnFixedUpdate(() => callCount += 1);
            state.OnFixedUpdate(() => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnFixedUpdate();

            // Assert
            Assert.AreEqual(10, callCount);
        }

        /// <summary>
        /// Verifies that OnFixedUpdate with null callback does not throw when invoked.
        /// </summary>
        [Test]
        public void OnFixedUpdate_NullCallback_DoesNotThrow()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            state.OnFixedUpdate(null);

            // Act & Assert
            var iState = (IState<ChainableTestState>)state;
            Assert.DoesNotThrow(() => iState.OnFixedUpdate());
        }

        #endregion

        #region Fluent API Tests

        /// <summary>
        /// Verifies that all callbacks can be configured using fluent API.
        /// </summary>
        [Test]
        public void FluentAPI_AllCallbacks_ConfiguredCorrectly()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            bool onEnterCalled = false;
            bool onExitCalled = false;
            bool onUpdateCalled = false;
            bool onFixedUpdateCalled = false;

            // Act
            state.OnEnter(() => onEnterCalled = true)
                .OnExit(() => onExitCalled = true)
                .OnUpdate(() => onUpdateCalled = true)
                .OnFixedUpdate(() => onFixedUpdateCalled = true);

            // Assert
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter();
            Assert.IsTrue(onEnterCalled);

            iState.OnUpdate();
            Assert.IsTrue(onUpdateCalled);

            iState.OnFixedUpdate();
            Assert.IsTrue(onFixedUpdateCalled);

            iState.OnExit();
            Assert.IsTrue(onExitCalled);
        }

        /// <summary>
        /// Verifies that fluent API maintains instance reference across multiple calls.
        /// </summary>
        [Test]
        public void FluentAPI_MultipleChains_ReturnsSameInstance()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();

            // Act
            var result = state.OnEnter(() => { })
                .OnExit(() => { })
                .OnUpdate(() => { })
                .OnFixedUpdate(() => { });

            // Assert
            Assert.AreSame(state, result);
        }

        #endregion

        #region IState Interface Implementation Tests

        /// <summary>
        /// Verifies that ChainableState implements IState interface correctly.
        /// </summary>
        [Test]
        public void IState_Interface_Implementation_WorksCorrectly()
        {
            // Arrange
            var chainableState = new ChainableState<ChainableTestState>();
            IState<ChainableTestState> iState = chainableState;

            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() =>
            {
                iState.OnEnter();
                iState.OnUpdate();
                iState.OnFixedUpdate();
                iState.OnExit();
            });
        }

        /// <summary>
        /// Verifies that callbacks set via ChainableState methods are called through IState interface.
        /// </summary>
        [Test]
        public void IState_InterfaceCallbacks_InvokedCorrectly()
        {
            // Arrange
            var chainableState = new ChainableState<ChainableTestState>();
            int callOrder = 0;
            int onEnterOrder = 0;
            int onUpdateOrder = 0;
            int onFixedUpdateOrder = 0;
            int onExitOrder = 0;

            chainableState.OnEnter(() => onEnterOrder = ++callOrder)
                .OnUpdate(() => onUpdateOrder = ++callOrder)
                .OnFixedUpdate(() => onFixedUpdateOrder = ++callOrder)
                .OnExit(() => onExitOrder = ++callOrder);

            // Act
            var iState = (IState<ChainableTestState>)chainableState;
            iState.OnEnter();
            iState.OnUpdate();
            iState.OnFixedUpdate();
            iState.OnExit();

            // Assert
            Assert.AreEqual(1, onEnterOrder);
            Assert.AreEqual(2, onUpdateOrder);
            Assert.AreEqual(3, onFixedUpdateOrder);
            Assert.AreEqual(4, onExitOrder);
        }

        #endregion

        #region Callback Independence Tests

        /// <summary>
        /// Verifies that each callback can be set independently.
        /// </summary>
        [Test]
        public void Callbacks_Independent_SettingOneDoesNotAffectOthers()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            bool onEnterCalled = false;
            bool onExitCalled = false;

            // Act
            state.OnEnter(() => onEnterCalled = true);
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter();

            state.OnExit(() => onExitCalled = true);
            iState.OnExit();

            // Assert
            Assert.IsTrue(onEnterCalled);
            Assert.IsTrue(onExitCalled);
        }

        /// <summary>
        /// Verifies that setting a callback to null does not affect other callbacks.
        /// </summary>
        [Test]
        public void Callbacks_SettingToNull_DoesNotAffectOthers()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            bool onEnterCalled = false;
            bool onUpdateCalled = false;

            state.OnEnter(() => onEnterCalled = true);
            state.OnUpdate(() => onUpdateCalled = true);

            // Act
            state.OnEnter(null);
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter();
            iState.OnUpdate();

            // Assert
            Assert.IsFalse(onEnterCalled);
            Assert.IsTrue(onUpdateCalled);
        }

        #endregion

        #region Multiple Invocation Tests

        /// <summary>
        /// Verifies that callbacks can be invoked multiple times.
        /// </summary>
        [Test]
        public void Callbacks_MultipleInvocations_ExecutesEachTime()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            int callCount = 0;
            state.OnUpdate(() => callCount++);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnUpdate();
            iState.OnUpdate();
            iState.OnUpdate();

            // Assert
            Assert.AreEqual(3, callCount);
        }

        #endregion

        #region Exception Handling Tests

        /// <summary>
        /// Verifies that exceptions in callbacks are propagated.
        /// </summary>
        [Test]
        public void Callback_Exception_PropagatesToCaller()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            state.OnEnter(() => throw new InvalidOperationException("Test exception"));

            // Act & Assert
            var iState = (IState<ChainableTestState>)state;
            Assert.Throws<InvalidOperationException>(() => iState.OnEnter());
        }

        #endregion
    }
}
