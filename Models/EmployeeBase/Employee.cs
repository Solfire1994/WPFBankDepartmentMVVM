using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBankDepartmentMVVM.Models.ClientBase;

namespace WPFBankDepartmentMVVM.Models.EmployeeBase
{
    internal abstract class Employee
    {
        public AccessRights AccessRights { get; protected set; }
        public virtual List<Client> ViewClients(List<Client> clients)
        {
            return clients;
        }

        public virtual List<Client> ChangeClients(Client client, List<Client> clients)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].id == client.id && CheckChangedField(clients[i], client))
                {
                    clients[i] = client;
                    break;
                }
            }
            return clients;
        }
        public virtual Client AddClients(int id)
        {
            string fieldClient = $"Новый клиент_{id} заполните данные";
            Client client = new Client(id, fieldClient, fieldClient, fieldClient, fieldClient, fieldClient);
            return client;
        }

        protected void ChangeData(Client oldClientData, Client newClientData, int changedDataType)
        {
            switch (changedDataType)
            {
                case 1:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменено имя", oldClientData.firstName,
                    newClientData.firstName, this));
                    oldClientData.firstName = newClientData.firstName;
                    break;
                case 2:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменена фамилия", oldClientData.lastName,
                    newClientData.lastName, this));
                    oldClientData.lastName = newClientData.lastName;
                    break;
                case 3:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменено отчество", oldClientData.middleName,
                    newClientData.middleName, this));
                    oldClientData.middleName = newClientData.middleName;
                    break;
                case 4:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменен номер телефона", oldClientData.phoneNumber,
                    newClientData.phoneNumber, this));
                    oldClientData.phoneNumber = newClientData.phoneNumber;
                    break;
                case 5:
                    oldClientData.ClientChanges.Add(new ClientChanges("Изменен номер паспорта", oldClientData.passportNumber,
                    newClientData.passportNumber, this));
                    oldClientData.passportNumber = newClientData.passportNumber;
                    break;
            }
        }

        protected virtual bool CheckChangedField(Client oldClientData, Client newClientData)
        {
            /* changedDataType
             * 1 - first name
             * 2 - last name
             * 3 - middle name
             * 4 - phone number
             * 5 - passport number
             */
            bool checkField = false;
            if (oldClientData.firstName != newClientData.firstName && !string.IsNullOrEmpty(newClientData.firstName))
            {
                ChangeData(oldClientData, newClientData, 1);
                checkField = true;
            }
            if (oldClientData.lastName != newClientData.lastName && !string.IsNullOrEmpty(newClientData.lastName))
            {
                ChangeData(oldClientData, newClientData, 2);
                checkField = true;
            }
            if (oldClientData.middleName != newClientData.middleName && !string.IsNullOrEmpty(newClientData.middleName))
            {
                ChangeData(oldClientData, newClientData, 3);
                checkField = true;
            }
            if (oldClientData.phoneNumber != newClientData.phoneNumber && !string.IsNullOrEmpty(newClientData.phoneNumber))
            {
                ChangeData(oldClientData, newClientData, 4);
                checkField = true;
            }
            if (oldClientData.passportNumber != newClientData.passportNumber && !string.IsNullOrEmpty(newClientData.passportNumber))
            {
                ChangeData(oldClientData, newClientData, 5);
                checkField = true;
            }
            return checkField;
        }
    }    
}
