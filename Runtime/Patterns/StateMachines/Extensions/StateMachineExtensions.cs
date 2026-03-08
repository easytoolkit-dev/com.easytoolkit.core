using System;

namespace EasyToolkit.Core.Patterns
{
    public static class StateMachineExtensions
    {
        public static IChainableState CreateState(this IStateMachine stateMachine, string keyName)
        {
            var state = new Implementations.ChainableState();
            stateMachine.AddState(keyName, state);
            return state;
        }

        public static IChainableState<T> CreateState<T>(this IStateMachine<T> stateMachine, T key)
            where T : struct, Enum
        {
            var state = new Implementations.ChainableState<T>();
            stateMachine.AddState(key, state);
            return state;
        }
    }
}
