using System;

namespace EasyToolkit.Core.Patterns
{
    public static class StateMachineExtensions
    {
        public static ChainableState CreateState(this StateMachine stateMachine, string keyName)
        {
            var state = new ChainableState();
            stateMachine.AddState(keyName, state);
            return state;
        }

        public static ChainableState<T> CreateState<T>(this StateMachine<T> stateMachine, T key)
            where T : struct, Enum
        {
            var state = new ChainableState<T>();
            stateMachine.AddState(key, state);
            return state;
        }
    }
}
