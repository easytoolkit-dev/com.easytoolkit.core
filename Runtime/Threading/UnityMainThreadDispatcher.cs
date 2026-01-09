using System;
using EasyToolKit.Core.Lifecycle;

namespace EasyToolKit.Core.Threading
{
    [MonoSingletonConfig(MonoSingletonFlags.DontDestroyOnLoad)]
    public class UnityMainThreadDispatcher : MonoSingleton<UnityMainThreadDispatcher>
    {
        private readonly object _lock = new object();
        private Action _pendingActions;

        private UnityMainThreadDispatcher()
        {
        }

        public void Enquence(Action action)
        {
            lock (_lock)
            {
                _pendingActions += action;
            }
        }

        void Update()
        {
            Action temp;
            lock (_lock)
            {
                temp = _pendingActions;
                _pendingActions = null;
            }

            temp?.Invoke();
        }
    }
}
