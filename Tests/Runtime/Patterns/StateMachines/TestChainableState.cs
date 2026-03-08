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
            IStateMachine<ChainableTestState> receivedOwner = null;
            state.OnEnter((owner) =>
            {
                callbackInvoked = true;
                receivedOwner = owner;
            });

            // Act
            var mockOwner = new StateMachine<ChainableTestState>();
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter(mockOwner);

            // Assert
            Assert.IsTrue(callbackInvoked);
            Assert.AreSame(mockOwner, receivedOwner);
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
            var result = state.OnEnter((owner) => { });

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
            state.OnEnter((owner) => callCount += 1);
            state.OnEnter((owner) => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter(null);

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
            Assert.DoesNotThrow(() => iState.OnEnter(null));
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
            IStateMachine<ChainableTestState> receivedOwner = null;
            state.OnExit((owner) =>
            {
                callbackInvoked = true;
                receivedOwner = owner;
            });

            // Act
            var mockOwner = new StateMachine<ChainableTestState>();
            var iState = (IState<ChainableTestState>)state;
            iState.OnExit(mockOwner);

            // Assert
            Assert.IsTrue(callbackInvoked);
            Assert.AreSame(mockOwner, receivedOwner);
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
            var result = state.OnExit((owner) => { });

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
            state.OnExit((owner) => callCount += 1);
            state.OnExit((owner) => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnExit(null);

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
            Assert.DoesNotThrow(() => iState.OnExit(null));
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
            IStateMachine<ChainableTestState> receivedOwner = null;
            state.OnUpdate((owner) =>
            {
                callbackInvoked = true;
                receivedOwner = owner;
            });

            // Act
            var mockOwner = new StateMachine<ChainableTestState>();
            var iState = (IState<ChainableTestState>)state;
            iState.OnUpdate(mockOwner);

            // Assert
            Assert.IsTrue(callbackInvoked);
            Assert.AreSame(mockOwner, receivedOwner);
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
            var result = state.OnUpdate((owner) => { });

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
            state.OnUpdate((owner) => callCount += 1);
            state.OnUpdate((owner) => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnUpdate(null);

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
            Assert.DoesNotThrow(() => iState.OnUpdate(null));
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
            IStateMachine<ChainableTestState> receivedOwner = null;
            state.OnFixedUpdate((owner) =>
            {
                callbackInvoked = true;
                receivedOwner = owner;
            });

            // Act
            var mockOwner = new StateMachine<ChainableTestState>();
            var iState = (IState<ChainableTestState>)state;
            iState.OnFixedUpdate(mockOwner);

            // Assert
            Assert.IsTrue(callbackInvoked);
            Assert.AreSame(mockOwner, receivedOwner);
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
            var result = state.OnFixedUpdate((owner) => { });

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
            state.OnFixedUpdate((owner) => callCount += 1);
            state.OnFixedUpdate((owner) => callCount += 10);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnFixedUpdate(null);

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
            Assert.DoesNotThrow(() => iState.OnFixedUpdate(null));
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
            state.OnEnter((owner) => onEnterCalled = true)
                .OnExit((owner) => onExitCalled = true)
                .OnUpdate((owner) => onUpdateCalled = true)
                .OnFixedUpdate((owner) => onFixedUpdateCalled = true);

            // Assert
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter(null);
            Assert.IsTrue(onEnterCalled);

            iState.OnUpdate(null);
            Assert.IsTrue(onUpdateCalled);

            iState.OnFixedUpdate(null);
            Assert.IsTrue(onFixedUpdateCalled);

            iState.OnExit(null);
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
            var result = state.OnEnter((owner) => { })
                .OnExit((owner) => { })
                .OnUpdate((owner) => { })
                .OnFixedUpdate((owner) => { });

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
                iState.OnEnter(null);
                iState.OnUpdate(null);
                iState.OnFixedUpdate(null);
                iState.OnExit(null);
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

            chainableState.OnEnter((owner) => onEnterOrder = ++callOrder)
                .OnUpdate((owner) => onUpdateOrder = ++callOrder)
                .OnFixedUpdate((owner) => onFixedUpdateOrder = ++callOrder)
                .OnExit((owner) => onExitOrder = ++callOrder);

            // Act
            var iState = (IState<ChainableTestState>)chainableState;
            iState.OnEnter(null);
            iState.OnUpdate(null);
            iState.OnFixedUpdate(null);
            iState.OnExit(null);

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
            state.OnEnter((owner) => onEnterCalled = true);
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter(null);

            state.OnExit((owner) => onExitCalled = true);
            iState.OnExit(null);

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

            state.OnEnter((owner) => onEnterCalled = true);
            state.OnUpdate((owner) => onUpdateCalled = true);

            // Act
            state.OnEnter(null);
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter(null);
            iState.OnUpdate(null);

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
            state.OnUpdate((owner) => callCount++);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnUpdate(null);
            iState.OnUpdate(null);
            iState.OnUpdate(null);

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
            state.OnEnter((owner) => throw new InvalidOperationException("Test exception"));

            // Act & Assert
            var iState = (IState<ChainableTestState>)state;
            Assert.Throws<InvalidOperationException>(() => iState.OnEnter(null));
        }

        #endregion

        #region Owner Parameter Tests

        /// <summary>
        /// Verifies that the owner parameter is correctly passed to callbacks.
        /// </summary>
        [Test]
        public void OwnerParameter_CorrectlyPassed_ToAllCallbacks()
        {
            // Arrange
            var state = new ChainableState<ChainableTestState>();
            var stateMachine = new StateMachine<ChainableTestState>();
            IStateMachine<ChainableTestState> onEnterOwner = null;
            IStateMachine<ChainableTestState> onUpdateOwner = null;
            IStateMachine<ChainableTestState> onExitOwner = null;

            state.OnEnter((owner) => onEnterOwner = owner)
                .OnUpdate((owner) => onUpdateOwner = owner)
                .OnExit((owner) => onExitOwner = owner);

            // Act
            var iState = (IState<ChainableTestState>)state;
            iState.OnEnter(stateMachine);
            iState.OnUpdate(stateMachine);
            iState.OnExit(stateMachine);

            // Assert
            Assert.AreSame(stateMachine, onEnterOwner);
            Assert.AreSame(stateMachine, onUpdateOwner);
            Assert.AreSame(stateMachine, onExitOwner);
        }

        #endregion
    }
}
