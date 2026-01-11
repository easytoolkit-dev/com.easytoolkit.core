using System;
using System.Threading;
using EasyToolKit.Core.Patterns;

namespace EasyToolKit.Core.Threading
{
    [MonoSingletonConfiguration(MonoSingletonFlags.DontDestroyOnLoad)]
    public class UnityMainThreadDispatcher : MonoSingleton<UnityMainThreadDispatcher>
    {
        private readonly object _lock = new object();
        private Action _pendingActions;

        public int? MainThreadId { get; private set; }

        private UnityMainThreadDispatcher()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            MainThreadId = Thread.CurrentThread.ManagedThreadId;
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
