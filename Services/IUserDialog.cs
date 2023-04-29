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
