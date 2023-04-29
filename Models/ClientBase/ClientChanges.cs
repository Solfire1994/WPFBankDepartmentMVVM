using System;
using WPFBankDepartmentMVVM.Models.EmployeeBase;

namespace WPFBankDepartmentMVVM.Models.ClientBase
{
    internal class ClientChanges
    {        
        // Тип изменений
        // 0 Создание клиента
        // 1 Изменение данных
        // 2 Изменения финансового плана
        public int idTypeChange { get; protected set; }
        public DateTime lastUpdate { get; protected set; }
        public string changedDataType { get; protected set; }
        public string oldChangedData { get; protected set; }
        public string newChangedData { get; protected set; }
        public string changedEmployee { get; protected set; }

        public ClientChanges()
        {
            idTypeChange = 0;
            changedDataType = "Клиент создан";
            lastUpdate = DateTime.Now;
            changedEmployee = "Клиента создал Менеджер";
        }
        public ClientChanges(string changedDataType, string changedEmployee, DateTime lastUpdate)
        {
            this.changedDataType = changedDataType;
            this.changedEmployee = changedEmployee;
            this.lastUpdate = lastUpdate;
            idTypeChange = 0;
        }
        public ClientChanges(string changedDataType, string oldChangedData, string newChangedData, Employee changedEmployee)
        {
            this.changedDataType = changedDataType;
            this.oldChangedData = oldChangedData;
            this.newChangedData = newChangedData;
            this.changedEmployee = "Данные изменял консультант";
            if (changedEmployee is Manager) this.changedEmployee = "Данные изменял Менеджер";
            lastUpdate = DateTime.Now;
            idTypeChange = 1;
        }
        public ClientChanges(string changedDataType, string oldChangedData, string newChangedData, string changedEmployee, DateTime lastUpdate)
        {
            this.changedDataType = changedDataType;
            this.oldChangedData = oldChangedData;
            this.newChangedData = newChangedData;
            this.changedEmployee = changedEmployee;
            this.lastUpdate = lastUpdate;
            idTypeChange = 1;
        }
    }
}
