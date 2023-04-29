using System.Collections.ObjectModel;
using WPFBankDepartmentMVVM.Models.AccountBase;

namespace WPFBankDepartmentMVVM.Models.ClientBase
{
    internal class Client
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string middleName { get; set; }
        public string phoneNumber { get; set; }
        public string passportNumber { get; set; }
        private ObservableCollection<ClientChanges> clientChanges;
        public ObservableCollection<ClientChanges> ClientChanges
        {
            get { return clientChanges; }
            set => clientChanges = value;
        }

        private DepositAccount depositAccount;

        public DepositAccount DepositAccount
        {
            get { return depositAccount; }
            set { depositAccount = value; }
        }

        private NonDepositAccount nonDepositAccount;

        public NonDepositAccount NonDepositAccount
        {
            get { return nonDepositAccount; }
            set { nonDepositAccount = value; }
        }

        public Client()
        {
            lastName = "Список клиентов пуст,";
            firstName = "менеджеру необходимо";
            middleName = "добавить новых клиентов";
        }

        public Client(int id, string lastName, string firstName, string middleName, string phoneNumber, string passportNumber)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.middleName = middleName;
            this.phoneNumber = phoneNumber;
            this.passportNumber = passportNumber;
            clientChanges = new ObservableCollection<ClientChanges>();

        }

        public Client(int id, string lastName, string firstName, string middleName, string phoneNumber, string passportNumber, ObservableCollection<ClientChanges> clientChanges)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.middleName = middleName;
            this.phoneNumber = phoneNumber;
            this.passportNumber = passportNumber;
            this.clientChanges = clientChanges;
        }
    }
}
