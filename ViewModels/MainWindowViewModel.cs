using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.ClientBase;
using WPFBankDepartmentMVVM.Models.EmployeeBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region New Work

        #region Свойста и поля

        private const string pathClient = @"Clients.txt";
        private List<Client> workClientsList;
        private readonly IUserDialog _UserDialog;

        #region Видимый список клиентов
        private ObservableCollection<Client> viewClientList;

        public ObservableCollection<Client> ViewClientList
        {
            get { return viewClientList; }
            set => Set(ref viewClientList, value, nameof(ViewClientList));
        }
        #endregion



        #endregion

        #region Команды

        #region Сохранение изменений
        public ICommand OpenAuthWindowCommand { get; }
        private bool CanOpenAuthWindowCommandExecute(object p) => true;

        private void OnOpenAuthWindowCommandExecuted(object p)
        {
            _UserDialog.OpenAuthWindow();
        }
        #endregion

        #endregion

        #region Методы

        #region Методы получения workClientList
        private void GetClients()
        {
            StreamReader sr = new StreamReader(pathClient);
            string[] result;
            workClientsList = new List<Client>();
            string line = sr.ReadLine();
            int i = 0;
            while (line != null)
            {
                result = line.Split('#');
                workClientsList.Add(new Client(int.Parse(result[0]), result[1], result[2],
                    result[3], result[4], result[5]));
                line = sr.ReadLine();
                maxID = maxID < workClientsList[i].id ? maxID + 1 : maxID;
                if (File.Exists($@"Changes_{workClientsList[i].id}.txt")) GetClientsChanges($@"Changes_{workClientsList[i].id}.txt", i);
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
                workClientsList[index].ClientChanges.Add(new ClientChanges(result[0], result[1], result[2],
                        result[3], DateTime.Parse(result[4])));
                line = sr.ReadLine();
            }
            sr.Close();
        }

        #endregion


        /// <summary>
        /// Метод для получения, либо обновления видимого списка клиентов
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<Client> GetViewClientList()
        {
            int i = 0;
            ViewClientList = new ObservableCollection<Client>();
            foreach (Client client in workClientsList)
            {
                ViewClientList.Add(new Client(client.id, client.lastName,
                    client.firstName, client.middleName, client.phoneNumber, client.passportNumber, client.ClientChanges));
                i++;
            }
            return ViewClientList;
        }


        #endregion



        public MainWindowViewModel()
        {
            GetClients();
            viewClientList = GetViewClientList();
            OpenAuthWindowCommand = new BaseCommand(OnOpenAuthWindowCommandExecuted, CanOpenAuthWindowCommandExecute);
            //repository = new Repository(isManager);
            //ClientList = repository.GetAllClient();
            //ViewClientList = GetViewClientList();
            //accessRights = repository.employee.AccessRights;
            //SaveChangingCommand = new BaseCommand(OnSaveChangingCommandExecuted, CanSaveChangingCommandExecute);
            //AddClientCommand = new BaseCommand(OnAddClientCommandExecuted, CanAddClientCommandExecute);
            //DeleteClientCommand = new BaseCommand(OnDeleteClientCommandExecuted, CanDeleteClientCommandExecute);
        }

        public MainWindowViewModel(IUserDialog userDialog) : this()
        {
            _UserDialog = userDialog;
        }
        #endregion

















        #region Свойста и поля



        #region Базовый список клиентов
        private List<Client> сlientList;

        public List<Client> ClientList
        {
            get { return сlientList; }
            set
            {
                сlientList = value;
                OnPropertyChanged("сlientList");
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
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }
        #endregion

        #region Чек бокс для выбора типа сотрудника
        //private byte isManager;

        //public byte IsManager
        //{
        //    get { return isManager; }
        //    set
        //    {
        //        if (isManager != value) { checkStatusManager = true; }
        //        isManager = value;
        //        OnPropertyChanged("IsManager");
        //        if (checkStatusManager) { repository = new Repository(isManager); checkStatusManager = false; }
        //        OnPropertyChanged("Repository");
        //        viewClientList = GetViewClientList();
        //        OnPropertyChanged("ClientList");
        //        accessRights = repository.employee.AccessRights;
        //        OnPropertyChanged("AccessRights");
        //    }
        //}
        #endregion

        #region Репозиторий
        //private Repository repository;
        //public Repository Repository
        //{
        //    get { return repository; }
        //    set
        //    {
        //        repository = value;

        //        OnPropertyChanged("Repository");
        //    }
        //}
        #endregion

        #region Флаг для проверки изменения типа сотрудника
        private bool checkStatusManager = false;
        #endregion

        #region Флаг для проверки прав на изменение поля клиента
        private AccessRights accessRights;

        public AccessRights AccessRights
        {
            get { return accessRights; }
            set
            {
                accessRights = value;
                OnPropertyChanged("AccessRights");
            }
        }
        #endregion

        #region Чек бокс для сортировки
        private byte sortingComboBox;

        public byte SortingComboBox
        {
            get { return sortingComboBox; }
            set
            {
                sortingComboBox = value;
                SortingClientList();
                OnPropertyChanged("SortingComboBox");
            }
        }
        #endregion



        #endregion

        #region Команды

        #region Сохранение изменений
        //public ICommand SaveChangingCommand { get; }
        //private bool CanSaveChangingCommandExecute(object p) => p is Client;

        //private void OnSaveChangingCommandExecuted(object p)
        //{
        //    repository.ChangeClient(selectedClient);
        //    ViewClientList = GetViewClientList();
        //    OnPropertyChanged("ViewClientList");
        //}
        #endregion

        #region Добавление нового клиента
        //public ICommand AddClientCommand { get; }
        //private bool CanAddClientCommandExecute(object p) => isManager == 1;

        //private void OnAddClientCommandExecuted(object p)
        //{
        //    repository.AddClient();
        //    ViewClientList = GetViewClientList();
        //    OnPropertyChanged("ViewClientList");
        //}
        #endregion

        #region Удаление выбранного клиента
        //public ICommand DeleteClientCommand { get; }
        //private bool CanDeleteClientCommandExecute(object p) => p is Client && isManager == 1;

        //private void OnDeleteClientCommandExecuted(object p)
        //{
        //    if (!(p is Client client)) return;
        //    repository.DeleteClient(p as Client);
        //    ViewClientList = GetViewClientList();
        //    OnPropertyChanged("ViewClientList");
        //}
        #endregion

        #endregion

        

        #region Методы
        

        /// <summary>
        /// Метод для сортировки списка клиентов
        /// </summary>
        /// <returns></returns>
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
            viewClientList = sortedList2;
            OnPropertyChanged("ViewClientList");
        }
        #endregion

        #region Repository

        

        public Employee employee { get; }
        
        private int maxID = 0;


        ///// <summary>
        ///// Конструктор репозитория
        ///// </summary>
        ///// <param name="IsManager"></param>
        ////public Repository(byte isManager)
        ////{
        ////    GetClients();
        ////    employee = new Consultant();
        ////    if (isManager == 1) { employee = new Manager(); }
        ////}

        ///// <summary>
        ///// Метод по получению списка всех клиентов
        ///// </summary>
        ///// <returns>Список клиентов</returns>
        //public List<Client> GetAllClient()
        //{
        //    return clients;
        //}

        ///// <summary>
        ///// Метод по выводу списка для просмотра клиентов
        ///// </summary>
        ///// <returns></returns>
        //public List<Client> ViewAllClient()
        //{
        //    return employee.ViewClients(clients);
        //}

        ///// <summary>
        ///// Метод по изменению данных списка клиентов
        ///// </summary>
        ///// <returns></returns>
        //public void ChangeClient(Client client)
        //{
        //    clients = employee.ChangeClients(client, clients);
        //    PrintInFile();
        //}

        ///// <summary>
        ///// Метод по добавлению нового клиента
        ///// </summary>
        ///// <returns></returns>
        //public void AddClient()
        //{
        //    clients.Add(employee.AddClients(maxID + 1));
        //    PrintInFile();
        //}

        ///// <summary>
        ///// Метод по удалению клиента
        ///// </summary>
        ///// <returns></returns>
        //public void DeleteClient(Client client)
        //{
        //    foreach (var _client in clients)
        //    {
        //        if (_client.id == client.id)
        //        {
        //            clients.Remove(_client);
        //            break;
        //        }
        //    }
        //    PrintInFile();
        //}

        #region Группа методов для записи и получения данных из файлов
        //private void PrintInFile()
        //{
        //    StreamWriter sw = new StreamWriter(pathClient);
        //    string str;
        //    for (int i = 0; i < clients.Count; i++)
        //    {
        //        str = clients[i].id + "#" + clients[i].lastName +
        //            "#" + clients[i].firstName + "#" + clients[i].middleName +
        //            "#" + clients[i].phoneNumber + "#" + clients[i].passportNumber;
        //        sw.WriteLine(str);
        //        if (clients[i].ClientChanges.Count != 0) PrintChangingInFile(clients[i]);
        //    }
        //    sw.Close();
        //}

        //private void PrintChangingInFile(Client client)
        //{
        //    StreamWriter sw = new StreamWriter($@"Changes_{client.id}.txt");
        //    string str;
        //    for (int i = 0; i < client.ClientChanges.Count; i++)
        //    {
        //        str = client.ClientChanges[i].changedDataType + "#" + client.ClientChanges[i].oldChangedData + "#"
        //            + client.ClientChanges[i].newChangedData + "#" + client.ClientChanges[i].changedEmployee + "#"
        //            + client.ClientChanges[i].lastUpdate;
        //        sw.WriteLine(str);
        //    }
        //    sw.Close();
        //}


        
        #endregion

        #endregion

    }
}
