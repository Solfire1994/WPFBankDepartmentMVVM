namespace WPFBankDepartmentMVVM.Models.EmployeeBase
{
    internal class AccessRights
    {
        public bool firstNameChanged { get; }
        public bool lastNameChanged { get; }
        public bool middleNameChanged { get; }
        public bool phoneNumberChanged { get; }
        public bool passpoortNumberChanged { get; }

        public AccessRights(bool firstNameChanged, bool lastNameChanged,
            bool middleNameChanged, bool phoneNumberChanged, bool passpoortNumberChanged)
        {
            this.firstNameChanged = firstNameChanged;
            this.lastNameChanged = lastNameChanged;
            this.middleNameChanged = middleNameChanged;
            this.phoneNumberChanged = phoneNumberChanged;
            this.passpoortNumberChanged = passpoortNumberChanged;
        }
    }
}
