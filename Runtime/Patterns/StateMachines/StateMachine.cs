using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyToolkit.Core.Patterns
{
    /// <summary>
    /// A generic State Machine implementation where states are identified by an enum type.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    public class StateMachine<T> : IStateMachine<T> where T : struct, Enum
    {
        // Dictionary for O(1) state lookup during transitions
        private readonly Dictionary<T, IState<T>> _stateByKey = new();

        /// <inheritdoc/>
        public event StateChangeHandler<T> StateChanged;

        /// <inheritdoc/>
        public IState<T> CurrentState { get; private set; }

        /// <inheritdoc/>
        public T? CurrentStateKey { get; private set; }

        /// <summary>
        /// Gets or sets whether the state machine allows state transitions to non-existent states.
        /// </summary>
        /// <remarks>
        /// When enabled, transitioning to a non-existent state will only trigger the <see cref="StateChanged"/> event
        /// without calling state lifecycle methods or throwing an exception.
        /// </remarks>
        public bool AllowMissingStates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
        /// </summary>
        /// <param name="allowMissingStates">Whether to allow transitions to non-existent states. Default is false.</param>
        public StateMachine(bool allowMissingStates = false)
        {
            AllowMissingStates = allowMissingStates;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">
        /// Thrown when a state with the same key already exists in the state machine.
        /// </exception>
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
        /// </summary>
        /// <param name="key">The enum key for the state.</param>
        /// <returns>A <see cref="ChainableState{T}"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when a state with the same key already exists in the state machine.
        /// </exception>
        public ChainableState<T> CreateState(T key)
        {
            var state = new ChainableState<T>();
            AddState(key, state);
            return state;
        }

        /// <inheritdoc/>
        public void RemoveState(T key)
        {
            if (_stateByKey.ContainsKey(key))
            {
                _stateByKey.Remove(key);
            }
        }

        /// <inheritdoc/>
        public IState<T> FindState(T key)
        {
            if (_stateByKey.TryGetValue(key, out var state))
            {
                return state;
            }
            return null;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the state machine has already been started.
        /// Use <see cref="ChangeState"/> to transition to a different state.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the state key does not exist and <see cref="AllowMissingStates"/> is false.
        /// </exception>
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
                CurrentState.OnEnter(this);

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

        /// <inheritdoc/>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the target state key does not exist in the state machine
        /// and <see cref="AllowMissingStates"/> is false.
        /// </exception>
        public void ChangeState(T key)
        {
            if (_stateByKey.TryGetValue(key, out var newState))
            {
                var previousKey = CurrentStateKey;

                if (CurrentState != null)
                {
                    CurrentState.OnExit(this);
                }

                CurrentState = newState;
                CurrentStateKey = key;
                CurrentState.OnEnter(this);

                StateChanged?.Invoke(previousKey, CurrentStateKey.Value);
            }
            else if (AllowMissingStates)
            {
                var previousKey = CurrentStateKey;

                if (CurrentState != null)
                {
                    CurrentState.OnExit(this);
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

        /// <inheritdoc/>
        public void Update()
        {
            CurrentState?.OnUpdate(this);
        }

        /// <inheritdoc/>
        public void FixedUpdate()
        {
            CurrentState?.OnFixedUpdate(this);
        }
    }
}
