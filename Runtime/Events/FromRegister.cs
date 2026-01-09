using System;

namespace EasyToolKit.Core.Events
{
    public interface IFromRegister : IUnregister
    {
    }

    public class FromRegisterGeneric : UnregisterGeneric, IFromRegister
    {
        public FromRegisterGeneric(Action onUnregister) : base(onUnregister)
        {
        }
    }
}
