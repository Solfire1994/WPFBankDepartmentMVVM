using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public void OpenChangedLogWindow()
        {
            throw new NotImplementedException();
        }

        public void OpenClientWindow()
        {
            throw new NotImplementedException();
        }

        

        public void OpenTransferWindow()
        {
            throw new NotImplementedException();
        }

        public void OpenAddClientWindow()
        {
            throw new NotImplementedException();
        }
    }
}
