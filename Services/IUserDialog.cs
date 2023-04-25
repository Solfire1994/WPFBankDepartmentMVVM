using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.Services
{
    interface IUserDialog
    {
        void OpenMainWindow();
        void OpenAuthWindow();
        void OpenAddNewClientWindow();
        void OpenChangedLogWindow();
        void CreateClientWindow();
        void OpenClientWindow();
        void OpenTransferWindow();
    }
}
