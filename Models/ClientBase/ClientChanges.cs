using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBankDepartmentMVVM.Models.EmployeeBase;

namespace WPFBankDepartmentMVVM.Models.ClientBase
{
    internal class ClientChanges
    {
        public DateTime lastUpdate { get; }
        public string changedDataType { get; }
        public string oldChangedData { get; }
        public string newChangedData { get; }
        public string changedEmployee { get; }

        public ClientChanges(string changedDataType, string oldChangedData, string newChangedData, Employee changedEmployee)
        {
            this.changedDataType = changedDataType;
            this.oldChangedData = oldChangedData;
            this.newChangedData = newChangedData;
            this.changedEmployee = "Данные изменял консультант";
            if (changedEmployee is Manager) this.changedEmployee = "Данные изменял Менеджер";
            lastUpdate = DateTime.Now;
        }

        public ClientChanges(string changedDataType, string oldChangedData, string newChangedData, string changedEmployee, DateTime lastUpdate)
        {
            this.changedDataType = changedDataType;
            this.oldChangedData = oldChangedData;
            this.newChangedData = newChangedData;
            this.changedEmployee = changedEmployee;
            this.lastUpdate = lastUpdate;
        }

    }
}
