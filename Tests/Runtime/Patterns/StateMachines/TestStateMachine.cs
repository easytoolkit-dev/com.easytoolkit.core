using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace EasyToolkit.Core.Patterns.Tests
{
    /// <summary>
    /// Test enum for StateMachine unit tests.
    /// </summary>
    internal enum TestState
    {
        Idle,
        Running,
        Jumping,
        Crouching
    }

    /// <summary>
    /// Unit tests for StateMachine functionality.
    /// </summary>
    [TestFixture]
    public class TestStateMachine
    {
        #region AddState Tests

        /// <summary>
        /// Verifies that AddState successfully adds a state to the state machine.
        /// </summary>
        [Test]
        public void AddState_ValidState_StateIsAdded()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            var state = new ChainableState<TestState>();

            // Act
            stateMachine.AddState(TestState.Idle, state);

            // Assert
            Assert.AreSame(state, stateMachine.FindState(TestState.Idle));
        }

        /// <summary>
        /// Verifies that AddState throws ArgumentException when adding duplicate state.
        /// </summary>
        [Test]
        public void AddState_DuplicateState_ThrowsArgumentException()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            var state1 = new ChainableState<TestState>();
            var state2 = new ChainableState<TestState>();
            stateMachine.AddState(TestState.Idle, state1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => stateMachine.AddState(TestState.Idle, state2));
        }

        #endregion

        #region CreateState Tests

        /// <summary>
        /// Verifies that CreateState creates and adds a chainable state.
        /// </summary>
        [Test]
        public void CreateState_ValidKey_ReturnsChainableState()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();

            // Act
            var state = stateMachine.CreateState(TestState.Idle);

            // Assert
            Assert.IsNotNull(state);
            Assert.AreSame(state, stateMachine.FindState(TestState.Idle));
        }

        /// <summary>
        /// Verifies that CreateState throws ArgumentException for duplicate state.
        /// </summary>
        [Test]
        public void CreateState_DuplicateState_ThrowsArgumentException()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            stateMachine.CreateState(TestState.Idle);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => stateMachine.CreateState(TestState.Idle));
        }

        /// <summary>
        /// Verifies that CreateState supports fluent API chaining.
        /// </summary>
        [Test]
        public void CreateState_FluentConfiguration_SetsCallbacksCorrectly()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            bool onEnterCalled = false;
            bool onExitCalled = false;

            // Act
            stateMachine.CreateState(TestState.Idle)
                .OnEnter(() => onEnterCalled = true)
                .OnExit(() => onExitCalled = true);

            // Assert
            var state = stateMachine.FindState(TestState.Idle);
            Assert.IsNotNull(state);
            state.OnEnter();
            Assert.IsTrue(onEnterCalled);
            state.OnExit();
            Assert.IsTrue(onExitCalled);
        }

        #endregion

        #region RemoveState Tests

        /// <summary>
        /// Verifies that RemoveState removes an existing state.
        /// </summary>
        [Test]
        public void RemoveState_ExistingState_StateIsRemoved()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            var state = new ChainableState<TestState>();
            stateMachine.AddState(TestState.Idle, state);

            // Act
            stateMachine.RemoveState(TestState.Idle);

            // Assert
            Assert.IsNull(stateMachine.FindState(TestState.Idle));
        }

        /// <summary>
        /// Verifies that RemoveState handles non-existent state gracefully.
        /// </summary>
        [Test]
        public void RemoveState_NonExistingState_DoesNotThrow()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();

            // Act & Assert
            Assert.DoesNotThrow(() => stateMachine.RemoveState(TestState.Idle));
        }

        #endregion

        #region FindState Tests

        /// <summary>
        /// Verifies that FindState returns the correct state when it exists.
        /// </summary>
        [Test]
        public void FindState_ExistingState_ReturnsState()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            var state = new ChainableState<TestState>();
            stateMachine.AddState(TestState.Idle, state);

            // Act
            var result = stateMachine.FindState(TestState.Idle);

            // Assert
            Assert.AreSame(state, result);
        }

        /// <summary>
        /// Verifies that FindState returns null when state does not exist.
        /// </summary>
        [Test]
        public void FindState_NonExistingState_ReturnsNull()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();

            // Act
            var result = stateMachine.FindState(TestState.Idle);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region StartState Tests

        /// <summary>
        /// Verifies that StartState sets the current state and calls OnEnter.
        /// </summary>
        [Test]
        public void StartState_ValidState_SetsCurrentStateAndCallsOnEnter()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            bool onEnterCalled = false;
            var state = new ChainableState<TestState>().OnEnter(() => onEnterCalled = true);
            stateMachine.AddState(TestState.Idle, state);

            // Act
            stateMachine.StartState(TestState.Idle);

            // Assert
            Assert.AreSame(state, stateMachine.CurrentState);
            Assert.AreEqual(TestState.Idle, stateMachine.CurrentStateKey);
            Assert.IsTrue(onEnterCalled);
        }

        /// <summary>
        /// Verifies that StartState throws InvalidOperationException when already started.
        /// </summary>
        [Test]
        public void StartState_AlreadyStarted_ThrowsInvalidOperationException()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            stateMachine.CreateState(TestState.Idle);
            stateMachine.CreateState(TestState.Running);
            stateMachine.StartState(TestState.Idle);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => stateMachine.StartState(TestState.Running));
        }

        /// <summary>
        /// Verifies that StartState throws KeyNotFoundException for non-existent state.
        /// </summary>
        [Test]
        public void StartState_NonExistingState_ThrowsKeyNotFoundException()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => stateMachine.StartState(TestState.Idle));
        }

        /// <summary>
        /// Verifies that StartState triggers StateChanged event with null previous state.
        /// </summary>
        [Test]
        public void StartState_ValidState_TriggersStateChangedEvent()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            stateMachine.CreateState(TestState.Idle);
            TestState? previousState = TestState.Jumping;
            TestState newState = TestState.Jumping;
            bool eventTriggered = false;

            stateMachine.StateChanged += (prev, next) =>
            {
                previousState = prev;
                newState = next;
                eventTriggered = true;
            };

            // Act
            stateMachine.StartState(TestState.Idle);

            // Assert
            Assert.IsTrue(eventTriggered);
            Assert.IsNull(previousState);
            Assert.AreEqual(TestState.Idle, newState);
        }

        #endregion

        #region ChangeState Tests

        /// <summary>
        /// Verifies that ChangeState transitions to new state and calls lifecycle methods.
        /// </summary>
        [Test]
        public void ChangeState_ValidState_TransitionsAndCallsLifecycleMethods()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            bool idleOnExitCalled = false;
            bool runningOnEnterCalled = false;

            var idleState = new ChainableState<TestState>().OnExit(() => idleOnExitCalled = true);
            var runningState = new ChainableState<TestState>().OnEnter(() => runningOnEnterCalled = true);

            stateMachine.AddState(TestState.Idle, idleState);
            stateMachine.AddState(TestState.Running, runningState);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.ChangeState(TestState.Running);

            // Assert
            Assert.AreSame(runningState, stateMachine.CurrentState);
            Assert.AreEqual(TestState.Running, stateMachine.CurrentStateKey);
            Assert.IsTrue(idleOnExitCalled);
            Assert.IsTrue(runningOnEnterCalled);
        }

        /// <summary>
        /// Verifies that ChangeState throws KeyNotFoundException for non-existent state.
        /// </summary>
        [Test]
        public void ChangeState_NonExistingState_ThrowsKeyNotFoundException()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            stateMachine.CreateState(TestState.Idle);
            stateMachine.StartState(TestState.Idle);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => stateMachine.ChangeState(TestState.Running));
        }

        /// <summary>
        /// Verifies that ChangeState triggers StateChanged event with correct states.
        /// </summary>
        [Test]
        public void ChangeState_ValidState_TriggersStateChangedEvent()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            stateMachine.CreateState(TestState.Idle);
            stateMachine.CreateState(TestState.Running);
            stateMachine.StartState(TestState.Idle);

            TestState? previousState = TestState.Jumping;
            TestState newState = TestState.Jumping;
            bool eventTriggered = false;

            stateMachine.StateChanged += (prev, next) =>
            {
                previousState = prev;
                newState = next;
                eventTriggered = true;
            };

            // Act
            stateMachine.ChangeState(TestState.Running);

            // Assert
            Assert.IsTrue(eventTriggered);
            Assert.AreEqual(TestState.Idle, previousState);
            Assert.AreEqual(TestState.Running, newState);
        }

        /// <summary>
        /// Verifies that ChangeState supports multiple transitions.
        /// </summary>
        [Test]
        public void ChangeState_MultipleTransitions_UpdatesCurrentStateCorrectly()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            stateMachine.CreateState(TestState.Idle);
            stateMachine.CreateState(TestState.Running);
            stateMachine.CreateState(TestState.Jumping);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.ChangeState(TestState.Running);
            Assert.AreEqual(TestState.Running, stateMachine.CurrentStateKey);

            stateMachine.ChangeState(TestState.Jumping);
            Assert.AreEqual(TestState.Jumping, stateMachine.CurrentStateKey);

            stateMachine.ChangeState(TestState.Idle);
            Assert.AreEqual(TestState.Idle, stateMachine.CurrentStateKey);
        }

        #endregion

        #region Update Tests

        /// <summary>
        /// Verifies that Update calls OnUpdate on current state.
        /// </summary>
        [Test]
        public void Update_CurrentStateExists_CallsOnUpdate()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            bool onUpdateCalled = false;
            var state = new ChainableState<TestState>().OnUpdate(() => onUpdateCalled = true);
            stateMachine.AddState(TestState.Idle, state);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.Update();

            // Assert
            Assert.IsTrue(onUpdateCalled);
        }

        /// <summary>
        /// Verifies that Update does not throw when no current state is set.
        /// </summary>
        [Test]
        public void Update_NoCurrentState_DoesNotThrow()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();

            // Act & Assert
            Assert.DoesNotThrow(() => stateMachine.Update());
        }

        /// <summary>
        /// Verifies that Update only calls OnUpdate on current state.
        /// </summary>
        [Test]
        public void Update_MultipleStates_OnlyUpdatesCurrentState()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            bool idleOnUpdateCalled = false;
            bool runningOnUpdateCalled = false;

            var idleState = new ChainableState<TestState>().OnUpdate(() => idleOnUpdateCalled = true);
            var runningState = new ChainableState<TestState>().OnUpdate(() => runningOnUpdateCalled = true);

            stateMachine.AddState(TestState.Idle, idleState);
            stateMachine.AddState(TestState.Running, runningState);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.Update();

            // Assert
            Assert.IsTrue(idleOnUpdateCalled);
            Assert.IsFalse(runningOnUpdateCalled);
        }

        #endregion

        #region FixedUpdate Tests

        /// <summary>
        /// Verifies that FixedUpdate calls OnFixedUpdate on current state.
        /// </summary>
        [Test]
        public void FixedUpdate_CurrentStateExists_CallsOnFixedUpdate()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            bool onFixedUpdateCalled = false;
            var state = new ChainableState<TestState>().OnFixedUpdate(() => onFixedUpdateCalled = true);
            stateMachine.AddState(TestState.Idle, state);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.FixedUpdate();

            // Assert
            Assert.IsTrue(onFixedUpdateCalled);
        }

        /// <summary>
        /// Verifies that FixedUpdate does not throw when no current state is set.
        /// </summary>
        [Test]
        public void FixedUpdate_NoCurrentState_DoesNotThrow()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();

            // Act & Assert
            Assert.DoesNotThrow(() => stateMachine.FixedUpdate());
        }

        /// <summary>
        /// Verifies that FixedUpdate only calls OnFixedUpdate on current state.
        /// </summary>
        [Test]
        public void FixedUpdate_MultipleStates_OnlyUpdatesCurrentState()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            bool idleOnFixedUpdateCalled = false;
            bool runningOnFixedUpdateCalled = false;

            var idleState = new ChainableState<TestState>().OnFixedUpdate(() => idleOnFixedUpdateCalled = true);
            var runningState = new ChainableState<TestState>().OnFixedUpdate(() => runningOnFixedUpdateCalled = true);

            stateMachine.AddState(TestState.Idle, idleState);
            stateMachine.AddState(TestState.Running, runningState);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.FixedUpdate();

            // Assert
            Assert.IsTrue(idleOnFixedUpdateCalled);
            Assert.IsFalse(runningOnFixedUpdateCalled);
        }

        #endregion

        #region Custom IState Implementation Tests

        /// <summary>
        /// Verifies that StateMachine works with custom IState implementations.
        /// </summary>
        [Test]
        public void CustomIState_Implementation_WorksCorrectly()
        {
            // Arrange
            var stateMachine = new StateMachine<TestState>();
            var customState = new CustomTestState();
            stateMachine.AddState(TestState.Idle, customState);

            // Act
            stateMachine.StartState(TestState.Idle);

            // Assert
            Assert.AreEqual(1, customState.OnEnterCallCount);
            stateMachine.Update();
            Assert.AreEqual(1, customState.OnUpdateCallCount);
            stateMachine.FixedUpdate();
            Assert.AreEqual(1, customState.OnFixedUpdateCallCount);
            stateMachine.ChangeState(TestState.Idle);
            Assert.AreEqual(1, customState.OnExitCallCount);
            Assert.AreEqual(2, customState.OnEnterCallCount);
        }

        /// <summary>
        /// Custom IState implementation for testing.
        /// </summary>
        private class CustomTestState : IState<TestState>
        {
            public int OnEnterCallCount { get; private set; }
            public int OnExitCallCount { get; private set; }
            public int OnUpdateCallCount { get; private set; }
            public int OnFixedUpdateCallCount { get; private set; }

            public void OnEnter() => OnEnterCallCount++;
            public void OnExit() => OnExitCallCount++;
            public void OnUpdate() => OnUpdateCallCount++;
            public void OnFixedUpdate() => OnFixedUpdateCallCount++;
        }

        #endregion
    }
}
