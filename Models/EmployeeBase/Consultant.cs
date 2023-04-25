using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBankDepartmentMVVM.Models.ClientBase;

namespace WPFBankDepartmentMVVM.Models.EmployeeBase
{
    internal class Consultant : Employee
    {     

        public override List<Client> ChangeClients(Client client, List<Client> clients)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].id == client.id && CheckChangedField(clients[i], client))
                {
                    clients[i].phoneNumber = client.phoneNumber;
                    break;
                }
            }
            return clients;
        }

        protected override bool CheckChangedField(Client oldClientData, Client newClientData)
        {
            /* changedDataType
             * 4 - phone number
             */
            bool checkField = false;

            if (oldClientData.phoneNumber != newClientData.phoneNumber && !string.IsNullOrEmpty(newClientData.phoneNumber))
            {
                ChangeData(oldClientData, newClientData, 4);
                checkField = true;
            }
            return checkField;
        }

        public Consultant()
        {
            AccessRights = new AccessRights(true, true, true, false, true);
        }
    }
}
