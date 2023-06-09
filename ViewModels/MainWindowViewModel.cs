﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.AccountBase;
using WPFBankDepartmentMVVM.Models.ClientBase;
using WPFBankDepartmentMVVM.Models.EmployeeBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class MainWindowViewModel : DialogViewModel, IDisposable
    {
        #region Свойста и поля

        private const string pathClient = @"Clients.txt";
        private List<Client> workClientsList;
        private readonly IUserDialog _UserDialog = null!;
        private readonly IMessageBus _MessageBus = null!;
        private readonly IDisposable _SubscriptionAuth = null!;
        private readonly IDisposable _SubscriptionClient = null!;
        private readonly IDisposable _SubscriptionClientChanges = null!;
        private readonly IDisposable _SubscriptionAccount = null!;
        private int maxClientID = 0;
        private bool clientIsChanged = true;

        #region Видимый список клиентов
        private ObservableCollection<Client> viewClientList;

        public ObservableCollection<Client> ViewClientList
        {
            get 
            {
                if (viewClientList.Count() == 0) return new() { new() };
                return viewClientList; 
            }
            set => Set(ref viewClientList, value, nameof(ViewClientList));
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
                ChangingEmployeeType();
                viewClientList = GetViewClientList();
                AuthLabel = Visibility.Hidden;
                AuthList = 0;
                SortingComboBox = 0;
            }
        }
        #endregion

        #region Данные статус бара
        private string statusBar = "Авторизация не выполнена";

        public string StatusBar
        {
            get { return statusBar; }
            set => Set(ref statusBar, value, nameof(StatusBar));            
        }
        #endregion

        #region Отображение лейбла авторизации в главной области
        private Visibility authLabel = 0;

        public Visibility AuthLabel
        {
            get { return authLabel; }
            set => Set(ref authLabel, value, nameof(AuthLabel));
        }

        private Visibility authList = (Visibility)1;

        public Visibility AuthList
        {
            get { return authList; }
            set => Set(ref authList, value, nameof(AuthList));
        }
        #endregion

        #region Чек бокс для сортировки
        private byte sortingComboBox;

        public byte SortingComboBox
        {
            get { return sortingComboBox; }
            set
            {
                Set(ref sortingComboBox, value, nameof(SortingComboBox));
                SortingClientList();
            }
        }
        #endregion

        #region Выбранный клиент
        private Client selectedClient;

        public Client SelectedClient
        {
            get { return selectedClient; }
            set => Set(ref selectedClient, value, nameof(SelectedClient));
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

        #endregion

        #region Команды

        #region Открытие окна авторизации
        public ICommand OpenAuthWindowCommand { get; }
        private bool CanOpenAuthWindowCommandExecute(object p) => true;

        private void OnOpenAuthWindowCommandExecuted(object p)
        {
            _UserDialog.OpenAuthWindow();           
        }
        #endregion

        #region Открытие окна добавления нового клиента
        public ICommand OpenAddNewClientWindowCommand { get; }
        private bool CanOpenAddNewClientWindowCommandExecute(object p) => employeeType is Manager;

        private void OnOpenAddNewClientWindowCommandExecuted(object p)
        {
            _UserDialog.OpenAddNewClientWindow();
        }
        #endregion

        #region Удаление выбранного клиента
        public ICommand DeleteClientCommand { get; }
        private bool CanDeleteClientCommandExecute(object p)
        {
            if (p is Client) return employeeType is Manager;
            return false;
        }
        private void OnDeleteClientCommandExecuted(object p)
        {
            DeleteClient((Client)p);
        }
        #endregion

        #region Открытие окна клиента
        public ICommand OpenClientWindowCommand { get; }
        private bool CanOpenClientWindowCommandExecute(object p) => p is Client;

        private void OnOpenClientWindowCommandExecuted(object p)
        {
            _UserDialog.CreateClientWindow();
            _MessageBus.Send((Client)p);
            _MessageBus.Send(EmployeeType);
            _MessageBus.Send(AllAccounts);
            SelectedClient = (Client)p;
            _UserDialog.OpenClientWindow();
            
        }
        #endregion

        #endregion

        #region Методы

        #region Методы получения workClientList
        private void GetClients()
        {
            workClientsList = new List<Client>();
            if (!File.Exists(pathClient)) return;            
            StreamReader sr = new StreamReader(pathClient);
            string[] result;            
            string line = sr.ReadLine();
            int i = 0;
            while (line != null)
            {
                result = line.Split('#');
                workClientsList.Add(new Client(int.Parse(result[0]), result[1], result[2],
                    result[3], result[4], result[5]));
                line = sr.ReadLine();
                if (File.Exists($@"ClientChanges\Changes_{workClientsList[i].id}.txt"))
                    GetClientsChanges($@"ClientChanges\Changes_{workClientsList[i].id}.txt", i);
                if (File.Exists($@"ClientFinance\Finance_{workClientsList[i].id}.txt"))
                    GetClientsFinance($@"ClientFinance\Finance_{workClientsList[i].id}.txt", i);
                maxClientID = workClientsList[i].id <= maxClientID ? maxClientID : workClientsList[i].id;
                i++;
            }
            sr.Close();
        }

        private void GetClientsChanges(string path, int index)
        {
            StreamReader sr = new StreamReader(path);
            string[] result;
            string line = sr.ReadLine();
            while (line != null)
            {
                result = line.Split('#');
                switch (result[0])
                {
                    case "0": workClientsList[index].ClientChanges.Add(new ClientChanges(result[1], result[2], 
                        DateTime.Parse(result[3]))); break;
                    case "1": workClientsList[index].ClientChanges.Add(new ClientChanges(result[1], result[2], result[3],
                        result[4], DateTime.Parse(result[5]))); break;
                    case "2": workClientsList[index].ClientChanges.Add(new ClientFinanceChanges(result[1], result[2], result[3],
                        result[4], DateTime.Parse(result[5]))); break;
                    default: workClientsList[index].ClientChanges.Add(new ClientFinanceChanges(int.Parse(result[0]), result[1], 
                        result[2], DateTime.Parse(result[3]))); break;                        
                }                
                line = sr.ReadLine();
            }
            sr.Close();
        }

        private void GetClientsFinance(string path, int index)
        {
            StreamReader sr = new StreamReader(path);
            string[] result;
            string line = sr.ReadLine();
            if ( !string.IsNullOrEmpty(line))
            {
                result = line.Split('#');
                workClientsList[index].DepositAccount = new(int.Parse(result[0]), result[1], uint.Parse(result[2]));
            }
            line = sr.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                result = line.Split('#');
                workClientsList[index].NonDepositAccount = new(int.Parse(result[0]), result[1], uint.Parse(result[2]));
            }
            sr.Close();
        }
        #endregion

        #region Метод для получения, либо обновления видимого списка клиентов
        private ObservableCollection<Client> GetViewClientList()
        {
            int i = 0;
            allAccounts = new();
            viewClientList = new ObservableCollection<Client>();
            string consultantPassportView = "*********";
            if (employeeType is Consultant)
            {
                foreach (Client client in workClientsList)
                {
                    viewClientList.Add(new Client(client.id, client.lastName,
                        client.firstName, client.middleName, client.phoneNumber, consultantPassportView, client.ClientChanges));
                    if (workClientsList[i].DepositAccount != null)
                    {
                        AllAccounts.Add(workClientsList[i].DepositAccount);
                        viewClientList[i].DepositAccount = workClientsList[i].DepositAccount;
                    }
                    if (workClientsList[i].NonDepositAccount != null)
                    {
                        AllAccounts.Add(workClientsList[i].NonDepositAccount);
                        viewClientList[i].NonDepositAccount = workClientsList[i].NonDepositAccount;                        
                    }
                    i++;
                }
            }
            if (employeeType is Manager)
            {
                foreach (Client client in workClientsList)
                {
                    viewClientList.Add(new Client(client.id, client.lastName,
                        client.firstName, client.middleName, client.phoneNumber, client.passportNumber, client.ClientChanges));
                    if (workClientsList[i].DepositAccount != null)
                    {
                        AllAccounts.Add(workClientsList[i].DepositAccount);
                        viewClientList[i].DepositAccount = workClientsList[i].DepositAccount;
                    }
                    if (workClientsList[i].NonDepositAccount != null)
                    {
                        AllAccounts.Add(workClientsList[i].NonDepositAccount);
                        viewClientList[i].NonDepositAccount = workClientsList[i].NonDepositAccount;
                    }
                    i++;
                }
            }
            OnPropertyChanged(nameof(ViewClientList));
            return ViewClientList;
        }
        #endregion

        #region Метод изменения данных связаных с авторизацией
        private void ChangingEmployeeType()
        {
            if (employeeType is Manager) StatusBar = "В системе работает менеджер";
            if (employeeType is Consultant) StatusBar = "В системе работает консультант";
        }

        #endregion

        #region Метод для передачи между окнами типа работника в системе        
        private void OnReceiveEmployee(Employee message)
        {
            EmployeeType = message;
        }
        #endregion

        #region Метод для сортировки списка клиентов
        private void SortingClientList()
        {
            IOrderedEnumerable<Client> sortedList;
            switch (sortingComboBox)
            {
                case 1: sortedList = viewClientList.OrderBy(v => v.lastName); break;
                case 2: sortedList = viewClientList.OrderByDescending(v => v.lastName); break;
                case 3: sortedList = viewClientList.OrderBy(v => v.phoneNumber); break;
                default: sortedList = viewClientList.OrderBy(v => v.id); break;

            }
            ObservableCollection<Client> sortedList2 = new ObservableCollection<Client>();
            foreach (Client client in sortedList)
            {
                sortedList2.Add(client);
            }
            ViewClientList = sortedList2;
        }
        #endregion

        #region Метод для передачи между окнами клиента в системе        
        private void OnReceiveClient(Client message)
        {
            if (message.id == 0) AddClient(message);
            else if(clientIsChanged) ChangeClient(message);
            int i = 0;
            foreach (Client _client in workClientsList)
            {
                if (_client.id != message.id) { i++; continue; }
                workClientsList[i].DepositAccount = message.DepositAccount;
                workClientsList[i].NonDepositAccount = message.NonDepositAccount;
                PrintClientFinanceInFile(workClientsList[i]);
                clientIsChanged = true;
                return;
            }
        }
        #endregion

        #region Добавления нового клиента        
        private void AddClient(Client client)
        {
            client.id = ++maxClientID;
            client.ClientChanges = new() { new() };
            workClientsList.Add(client);
            PrintInFile();
            GetViewClientList();
        }
        #endregion

        #region Изменение данных клиента     
        private void ChangeClient(Client client)
        {
            int i = 0 ;
            foreach (Client _client in workClientsList)
            {
                if (_client.id != client.id) { i++; continue; }
                CreateClientChanges(workClientsList[i], client);                
                PrintInFile();
                GetViewClientList();
                return;
            }
        }
        #endregion

        #region Группа методов для записи  данных в файлы
        private void PrintInFile()
        {
            StreamWriter sw = new StreamWriter(pathClient);
            string str;
            for (int i = 0; i < workClientsList.Count; i++)
            {
                str = workClientsList[i].id + "#" + workClientsList[i].lastName +
                    "#" + workClientsList[i].firstName + "#" + workClientsList[i].middleName +
                    "#" + workClientsList[i].phoneNumber + "#" + workClientsList[i].passportNumber;
                sw.WriteLine(str);
                if (workClientsList[i].ClientChanges.Count != 0) PrintChangingInFile(workClientsList[i]);
                if (workClientsList[i].DepositAccount != null || workClientsList[i].NonDepositAccount != null)
                    PrintClientFinanceInFile(workClientsList[i]); 
            }
            sw.Close();
        }

        private void PrintChangingInFile(Client client)
        {
            if (!Directory.Exists($@"ClientChanges")) Directory.CreateDirectory($@"ClientChanges");
            var sw = new StreamWriter($@"ClientChanges\Changes_{client.id}.txt");
            string str;
            for (int i = 0; i < client.ClientChanges.Count; i++)
            {
                str = client.ClientChanges[i].idTypeChange + "#" + client.ClientChanges[i].changedDataType + "#"
                            + client.ClientChanges[i].oldChangedData + "#" + client.ClientChanges[i].newChangedData + "#"
                            + client.ClientChanges[i].changedEmployee + "#" + client.ClientChanges[i].lastUpdate;
                if (client.ClientChanges[i].idTypeChange == 0 || client.ClientChanges[i].idTypeChange == 3 || client.ClientChanges[i].idTypeChange == 4)
                {
                    str = client.ClientChanges[i].idTypeChange + "#" + client.ClientChanges[i].changedDataType + "#"
                            + client.ClientChanges[i].changedEmployee + "#" + client.ClientChanges[i].lastUpdate;
                }
                sw.WriteLine(str);
            }
            sw.Close();
        }

        private void PrintClientFinanceInFile(Client client)
        {
            if (!Directory.Exists($@"ClientFinance")) Directory.CreateDirectory($@"ClientFinance");
            var sw = new StreamWriter($@"ClientFinance\Finance_{client.id}.txt");
            string strDeposit = null;
            string strNonDeposit = null;
            if (client.DepositAccount != null) strDeposit = $"{client.DepositAccount.Id}#" +
                    $"{client.DepositAccount.name}#{client.DepositAccount.GetValue()}";
            if (client.NonDepositAccount != null) strNonDeposit = $"{client.NonDepositAccount.Id}#" +
                    $"{client.NonDepositAccount.name}#{client.NonDepositAccount.GetValue()}";            
            sw.WriteLine(strDeposit);
            sw.WriteLine(strNonDeposit);
            sw.Close();
        }

        #endregion

        #region Dispose
        public void Dispose()
        {
            _SubscriptionAuth.Dispose();
            _SubscriptionClient.Dispose();
            _SubscriptionClientChanges.Dispose();
            _SubscriptionAccount.Dispose();
        }
        #endregion

        #region Удаление выбранного клиента
        public void DeleteClient(Client client)
        {
            foreach (var _client in workClientsList)
            {
                if (_client.id == client.id)
                {
                    workClientsList.Remove(_client);
                    break;
                }
            }
            PrintInFile();
            GetViewClientList();
            if (File.Exists($@"ClientChanges\Changes_{client.id}.txt")) File.Delete($@"ClientChanges\Changes_{client.id}.txt");
            if (File.Exists($@"ClientFinance\Finance_{client.id}.txt")) File.Delete($@"ClientFinance\Finance_{client.id}.txt");
        }
        #endregion

        #region Создание изменений       
        private void CreateClientChanges(Client oldClientData, Client newClientData)
        {
            /* changedDataType
             * 1 - first name
             * 2 - last name
             * 3 - middle name
             * 4 - phone number
             * 5 - passport number
             */
            
            if (oldClientData.firstName != newClientData.firstName)
            {
                ChangeData(oldClientData, newClientData, 1);                
            }
            if (oldClientData.lastName != newClientData.lastName)
            {
                ChangeData(oldClientData, newClientData, 2);
            }
            if (oldClientData.middleName != newClientData.middleName)
            {
                ChangeData(oldClientData, newClientData, 3);
            }
            if (oldClientData.phoneNumber != newClientData.phoneNumber)
            {
                ChangeData(oldClientData, newClientData, 4);
            }
            if (oldClientData.passportNumber != newClientData.passportNumber && EmployeeType is Manager)
            {
                ChangeData(oldClientData, newClientData, 5);
            }
        }

        private void ChangeData(Client oldClientData, Client newClientData, int changedDataType)
        {
            switch (changedDataType)
            {
                case 1:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменено имя", oldClientData.firstName,
                    newClientData.firstName, EmployeeType));
                    oldClientData.firstName = newClientData.firstName;
                    break;
                case 2:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменена фамилия", oldClientData.lastName,
                    newClientData.lastName, EmployeeType));
                    oldClientData.lastName = newClientData.lastName;
                    break;
                case 3:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменено отчество", oldClientData.middleName,
                    newClientData.middleName, EmployeeType));
                    oldClientData.middleName = newClientData.middleName;
                    break;
                case 4:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменен номер телефона", oldClientData.phoneNumber,
                    newClientData.phoneNumber, EmployeeType));
                    oldClientData.phoneNumber = newClientData.phoneNumber;
                    break;
                case 5:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменен номер паспорта", oldClientData.passportNumber,
                    newClientData.passportNumber, EmployeeType));
                    oldClientData.passportNumber = newClientData.passportNumber;
                    break;
            }
        }

        #endregion

        #region Метод для передачи между окнами изменений клиента в системе        
        private void OnReceiveClientChanges(ClientFinanceChanges message)
        {
            clientIsChanged = false;
            selectedClient.ClientChanges.Add(message);
            PrintChangingInFile(selectedClient);
            _MessageBus.Send(selectedClient);
        }
        #endregion

        #region Метод для передачи между окнами измененного счета клиента в системе        
        private void OnReceiveAccount(IAccount message)
        {
            foreach (var account in AllAccounts)
            {
                if(account.name == message.name)
                {
                    for (int i = 0; i< workClientsList.Count; i++)
                    {
                        if (workClientsList[i].id == message.Id)
                        {
                            if(message is DepositAccount)
                            {
                                workClientsList[i].ClientChanges.Add(new ClientFinanceChanges(message,
                                    (int)(message.GetValue() - workClientsList[i].DepositAccount.GetValue()), employeeType));
                                workClientsList[i].DepositAccount = (DepositAccount)message;                                
                            }
                            if (message is NonDepositAccount)
                            {
                                workClientsList[i].ClientChanges.Add(new ClientFinanceChanges(message,
                                    (int)(message.GetValue() - workClientsList[i].NonDepositAccount.GetValue()), employeeType));
                                workClientsList[i].NonDepositAccount = (NonDepositAccount)message;                                
                            }
                            PrintChangingInFile(workClientsList[i]);
                            PrintClientFinanceInFile(workClientsList[i]);
                            viewClientList = GetViewClientList();
                            break;
                        }
                    }
                    break;
                }
            }
        }
        #endregion



        #endregion

        #region Конструкторы
        public MainWindowViewModel()
        {
            GetClients();
            viewClientList = GetViewClientList();
            OpenAuthWindowCommand = new BaseCommand(OnOpenAuthWindowCommandExecuted, CanOpenAuthWindowCommandExecute);
            OpenAddNewClientWindowCommand = new BaseCommand(OnOpenAddNewClientWindowCommandExecuted, CanOpenAddNewClientWindowCommandExecute);
            DeleteClientCommand = new BaseCommand(OnDeleteClientCommandExecuted, CanDeleteClientCommandExecute);
            OpenClientWindowCommand = new BaseCommand(OnOpenClientWindowCommandExecuted, CanOpenClientWindowCommandExecute);               
        }

        public MainWindowViewModel(IUserDialog userDialog, IMessageBus messageBus) : this()
        {
            _UserDialog = userDialog;
            _MessageBus = messageBus;
            _SubscriptionAuth = messageBus.RegesterHandler<Employee>(OnReceiveEmployee);
            _SubscriptionClient = messageBus.RegesterHandler<Client>(OnReceiveClient);
            _SubscriptionClientChanges = messageBus.RegesterHandler<ClientFinanceChanges>(OnReceiveClientChanges);
            _SubscriptionAccount = messageBus.RegesterHandler<IAccount>(OnReceiveAccount);
        }
        #endregion        
    }
}
