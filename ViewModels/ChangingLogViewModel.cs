using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.ClientBase;
using WPFBankDepartmentMVVM.Models.EmployeeBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class ChangingLogViewModel : DialogViewModel
    {
        private readonly IMessageBus _MessageBus = null!;
        private readonly IDisposable _SubscriptionClientChanges = null!;

        private ObservableCollection<ClientChanges> selectedClientChanges;

        public ObservableCollection<ClientChanges> SelectedClientChanges
        {
            get { return selectedClientChanges; }
            set
            {
                Set(ref selectedClientChanges, value, nameof(SelectedClientChanges));
            }
        }

        private void OnReceiveClientChanges(ObservableCollection<ClientChanges> message)
        {
            SelectedClientChanges = message;
        }

        public ICommand CloseWindowCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;

        private void OnCloseWindowCommandExecuted(object p)
        {
            OnDialogComplete(EventArgs.Empty);
        }

        public ChangingLogViewModel() 
        {
            CloseWindowCommand = new BaseCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }

        public ChangingLogViewModel(IMessageBus messageBus) : this ()
        {
            _MessageBus = messageBus;
            _SubscriptionClientChanges = messageBus.RegesterHandler<ObservableCollection<ClientChanges>>(OnReceiveClientChanges);
        }

    }
}
