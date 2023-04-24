using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.Services.Implementations
{
    class MessageBusService : IMessageBus
    {
        public IDisposable RegesterHandler<T>(Action<T> handler)
        {
            throw new NotImplementedException();
        }

        public void Send<T>(T message)
        {
            throw new NotImplementedException();
        }
    }
}
