using System;
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

        #endregion

        #region Конструкторы
        public ClientWindowViewModel()
        {
            SaveChangingClientCommand = new BaseCommand(OnSaveChangingClientCommandExecuted, CanSaveChangingClientCommandExecute);
            OpenDepositAccountCommand = new BaseCommand(OnOpenDepositAccountCommandExecuted, CanOpenDepositAccountCommandExecute);
            OpenNonDepositAccountCommand = new BaseCommand(OnOpenNonDepositAccountCommandExecuted, CanOpenNonDepositAccountCommandExecute);
        }

        public ClientWindowViewModel(IUserDialog userDialog, IMessageBus messageBus) : this()
        {
            _UserDialog = userDialog;
            _MessageBus = messageBus;
            _SubscriptionClient = messageBus.RegesterHandler<Client>(OnReceiveClient);
            _SubscriptionAuth = messageBus.RegesterHandler<Employee>(OnReceiveEmployee);
        }

        #endregion


        
    }
}
