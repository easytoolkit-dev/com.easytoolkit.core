using EasyToolkit.Core.Patterns.Implementations;
using NUnit.Framework;

namespace EasyToolkit.Core.Patterns.Tests
{
    /// <summary>
    /// Unit tests for LenientStateMachine functionality.
    /// </summary>
    [TestFixture]
    public class TestLenientStateMachine
    {
        #region StartState Tests

        /// <summary>
        /// Verifies that StartState with a missing state sets CurrentState to null and CurrentStateKey to the specified key.
        /// </summary>
        [Test]
        public void StartState_MissingState_SetsCurrentStateKeyToNull()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();

            // Act
            stateMachine.StartState(TestState.Idle);

            // Assert
            Assert.IsNull(stateMachine.CurrentState);
            Assert.AreEqual(TestState.Idle, stateMachine.CurrentStateKey);
        }

        /// <summary>
        /// Verifies that StartState with a missing state triggers the StateChanged event.
        /// </summary>
        [Test]
        public void StartState_MissingState_TriggersStateChangedEvent()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
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

        /// <summary>
        /// Verifies that StartState with a valid state works correctly.
        /// </summary>
        [Test]
        public void StartState_ValidState_WorksCorrectly()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            bool onEnterCalled = false;
            var state = new ChainableState<TestState>().WithEnter((owner) => onEnterCalled = true);
            stateMachine.AddState(TestState.Idle, state);

            // Act
            stateMachine.StartState(TestState.Idle);

            // Assert
            Assert.AreSame(state, stateMachine.CurrentState);
            Assert.AreEqual(TestState.Idle, stateMachine.CurrentStateKey);
            Assert.IsTrue(onEnterCalled);
        }

        #endregion

        #region ChangeState Tests

        /// <summary>
        /// Verifies that ChangeState from a valid state to a missing state exits the previous state.
        /// </summary>
        [Test]
        public void ChangeState_FromValidToMissingState_ExitsPreviousState()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            bool onExitCalled = false;
            var idleState = new ChainableState<TestState>().WithExit((owner) => onExitCalled = true);

            stateMachine.AddState(TestState.Idle, idleState);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.ChangeState(TestState.Running); // Running not added

            // Assert
            Assert.IsTrue(onExitCalled);
            Assert.IsNull(stateMachine.CurrentState);
            Assert.AreEqual(TestState.Running, stateMachine.CurrentStateKey);
        }

        /// <summary>
        /// Verifies that ChangeState from a missing state to a valid state enters the new state.
        /// </summary>
        [Test]
        public void ChangeState_FromMissingToValidState_EntersNewState()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            bool onEnterCalled = false;
            var runningState = new ChainableState<TestState>().WithEnter((owner) => onEnterCalled = true);

            stateMachine.AddState(TestState.Running, runningState);
            stateMachine.StartState(TestState.Idle); // Idle not added

            // Act
            stateMachine.ChangeState(TestState.Running);

            // Assert
            Assert.IsTrue(onEnterCalled);
            Assert.AreSame(runningState, stateMachine.CurrentState);
        }

        /// <summary>
        /// Verifies that ChangeState between two missing states does not throw.
        /// </summary>
        [Test]
        public void ChangeState_BetweenMissingStates_DoesNotThrow()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            stateMachine.StartState(TestState.Idle); // Idle not added

            // Act & Assert
            Assert.DoesNotThrow(() => stateMachine.ChangeState(TestState.Running));
            Assert.AreEqual(TestState.Running, stateMachine.CurrentStateKey);
            Assert.IsNull(stateMachine.CurrentState);
        }

        /// <summary>
        /// Verifies that ChangeState with a valid state works correctly.
        /// </summary>
        [Test]
        public void ChangeState_ValidState_WorksCorrectly()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            bool idleOnExitCalled = false;
            bool runningOnEnterCalled = false;

            var idleState = new ChainableState<TestState>().WithExit((owner) => idleOnExitCalled = true);
            var runningState = new ChainableState<TestState>().WithEnter((owner) => runningOnEnterCalled = true);

            stateMachine.AddState(TestState.Idle, idleState);
            stateMachine.AddState(TestState.Running, runningState);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.ChangeState(TestState.Running);

            // Assert
            Assert.IsTrue(idleOnExitCalled);
            Assert.IsTrue(runningOnEnterCalled);
            Assert.AreSame(runningState, stateMachine.CurrentState);
            Assert.AreEqual(TestState.Running, stateMachine.CurrentStateKey);
        }

        #endregion

        #region Update/FixedUpdate Tests

        /// <summary>
        /// Verifies that Update with a missing current state does not throw.
        /// </summary>
        [Test]
        public void Update_MissingCurrentState_DoesNotThrow()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            stateMachine.StartState(TestState.Idle); // Idle not added

            // Act & Assert
            Assert.DoesNotThrow(() => stateMachine.Update());
        }

        /// <summary>
        /// Verifies that FixedUpdate with a missing current state does not throw.
        /// </summary>
        [Test]
        public void FixedUpdate_MissingCurrentState_DoesNotThrow()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            stateMachine.StartState(TestState.Idle); // Idle not added

            // Act & Assert
            Assert.DoesNotThrow(() => stateMachine.FixedUpdate());
        }

        /// <summary>
        /// Verifies that Update with a valid state calls OnUpdate.
        /// </summary>
        [Test]
        public void Update_ValidState_CallsOnUpdate()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            bool onUpdateCalled = false;
            var state = new ChainableState<TestState>().WithUpdate((owner) => onUpdateCalled = true);
            stateMachine.AddState(TestState.Idle, state);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.Update();

            // Assert
            Assert.IsTrue(onUpdateCalled);
        }

        /// <summary>
        /// Verifies that FixedUpdate with a valid state calls OnFixedUpdate.
        /// </summary>
        [Test]
        public void FixedUpdate_ValidState_CallsOnFixedUpdate()
        {
            // Arrange
            var stateMachine = new LenientStateMachine<TestState>();
            bool onFixedUpdateCalled = false;
            var state = new ChainableState<TestState>().WithFixedUpdate((owner) => onFixedUpdateCalled = true);
            stateMachine.AddState(TestState.Idle, state);
            stateMachine.StartState(TestState.Idle);

            // Act
            stateMachine.FixedUpdate();

            // Assert
            Assert.IsTrue(onFixedUpdateCalled);
        }

        #endregion

        #region Factory Tests

        /// <summary>
        /// Verifies that StateMachineFactory.CreateLenient creates a lenient state machine.
        /// </summary>
        [Test]
        public void Factory_CreateLenient_CreatesLenientStateMachine()
        {
            // Arrange & Act
            var stateMachine = StateMachineFactory.CreateLenient<TestState>();

            // Assert
            Assert.IsNotNull(stateMachine);
            Assert.IsInstanceOf<LenientStateMachine<TestState>>(stateMachine);
        }

        /// <summary>
        /// Verifies that StateMachineFactory.Create creates a strict state machine.
        /// </summary>
        [Test]
        public void Factory_Create_CreatesStrictStateMachine()
        {
            // Arrange & Act
            var stateMachine = StateMachineFactory.Create<TestState>();

            // Assert
            Assert.IsNotNull(stateMachine);
            Assert.IsInstanceOf<StateMachine<TestState>>(stateMachine);
        }

        #endregion
    }
}
