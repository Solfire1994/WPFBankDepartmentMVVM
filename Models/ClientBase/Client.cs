using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
