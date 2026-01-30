using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// A generic State Machine implementation where states are identified by an Tuple enum.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public class StateMachine<T> where T : struct, Enum
    {
        private readonly Dictionary<T, IState<T>> _stateByKey = new Dictionary<T, IState<T>>();

        /// <summary>
        /// Delegate for handling state changes.
        /// </summary>
        /// <param name="previousState">The key of the state being exited. Null when starting the state machine.</param>
        /// <param name="newState">The key of the state being entered.</param>
        public delegate void StateChangeHandler(T? previousState, T newState);

        /// <summary>
        /// Event triggered when the state changes.
        /// </summary>
        public event StateChangeHandler StateChanged;

        /// <summary>
        /// Gets the current active state instance.
        /// </summary>
        public IState<T> CurrentState { get; private set; }

        /// <summary>
        /// Gets the key of the current active state.
        /// </summary>
        public T? CurrentStateKey { get; private set; }

        /// <summary>
        /// Gets or sets whether the state machine allows state transitions to non-existent states.
        /// When enabled, transitioning to a non-existent state will only trigger the StateChanged event
        /// without calling state lifecycle methods or throwing an exception.
        /// </summary>
        public bool AllowMissingStates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
        /// </summary>
        /// <param name="allowMissingStates">Whether to allow transitions to non-existent states. Default is false.</param>
        public StateMachine(bool allowMissingStates = false)
        {
            AllowMissingStates = allowMissingStates;
        }

        /// <summary>
        /// Adds a state to the state machine.
        /// </summary>
        /// <param name="key">The enum key for the state.</param>
        /// <param name="state">The state instance.</param>
        /// <exception cref="ArgumentException">Thrown when a state with the same key already exists.</exception>
        public void AddState(T key, IState<T> state)
        {
            if (_stateByKey.ContainsKey(key))
            {
                throw new ArgumentException($"State {key} already exists in StateMachine. Cannot add duplicate states.", nameof(key));
            }
            _stateByKey.Add(key, state);
        }

        /// <summary>
        /// Creates a new chainable state and adds it to the state machine.
        /// Use this method for fluent state configuration.
        /// </summary>
        /// <param name="key">The enum key for the state.</param>
        /// <returns>A <see cref="ChainableState{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when a state with the same key already exists.</exception>
        /// <example>
        /// <code>
        /// stateMachine.CreateState(GameState.Idle)
        ///     .OnEnter(() => Debug.Log("Entering Idle"))
        ///     .OnExit(() => Debug.Log("Exiting Idle"))
        ///     .OnUpdate(() => Debug.Log("Updating Idle"));
        /// </code>
        /// </example>
        public ChainableState<T> CreateState(T key)
        {
            var state = new ChainableState<T>();
            AddState(key, state);
            return state;
        }

        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <param name="key">The enum key of the state to remove.</param>
        public void RemoveState(T key)
        {
            if (_stateByKey.ContainsKey(key))
            {
                _stateByKey.Remove(key);
            }
        }

        /// <summary>
        /// Get a state by its key.
        /// </summary>
        /// <param name="key">The enum key to look for.</param>
        /// <returns>The state instance if found, otherwise null.</returns>
        public IState<T> FindState(T key)
        {
            if (_stateByKey.TryGetValue(key, out var state))
            {
                return state;
            }
            return null;
        }

        /// <summary>
        /// Starts the state machine with the specified state.
        /// Can only be called when the state machine has not been started yet (CurrentState is null).
        /// </summary>
        /// <param name="key">The enum key of the starting state.</param>
        /// <exception cref="InvalidOperationException">Thrown when the state machine has already been started.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the state key does not exist and <see cref="AllowMissingStates"/> is false.</exception>
        public void StartState(T key)
        {
            if (CurrentState != null)
            {
                throw new InvalidOperationException($"StateMachine is already running (CurrentState: {CurrentStateKey}). Use ChangeState to transition.");
            }

            if (_stateByKey.TryGetValue(key, out var newState))
            {
                CurrentState = newState;
                CurrentStateKey = key;
                CurrentState.OnEnter();

                StateChanged?.Invoke(null, key);
            }
            else if (AllowMissingStates)
            {
                CurrentState = null;
                CurrentStateKey = key;

                StateChanged?.Invoke(null, key);
            }
            else
            {
                throw new KeyNotFoundException($"State {key} does not exist in StateMachine! Ensure the state is added before starting.");
            }
        }

        /// <summary>
        /// Changes the current state to the state with the specified key.
        /// </summary>
        /// <param name="key">The enum key of the target state.</param>
        /// <exception cref="KeyNotFoundException">Thrown when the target state key does not exist in the state machine and <see cref="AllowMissingStates"/> is false.</exception>
        public void ChangeState(T key)
        {
            if (_stateByKey.TryGetValue(key, out var newState))
            {
                var previousKey = CurrentStateKey;

                if (CurrentState != null)
                {
                    CurrentState.OnExit();
                }

                CurrentState = newState;
                CurrentStateKey = key;
                CurrentState.OnEnter();

                StateChanged?.Invoke(previousKey, CurrentStateKey.Value);
            }
            else if (AllowMissingStates)
            {
                var previousKey = CurrentStateKey;

                if (CurrentState != null)
                {
                    CurrentState.OnExit();
                }

                CurrentState = null;
                CurrentStateKey = key;

                StateChanged?.Invoke(previousKey, CurrentStateKey.Value);
            }
            else
            {
                throw new KeyNotFoundException($"State {key} does not exist in StateMachine! Ensure the state is added before transitioning to it.");
            }
        }

        /// <summary>
        /// Updates the current state. Should be called every frame.
        /// </summary>
        public void Update()
        {
            CurrentState?.OnUpdate();
        }

        /// <summary>
        /// Updates the current state. Should be called every fixed framerate frame.
        /// </summary>
        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate();
        }
    }
}
