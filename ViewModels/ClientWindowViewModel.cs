using System;
using System.Collections.Generic;
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
    internal class ClientWindowViewModel : DialogViewModel
    {
        #region Поля и свойства
        private readonly IUserDialog _UserDialog = null!;
        private readonly IMessageBus _MessageBus = null!;        
        private readonly IDisposable _SubscriptionClient = null!;
        private readonly IDisposable _SubscriptionAuth = null!;
        private bool IsCorrectFields = false;

        #region Первоначальные данные клиента
        private Client startClient;

        public Client StartClient
        {
            get { return startClient; }
            set
            {
                Set(ref startClient, value, nameof(StartClient));
            }
        }
        #endregion

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

        #endregion

        #region Команды

        #region Сохранение изменений
        public ICommand SaveChangingClientCommand { get; }
        private bool CanSaveChangingClientCommandExecute(object p) => IsCorrectFields;

        private void OnSaveChangingClientCommandExecuted(object p)
        {
            _MessageBus.Send(selectedClient);
            StartClient.lastName = SelectedClient.lastName;
            StartClient.firstName = SelectedClient.firstName;
            StartClient.middleName = SelectedClient.middleName;
            StartClient.phoneNumber = SelectedClient.phoneNumber;
            StartClient.passportNumber = SelectedClient.passportNumber;
            IsCorrectFields = false;
        }
        #endregion

        #endregion

        #region Методы

        #region Метод для передачи между окнами клиента в системе        
        private void OnReceiveClient(Client message)
        {            
            startClient = new(message.id, message.lastName, message.firstName, message.middleName, message.phoneNumber, message.passportNumber);
            SelectedClient = message;
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
            if (StartClient.lastName != SelectedClient.lastName ||
            StartClient.firstName != SelectedClient.firstName ||
            StartClient.middleName != SelectedClient.middleName ||
            StartClient.phoneNumber != SelectedClient.phoneNumber ||
            StartClient.passportNumber != SelectedClient.passportNumber)
            {
                if (SelectedClient.lastName.Length <= 0 || selectedClient.lastName.Length > 40) IsCorrectFields = false;
                if (SelectedClient.firstName.Length <= 0 || SelectedClient.firstName.Length > 40) IsCorrectFields = false;
                if (SelectedClient.middleName.Length <= 0 || SelectedClient.middleName.Length > 40) IsCorrectFields = false;
                if (SelectedClient.phoneNumber.Length <= 0 || SelectedClient.phoneNumber.Length > 40) IsCorrectFields = false;
                if (SelectedClient.passportNumber.Length <= 0 || SelectedClient.passportNumber.Length > 40) IsCorrectFields = false;
                IsCorrectFields = true;
            }
            
        }
        #endregion


        #endregion

        #region Конструкторы
        public ClientWindowViewModel()
        {
            SaveChangingClientCommand = new BaseCommand(OnSaveChangingClientCommandExecuted, CanSaveChangingClientCommandExecute);
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
