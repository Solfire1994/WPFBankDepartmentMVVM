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
        void OpenAddClientWindow();
        void OpenChangedLogWindow();
        void OpenClientWindow();
        void OpenTransferWindow();
    }
}
