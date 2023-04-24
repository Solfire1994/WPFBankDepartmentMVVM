using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBankDepartmentMVVM.Models.EmployeeBase
{
    internal class Manager : Employee
    {
        public Manager()
        {
            AccessRights = new AccessRights(false, false, false, false, false);
        }
    }
}
