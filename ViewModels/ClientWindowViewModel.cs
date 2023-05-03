using System;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.AccountBase;
using WPFBankDepartmentMVVM.Models.ClientBase;
using WPFBankDepartmentMVVM.Models.EmployeeBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class ClientWindowViewModel : DialogViewModel
    {
        #region Поля и свойства

        private readonly IUserDialog _UserDialog = null!;
        private readonly IMessageBus _MessageBus = null!;
        private readonly IDisposable _SubscriptionClient = null!;
        private readonly IDisposable _SubscriptionAuth = null!;
        private readonly IDisposable _SubscriptionAccount = null!;
        private readonly IDisposable _SubscriptionValue = null!;
        private bool IsCorrectFields = false;
               
        #region Выбранный клиент
        private Client selectedClient;

        public Client SelectedClient
        {
            get { return selectedClient; }
            set
            {
                Set(ref selectedClient, value, nameof(SelectedClient));
                CheckClientFields();
            }
        }
        #endregion

        #region Тип сотрудника работающий в системе
        private Employee employeeType;

        public Employee EmployeeType
        {
            get { return employeeType; }
            set
            {
                Set(ref employeeType, value, nameof(EmployeeType));
            }
        }
        #endregion

        #region Строка состояния депозитного счета
        private string depositAccountData;

        public string DepositAccountData
        {
            get { return depositAccountData; }
            set
            {
                Set(ref depositAccountData, value, nameof(DepositAccountData));
            }
        }
        #endregion

        #region Строка состояния дебетового счета
        private string nonDepositAccountData;

        public string NonDepositAccountData
        {
            get { return nonDepositAccountData; }
            set
            {
                Set(ref nonDepositAccountData, value, nameof(NonDepositAccountData));
            }
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

        #region Флаг проверки пополнения либо перевода средств
        // 1 Пополнение депозитного счета
        // 2 Пополнение дебетового счета
        // 3 Перевод с депозитного счета
        // 4 Перевод с дебетового счета
        private byte TopUpDeposit = 0;
        #endregion


        #endregion

        #region Команды

        #region Сохранение изменений
        public ICommand SaveChangingClientCommand { get; }
        private bool CanSaveChangingClientCommandExecute(object p) => IsCorrectFields;

        private void OnSaveChangingClientCommandExecuted(object p)
        {
            _MessageBus.Send(selectedClient);
            IsCorrectFields = false;
        }
        #endregion

        #region Открытие окна изменений клиента
        public ICommand OpenClienChangedCommand { get; }
        private bool CanOpenClienChangedCommandExecute(object p) => true;

        private void OnOpenClienChangedCommandExecuted(object p)
        {
            _UserDialog.CreateChangingLogWindow();
            _MessageBus.Send(selectedClient.ClientChanges);
            _UserDialog.OpenChangingLogWindow();
        }
        #endregion

        #region Открытие депозитного счета
        public ICommand OpenDepositAccountCommand { get; }
        private bool CanOpenDepositAccountCommandExecute(object p) => selectedClient.DepositAccount == null;

        private void OnOpenDepositAccountCommandExecuted(object p)
        {
            selectedClient.DepositAccount = new(selectedClient.id);
            DepositAccountData = GetAccountData(selectedClient.DepositAccount);
            _MessageBus.Send(new ClientFinanceChanges(selectedClient.DepositAccount, EmployeeType, true));
            _MessageBus.Send(selectedClient);
        }
        #endregion

        #region Закрытие депозитного счета
        public ICommand CloseDepositAccountCommand { get; }
        private bool CanCloseDepositAccountCommandExecute(object p) => selectedClient.DepositAccount != null;

        private void OnCloseDepositAccountCommandExecuted(object p)
        {
            DepositAccountData = "Счет отсутствует";
            uint sum = selectedClient.DepositAccount.GetAvaibleValue();
            if (sum > 0)
            {
                selectedClient.DepositAccount.TransferMoneyPayment(sum);
                _MessageBus.Send(new ClientFinanceChanges(selectedClient.DepositAccount, (int)-sum, EmployeeType));
                if (selectedClient.NonDepositAccount != null)
                {
                    selectedClient.NonDepositAccount.TransferMoneyReceiving(sum);
                    _MessageBus.Send(new ClientFinanceChanges(selectedClient.NonDepositAccount, (int)sum, EmployeeType));
                }
            }
            _MessageBus.Send(new ClientFinanceChanges(selectedClient.DepositAccount, EmployeeType, false));
            selectedClient.DepositAccount = null;
            _MessageBus.Send(selectedClient);
        }
        #endregion

        #region Открытие недепозитного счета
        public ICommand OpenNonDepositAccountCommand { get; }
        private bool CanOpenNonDepositAccountCommandExecute(object p) => selectedClient.NonDepositAccount == null;

        private void OnOpenNonDepositAccountCommandExecuted(object p)
        {
            selectedClient.NonDepositAccount = new(selectedClient.id);
            NonDepositAccountData = GetAccountData(selectedClient.NonDepositAccount);
            _MessageBus.Send(new ClientFinanceChanges(selectedClient.NonDepositAccount, EmployeeType, true));
            _MessageBus.Send(selectedClient);
        }
        #endregion

        #region Закрытие дебетового счета
        public ICommand CloseNonDepositAccountCommand { get; }
        private bool CanCloseNonDepositAccountCommandExecute(object p) => SelectedClient.NonDepositAccount != null;

        private void OnCloseNonDepositAccountCommandExecuted(object p)
        {
            NonDepositAccountData = "Счет отсутствует";
            uint sum = SelectedClient.NonDepositAccount.GetAvaibleValue();
            if (sum > 0)
            {
                SelectedClient.NonDepositAccount.TransferMoneyPayment(sum);
                _MessageBus.Send(new ClientFinanceChanges(SelectedClient.NonDepositAccount, (int)-sum, EmployeeType));
                if (SelectedClient.DepositAccount != null)
                {
                    SelectedClient.DepositAccount.TransferMoneyReceiving(sum);
                    _MessageBus.Send(new ClientFinanceChanges(SelectedClient.DepositAccount, (int)sum, EmployeeType));
                }
            }
            _MessageBus.Send(new ClientFinanceChanges(SelectedClient.NonDepositAccount, EmployeeType, false));
            SelectedClient.NonDepositAccount = null;
            _MessageBus.Send(selectedClient);
        }
        #endregion

        #region Пополнение счетов
        public ICommand TopUpAccountCommand { get; }
        private bool CanTopUpAccountCommandExecute(object p)
        {
            if (p is DepositAccount) return SelectedClient.DepositAccount != null;
            if (p is NonDepositAccount) return SelectedClient.NonDepositAccount != null;
            return false;
        }

        private void OnTopUpAccountCommandExecuted(object p)
        {
            _UserDialog.CreateTopUpOrTransferToYourselfWindow();
            _MessageBus.Send((uint)0);
            if (p is DepositAccount) TopUpDeposit = 1; 
            if (p is NonDepositAccount) TopUpDeposit = 2;
            _UserDialog.OpenTopUpOrTransferToYourselfWindow();
        }
        #endregion

        #region Перевод между своими счетами
        public ICommand TransferToYourselfCommand { get; }
        private bool CanTransferToYourselfCommandExecute(object p)
        {
            if (SelectedClient.DepositAccount != null && SelectedClient.NonDepositAccount != null)
            {
                if (p is DepositAccount) return SelectedClient.DepositAccount.GetAvaibleValue() > 0;
                if (p is NonDepositAccount) return SelectedClient.NonDepositAccount.GetAvaibleValue() > 0;
            }
            return false;
        }

        private void OnTransferToYourselfCommandExecuted(object p)
        {
            _UserDialog.CreateTopUpOrTransferToYourselfWindow();
            
            if (p is DepositAccount)
            {
                _MessageBus.Send(SelectedClient.DepositAccount.GetAvaibleValue());
                TopUpDeposit = 3;
            }
            if (p is NonDepositAccount)
            {
                _MessageBus.Send(SelectedClient.NonDepositAccount.GetAvaibleValue());
                TopUpDeposit = 4;
            }
            _UserDialog.OpenTopUpOrTransferToYourselfWindow();
        }
        #endregion



        #region Открытие окна для совершения перевода с депозитного счета
        public ICommand OpenTransferFromDepToOtherCommand { get; }
        private bool CanOpenTransferFromDepToOtherCommandExecute(object p) => SelectedClient.DepositAccount != null;

        private void OnOpenTransferFromDepToOtherCommandExecuted(object p)
        {
            _UserDialog.CreateTransferWindow();
            ObservableCollection<IAccount> allAccountsForTransferOtherClient = new();
            foreach (var account in AllAccounts)
            {
                if (account.Id != selectedClient.id) allAccountsForTransferOtherClient.Add(account);
            }
            _MessageBus.Send(allAccountsForTransferOtherClient);
            _UserDialog.OpenTransferWindow();
        }
        #endregion

        #endregion

        #region Методы

        #region Метод для передачи между окнами клиента в системе        
        private void OnReceiveClient(Client message)
        {            
            SelectedClient = message;
            DepositAccountData = "Счет отсутствует";
            NonDepositAccountData = "Счет отсутствует";
            if (message.DepositAccount != null) DepositAccountData = GetAccountData(message.DepositAccount);
            if (message.NonDepositAccount != null) NonDepositAccountData = GetAccountData(message.NonDepositAccount);
        }
        #endregion

        #region Метод для передачи между окнами типа работника в системе        
        private void OnReceiveEmployee(Employee message)
        {
            EmployeeType = message;
        }
        #endregion

        #region Метод для передачи между окнами списка счетов        
        private void OnReciveAccount(ObservableCollection<IAccount> message)
        {
            AllAccounts = message;
        }
        #endregion

        #region Проверка корректонсти ввода полей
        private void CheckClientFields()
        {            
                if (SelectedClient.lastName.Length <= 0 || selectedClient.lastName.Length > 40) IsCorrectFields = false;
                if (SelectedClient.firstName.Length <= 0 || SelectedClient.firstName.Length > 40) IsCorrectFields = false;
                if (SelectedClient.middleName.Length <= 0 || SelectedClient.middleName.Length > 40) IsCorrectFields = false;
                if (SelectedClient.phoneNumber.Length <= 0 || SelectedClient.phoneNumber.Length > 40) IsCorrectFields = false;
                if (SelectedClient.passportNumber.Length <= 0 || SelectedClient.passportNumber.Length > 40) IsCorrectFields = false;
                IsCorrectFields = true;            
        }
        #endregion

        #region Метод для получения названия и суммы на указанном счете        
        private string GetAccountData(IAccount account)
        {
            return $"{account.name} Остаток суммы на счете: {account.GetValue()}";
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _SubscriptionAuth.Dispose();
            _SubscriptionClient.Dispose();  
            _SubscriptionAccount.Dispose();
        }
        #endregion

        #region Метод для передачи между окнами суммы на счете        
        private void OnReceiveValue(uint message)
        {
            if (TopUpDeposit == 1)
            {
                SelectedClient.DepositAccount.TopUpAccount(message);
                DepositAccountData = GetAccountData(SelectedClient.DepositAccount);
                TopUpDeposit = 0;
                _MessageBus.Send(new ClientFinanceChanges(selectedClient.DepositAccount, (int)message, EmployeeType));
                _MessageBus.Send(selectedClient);
            }

            if (TopUpDeposit == 2)
            {
                SelectedClient.NonDepositAccount.TopUpAccount(message);
                NonDepositAccountData = GetAccountData(SelectedClient.NonDepositAccount);
                TopUpDeposit = 0;
                _MessageBus.Send(new ClientFinanceChanges(selectedClient.NonDepositAccount, (int)message, EmployeeType));
                _MessageBus.Send(selectedClient);
            }

            if (TopUpDeposit == 3)
            {
                SelectedClient.DepositAccount.TransferMoneyPayment(message);
                DepositAccountData = GetAccountData(SelectedClient.DepositAccount);
                _MessageBus.Send(new ClientFinanceChanges(selectedClient.DepositAccount, (int)-message, EmployeeType));
                SelectedClient.NonDepositAccount.TransferMoneyReceiving(message);
                NonDepositAccountData = GetAccountData(SelectedClient.NonDepositAccount);
                _MessageBus.Send(new ClientFinanceChanges(selectedClient.NonDepositAccount, (int)message, EmployeeType));
                TopUpDeposit = 0;
                _MessageBus.Send(selectedClient);
            }

            if (TopUpDeposit == 4)
            {
                SelectedClient.NonDepositAccount.TransferMoneyPayment(message);
                NonDepositAccountData = GetAccountData(SelectedClient.NonDepositAccount);
                _MessageBus.Send(new ClientFinanceChanges(selectedClient.NonDepositAccount, (int)-message, EmployeeType));
                SelectedClient.DepositAccount.TransferMoneyReceiving(message);
                DepositAccountData = GetAccountData(SelectedClient.DepositAccount);
                _MessageBus.Send(new ClientFinanceChanges(selectedClient.DepositAccount, (int)message, EmployeeType));
                TopUpDeposit = 0;
                _MessageBus.Send(selectedClient);
            }
        }
        #endregion

        #endregion

        #region Конструкторы
        public ClientWindowViewModel()
        {
            SaveChangingClientCommand = new BaseCommand(OnSaveChangingClientCommandExecuted, CanSaveChangingClientCommandExecute);
            OpenDepositAccountCommand = new BaseCommand(OnOpenDepositAccountCommandExecuted, CanOpenDepositAccountCommandExecute);
            OpenNonDepositAccountCommand = new BaseCommand(OnOpenNonDepositAccountCommandExecuted, CanOpenNonDepositAccountCommandExecute);
            OpenClienChangedCommand = new BaseCommand(OnOpenClienChangedCommandExecuted, CanOpenClienChangedCommandExecute);
            OpenTransferFromDepToOtherCommand = new BaseCommand(OnOpenTransferFromDepToOtherCommandExecuted, CanOpenTransferFromDepToOtherCommandExecute);
            CloseDepositAccountCommand = new BaseCommand(OnCloseDepositAccountCommandExecuted, CanCloseDepositAccountCommandExecute);
            CloseNonDepositAccountCommand = new BaseCommand(OnCloseNonDepositAccountCommandExecuted, CanCloseNonDepositAccountCommandExecute);
            TopUpAccountCommand = new BaseCommand(OnTopUpAccountCommandExecuted, CanTopUpAccountCommandExecute);
            TransferToYourselfCommand = new BaseCommand(OnTransferToYourselfCommandExecuted, CanTransferToYourselfCommandExecute);
        }

        public ClientWindowViewModel(IUserDialog userDialog, IMessageBus messageBus) : this()
        {
            _UserDialog = userDialog;
            _MessageBus = messageBus;
            _SubscriptionClient = messageBus.RegesterHandler<Client>(OnReceiveClient);
            _SubscriptionAuth = messageBus.RegesterHandler<Employee>(OnReceiveEmployee);
            _SubscriptionAccount = messageBus.RegesterHandler<ObservableCollection<IAccount>>(OnReciveAccount);
            _SubscriptionValue = messageBus.RegesterHandler<uint>(OnReceiveValue);
        } 
        #endregion



    }
}
