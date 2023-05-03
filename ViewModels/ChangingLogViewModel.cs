using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.ClientBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class ChangingLogViewModel : DialogViewModel
    {
        private readonly IDisposable _SubscriptionClientChanges = null!;
        #region Свойство, коллекция изменений
        private ObservableCollection<ClientChanges> selectedClientChanges;

        public ObservableCollection<ClientChanges> SelectedClientChanges
        {
            get { return selectedClientChanges; }
            set
            {
                Set(ref selectedClientChanges, value, nameof(SelectedClientChanges));
            }
        }
        #endregion        
        #region Команда закрытия
        public ICommand CloseWindowCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;

        private void OnCloseWindowCommandExecuted(object p)
        {
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion
        public ChangingLogViewModel() 
        {
            CloseWindowCommand = new BaseCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
        }
        public ChangingLogViewModel(IMessageBus messageBus) : this ()
        {
            _SubscriptionClientChanges = messageBus.RegesterHandler<ObservableCollection<ClientChanges>>(OnReceiveClientChanges);
        }
        private void OnReceiveClientChanges(ObservableCollection<ClientChanges> message)
        {
            SelectedClientChanges = message;
        }

        #region Dispose
        public void Dispose()
        {
            _SubscriptionClientChanges.Dispose();
        }
        #endregion
    }
}
