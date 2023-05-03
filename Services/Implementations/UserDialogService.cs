using Microsoft.Extensions.DependencyInjection;
using System;
using WPFBankDepartmentMVVM.View;

namespace WPFBankDepartmentMVVM.Services.Implementations
{
    class UserDialogService : IUserDialog
    {
        private readonly IServiceProvider _Servises;
        public UserDialogService(IServiceProvider servises) => _Servises = servises;

        #region MainWindow
        private MainWindow? _MainWindow;
        public void OpenMainWindow()
        {
            if (_MainWindow is { } window)
            {
                window.Show();
                return;
            }
            window = _Servises.GetRequiredService<MainWindow>();
            window.Closed += (_, _) => _MainWindow = null;
            _MainWindow = window;
            window.Show();
        }
        #endregion

        #region AuthWindow
        private AuthWindow? _AuthWindow;
        public void OpenAuthWindow()
        {
            if (_AuthWindow is { } window)
            {
                window.Show();
                return;
            }
            window = _Servises.GetRequiredService<AuthWindow>();
            window.Closed += (_, _) => _AuthWindow = null;
            _AuthWindow = window;
            window.ShowDialog();            
        }
        #endregion

        #region AddNewClientWindow
        private AddNewClient? _AddNewClient;
        public void OpenAddNewClientWindow()
        {
            if (_AddNewClient is { } window)
            {
                window.ShowDialog();
                return;
            }
            window = _Servises.GetRequiredService<AddNewClient>();
            window.Closed += (_, _) => _AddNewClient = null;
            _AddNewClient = window;
            window.ShowDialog();
        }
        #endregion

        #region ClientWindow
        private ClientWindow? _ClientWindow;
        public void CreateClientWindow()
        {
            if (_ClientWindow is { } window)
            {
                window.ShowDialog();
                return;
            }
            window = _Servises.GetRequiredService<ClientWindow>();
            window.Closed += (_, _) => _ClientWindow = null;
            _ClientWindow = window;
        }
        public void OpenClientWindow()
        {
            _ClientWindow.ShowDialog();
        }
        #endregion

        #region ChangingLogWindow
        private ChangingLog? _ChangingLog;
        public void CreateChangingLogWindow()
        {
            if (_ChangingLog is { } window)
            {
                window.ShowDialog();
                return;
            }
            window = _Servises.GetRequiredService<ChangingLog>();
            window.Closed += (_, _) => _ChangingLog = null;
            _ChangingLog = window;
        }
        public void OpenChangingLogWindow()
        {
            _ChangingLog.ShowDialog();
        }
        #endregion

        #region TransferWindow
        private TransferWindow? _TransferWindow;
        public void CreateTransferWindow()
        {
            if (_TransferWindow is { } window)
            {
                window.ShowDialog();
                return;
            }
            window = _Servises.GetRequiredService<TransferWindow>();
            window.Closed += (_, _) => _TransferWindow = null;
            _TransferWindow = window;
        }
        public void OpenTransferWindow()
        {
            _TransferWindow.ShowDialog();
        }
        #endregion

        #region TopUpOrTransferToYourself
        private TopUpOrTransferToYourself? _TopUpOrTransferToYourself;
        public void CreateTopUpOrTransferToYourselfWindow()
        {
            if (_TopUpOrTransferToYourself is { } window)
            {
                window.ShowDialog();
                return;
            }
            window = _Servises.GetRequiredService<TopUpOrTransferToYourself>();
            window.Closed += (_, _) => _TopUpOrTransferToYourself = null;
            _TopUpOrTransferToYourself = window;
        }
        public void OpenTopUpOrTransferToYourselfWindow()
        {
            _TopUpOrTransferToYourself.ShowDialog();
        }
        #endregion
    }
}
