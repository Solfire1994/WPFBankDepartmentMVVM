using System;
using WPFBankDepartmentMVVM.Models.AccountBase;
using WPFBankDepartmentMVVM.Models.EmployeeBase;

namespace WPFBankDepartmentMVVM.Models.ClientBase
{
    internal class ClientFinanceChanges : ClientChanges
    {
        public ClientFinanceChanges(IAccount accountType, int sum, Employee employeeType) 
        {
            idTypeChange = 2;
            changedEmployee = "Данные изменял консультант";
            if (changedEmployee is Manager) this.changedEmployee = "Данные изменял Менеджер";
            lastUpdate = DateTime.Now;
            newChangedData = $"Конечный остаток на счете: {accountType.GetValue} рублей";
            changedDataType = $"Произошло поступление средств на: {accountType.name}";
            oldChangedData = $"Сумма поступления: {sum}";
            if (sum < 0)
            {
                changedDataType = $"Произошло списание средств c: {accountType.name}";
                oldChangedData = $"Сумма списания: {sum * -1}";
            }            
        }
        public ClientFinanceChanges(string changedDataType, string oldChangedData, string newChangedData, string changedEmployee, DateTime lastUpdate)
        {
            this.changedDataType = changedDataType;
            this.oldChangedData = oldChangedData;
            this.newChangedData = newChangedData;
            this.changedEmployee = changedEmployee;
            this.lastUpdate = lastUpdate;
            idTypeChange = 2;
        }
    }
}
