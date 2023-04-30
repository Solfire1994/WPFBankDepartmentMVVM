namespace WPFBankDepartmentMVVM.Services
{
    interface IUserDialog
    {
        void OpenMainWindow();
        void OpenAuthWindow();
        void OpenAddNewClientWindow();        
        void CreateClientWindow();
        void OpenClientWindow();
        void CreateTransferWindow();
        void OpenTransferWindow();
        void CreateChangingLogWindow();
        void OpenChangingLogWindow();
    }
}
