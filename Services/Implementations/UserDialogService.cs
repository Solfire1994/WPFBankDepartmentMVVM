using Microsoft.Extensions.DependencyInjection;
using System;
using WPFBankDepartmentMVVM.View;

namespace WPFBankDepartmentMVVM.Services.Implementations
{
    class UserDialogService : IUserDialog
    {
        private readonly IServiceProvider _Servises;
        public UserDialogService(IServiceProvider servises) => _Servises = servises;

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

        public void OpenChangedLogWindow()
        {
            throw new NotImplementedException();
        }

        

        

        public void OpenTransferWindow()
        {
            throw new NotImplementedException();
        }

        
    }
}
