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
