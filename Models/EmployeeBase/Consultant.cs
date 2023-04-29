namespace WPFBankDepartmentMVVM.Models.EmployeeBase
{
    internal class Consultant : Employee
    {    
        public Consultant()
        {
            AccessRights = new AccessRights(true, true, true, false, true);
        }
    }
}
