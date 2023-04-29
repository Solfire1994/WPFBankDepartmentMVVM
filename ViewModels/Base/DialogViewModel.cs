using System;

namespace WPFBankDepartmentMVVM.ViewModels.Base
{
    class DialogViewModel : ViewModelBase
    {
        public event EventHandler? DialogComplete;

        protected virtual void OnDialogComplete(EventArgs e) => DialogComplete?.Invoke(this, e);
    }
}
