using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.AccountBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class TransferWindowViewModel : DialogViewModel, IDisposable
    {
        private readonly IMessageBus _MessageBus = null!;
        private readonly IDisposable _SubscriptionAccount = null!;
        private readonly IDisposable _SubscriptionValue = null!;
        bool IsValueCorrect = false;

        #region Данные текст блока
        private string statusBar;

        public string StatusBar
        {
            get { return statusBar; }
            set => Set(ref statusBar, value, nameof(StatusBar));
        }
        #endregion

        #region Список доступных счетов
        private ObservableCollection<IAccount> allAccounts;

        public ObservableCollection<IAccount> AllAccounts
        {
            get { return allAccounts; }
            set => Set(ref allAccounts, value, nameof(AllAccounts));
        }
        #endregion

        #region Обозначение проверки корректности ввода
        private Thickness correctWeight;

        public Thickness CorrectWeight
        {
            get { return correctWeight; }
            set => Set(ref correctWeight, value, nameof(CorrectWeight));
        }
        #endregion

        #region Сумма
        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                Set(ref _value, value, nameof(Value));
                CheckValue();
            }
        }
        #endregion

        #region Доступная сумма
        private uint maxValue;

        public uint MaxValue
        {
            get { return maxValue; }
            set => Set(ref maxValue, value, nameof(MaxValue));
        }
        #endregion

        #region Выбранный счет для перевода
        private IAccount selectedAccount;

        public IAccount SelectedAccount
        {
            get { return selectedAccount; }
            set => Set(ref selectedAccount, value, nameof(SelectedAccount));
        }
        #endregion

        #region Метод для передачи между окнами списка счетов        
        private void OnReciveAccount(ObservableCollection<IAccount> message)
        {
            AllAccounts = message;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _SubscriptionAccount.Dispose();
            _SubscriptionValue.Dispose();
        }
        #endregion
        
        #region Метод для передачи между окнами суммы на счете        
        private void OnReceiveValue(uint message)
        {
            MaxValue = message;
            StatusBar = $"Для перевода введите сумму не более {message} рублей";
            _SubscriptionValue.Dispose();
        }
        #endregion

        #region Проверка корректности ввода
        public void CheckValue()
        {
            if (uint.TryParse(_value, out uint res) && res <= maxValue)
            {
                IsValueCorrect = true;
                CorrectWeight = new(0);
            }
            else
            {
                IsValueCorrect = false;
                CorrectWeight = new(1.5);
            }

        }
        #endregion

        #region Перечислить
        public ICommand TransferCommand { get; }
        private bool CanTransferCommandExecute(object p) => IsValueCorrect;


        private void OnTransferCommandExecuted(object p)
        {
            _MessageBus.Send(uint.Parse(Value));
            IAccount sendedAccount = null;
            if (selectedAccount is DepositAccount) sendedAccount = new DepositAccount(selectedAccount.Id, selectedAccount.GetValue());
            if (selectedAccount is NonDepositAccount) sendedAccount = new NonDepositAccount(selectedAccount.Id, selectedAccount.GetValue());
            sendedAccount.TransferMoneyReceiving(uint.Parse(Value));
            _MessageBus.Send(sendedAccount);
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        public TransferWindowViewModel()
        {
            TransferCommand = new BaseCommand(OnTransferCommandExecuted, CanTransferCommandExecute);
        }

        public TransferWindowViewModel(IMessageBus messageBus): this()
        {
            _MessageBus = messageBus;
            _SubscriptionAccount = messageBus.RegesterHandler<ObservableCollection<IAccount>>(OnReciveAccount);
            _SubscriptionValue = messageBus.RegesterHandler<uint>(OnReceiveValue);
        }
    }
}
