using System;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.EmployeeBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class AuthWindowViewModel : DialogViewModel
    {
        private readonly IMessageBus _MessageBus = null!;
        private Employee _employeeType = null!;

        #region Вход как консультант
        public ICommand AuthConsultantCommand { get; }
        private bool CanAuthConsultantCommandExecute(object p)
        { 
            if (_employeeType == null) { return true; }
            return _employeeType is Manager;            
        }

        private void OnAuthConsultantCommandExecuted(object p)
        {
            _employeeType = new Consultant();
            _MessageBus.Send(_employeeType);
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        #region Вход как менеджер
        public ICommand AuthManagerCommand { get; }
        private bool CanAuthManagerCommandExecute(object p)
        {
            if (_employeeType == null) { return true; }
            return _employeeType is Consultant;
        }

        private void OnAuthManagerCommandExecuted(object p)
        {
            _employeeType = new Manager();
            _MessageBus.Send(_employeeType);
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        public AuthWindowViewModel()
        {
            AuthConsultantCommand = new BaseCommand(OnAuthConsultantCommandExecuted, CanAuthConsultantCommandExecute);
            AuthManagerCommand = new BaseCommand(OnAuthManagerCommandExecuted, CanAuthManagerCommandExecute);
        }

        public AuthWindowViewModel(IMessageBus messageBus) : this()
        {
            _MessageBus = messageBus;
        }
    }
}
