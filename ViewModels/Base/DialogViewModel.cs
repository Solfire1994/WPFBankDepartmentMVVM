using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.ViewModels.Base
{
    class DialogViewModel : ViewModelBase
    {
        public event EventHandler? DialogComplete;

        protected virtual void OnDialogComplete(EventArgs e) => DialogComplete?.Invoke(this, e);
    }
}
