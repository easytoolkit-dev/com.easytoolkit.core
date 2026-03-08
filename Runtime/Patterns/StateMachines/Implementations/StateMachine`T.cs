using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyToolkit.Core.Patterns.Implementations
{
    /// <summary>
    /// A generic State Machine implementation where states are identified by an enum type.
    /// </summary>
    /// <typeparam name="T">The enum type identifying the state.</typeparam>
    /// <remarks>
    /// This implementation enforces strict state validation - all states must be registered via
    /// <see cref="AddState"/> before they can be used. For a lenient implementation that allows
    /// transitions to non-existent states, use <see cref="LenientStateMachine{T}"/>.
    /// </remarks>
    public class StateMachine<T> : IStateMachine<T>, IStateMachine where T : struct, Enum
    {
        private readonly Dictionary<T, IState<T>> _stateByKey = new();
        private StateChangeHandler _untypedStateChangeHandler;

        /// <inheritdoc/>
        public event StateChangeHandler<T> StateChanged;

        /// <inheritdoc/>
        public IState<T> CurrentState { get; protected set; }

        /// <inheritdoc/>
        public T? CurrentStateKey { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
        /// </summary>
        public StateMachine()
        {
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">
        /// Thrown when a state with the same key already exists in the state machine.
        /// </exception>
        public void AddState(T key, IState<T> state)
        {
            if (!_stateByKey.TryAdd(key, state))
            {
                throw new ArgumentException($"State {key} already exists in StateMachine. Cannot add duplicate states.",
                    nameof(key));
            }
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
            return _stateByKey.GetValueOrDefault(key);
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the state machine has already been started.
        /// Use <see cref="ChangeState"/> to transition to a different state.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the state key does not exist.
        /// </exception>
        public virtual void StartState(T key)
        {
            if (CurrentState != null)
            {
                throw new InvalidOperationException(
                    $"StateMachine is already running (CurrentState: {CurrentStateKey}). Use ChangeState to transition.");
            }

            if (_stateByKey.TryGetValue(key, out var newState))
            {
                CurrentState = newState;
                CurrentStateKey = key;
                CurrentState.OnEnter(this);

                OnStateChanged(null, key);
            }
            else
            {
                throw new KeyNotFoundException(
                    $"State {key} does not exist in StateMachine! Ensure the state is added before starting.");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the target state key does not exist in the state machine.
        /// </exception>
        public virtual void ChangeState(T key)
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

                OnStateChanged(previousKey, CurrentStateKey.Value);
            }
            else
            {
                throw new KeyNotFoundException(
                    $"State {key} does not exist in StateMachine! Ensure the state is added before transitioning to it.");
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

        /// <summary>
        /// Raises the <see cref="StateChanged"/> event with the specified state transition.
        /// </summary>
        /// <param name="previousStateKey">The previous state key.</param>
        /// <param name="currentStateKey">The current state key.</param>
        protected virtual void OnStateChanged(T? previousStateKey, T currentStateKey)
        {
            StateChanged?.Invoke(previousStateKey, currentStateKey);
            _untypedStateChangeHandler?.Invoke(previousStateKey?.ToString(), currentStateKey.ToString());
        }

        string IStateView.CurrentStateKey => CurrentStateKey?.ToString();

        IState IStateView.CurrentState
        {
            get
            {
                if (CurrentState is not IState castedState)
                {
                    throw new InvalidCastException(
                        "Cannot convert CurrentState to IState. " +
                        "The current state does not implement the non-generic IState interface.");
                }

                return castedState;
            }
        }

        IState IStateView.FindState(string keyName)
        {
            var state = FindState(ValidateKeyName(keyName));
            if (state is not IState castedState)
            {
                throw new InvalidOperationException(
                    $"Cannot convert state '{keyName}' to IState. " +
                    "The state does not implement the non-generic IState interface.");
            }

            return castedState;
        }

        event StateChangeHandler IStateView.StateChanged
        {
            add => _untypedStateChangeHandler += value;
            remove => _untypedStateChangeHandler -= value;
        }

        void IStateMachine.AddState(string keyName, IState state)
        {
            if (state is not IState<T> castedState)
            {
                throw new ArgumentException(
                    $"Cannot add state '{keyName}'. The state must implement IState<{typeof(T).Name}> " +
                    $"to be used with StateMachine<{typeof(T).Name}>. " +
                    "Ensure the state implements the correct generic interface.",
                    nameof(state));
            }
            AddState(ValidateKeyName(keyName), castedState);
        }

        void IStateMachine.RemoveState(string keyName)
        {
            RemoveState(ValidateKeyName(keyName));
        }

        void IStateMachine.StartState(string keyName)
        {
            StartState(ValidateKeyName(keyName));
        }

        void IStateMachine.ChangeState(string keyName)
        {
            ChangeState(ValidateKeyName(keyName));
        }

        void IStateMachine.Update()
        {
            Update();
        }

        void IStateMachine.FixedUpdate()
        {
            FixedUpdate();
        }

        private T ValidateKeyName(string keyName)
        {
            if (Enum.TryParse<T>(keyName, out var key))
            {
                return key;
            }

            throw new ArgumentException(
                $"'{keyName}' is not a valid value for enum type {typeof(T).Name}. " +
                $"The state key must be a valid {typeof(T).Name} enum value. " +
                "Check if the key name matches one of the enum values exactly.",
                nameof(keyName));
        }
    }
}
