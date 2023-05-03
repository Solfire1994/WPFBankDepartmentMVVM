using System;
using System.Collections.ObjectModel;
using WPFBankDepartmentMVVM.Models.AccountBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class TransferWindowViewModel : DialogViewModel
    {
        private readonly IMessageBus _MessageBus = null!;
        private readonly IDisposable _SubscriptionAccount = null!;

        #region Список доступных счетов
        private ObservableCollection<IAccount> allAccounts;

        public ObservableCollection<IAccount> AllAccounts
        {
            get { return allAccounts; }
            set => Set(ref allAccounts, value, nameof(AllAccounts));
        }
        #endregion

        #region Метод для передачи между окнами списка счетов        
        private void OnReciveAccount(ObservableCollection<IAccount> message)
        {
            AllAccounts = message;
        }
        #endregion

        public TransferWindowViewModel()
        {

        }

        public TransferWindowViewModel(IMessageBus messageBus): this()
        {
            _MessageBus = messageBus;
            _SubscriptionAccount = messageBus.RegesterHandler<ObservableCollection<IAccount>>(OnReciveAccount);
        }
    }
}
