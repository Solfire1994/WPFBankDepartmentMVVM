using System;

namespace WPFBankDepartmentMVVM.Services
{
    interface IMessageBus
    {
        IDisposable RegesterHandler<T>(Action<T> handler);

        void Send<T>(T message);
    }
}
