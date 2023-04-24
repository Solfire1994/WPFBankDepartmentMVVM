using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.Services
{
    interface IMessageBus
    {
        IDisposable RegesterHandler<T>(Action<T> handler);

        void Send<T>(T message);
    }
}
