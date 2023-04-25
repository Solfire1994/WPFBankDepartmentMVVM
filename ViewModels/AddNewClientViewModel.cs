using System;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Models.ClientBase;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class AddNewClientViewModel : DialogViewModel
    {
        #region Фамилия
        private string lastName;
        private bool lastNameIsCorrect = false;
        public string LastName
        {
            get { return lastName; }
            set 
            {
                Set(ref lastName, value, nameof(LastName));
                CheckField(lastName, ref lastNameIsCorrect);                
            }
        }
        #endregion

        #region Имя
        private string firstName;
        private bool firstNameIsCorrect = false;
        public string FirstName
        {
            get { return firstName; }
            set 
            {
                Set(ref firstName, value, nameof(FirstName));
                CheckField(firstName, ref firstNameIsCorrect);
            }
        }
        #endregion

        #region Отчество
        private string middleName;
        private bool middleNameIsCorrect = false;
        public string MiddleName
        {
            get { return middleName; }
            set 
            {
                Set(ref middleName, value, nameof(MiddleName));
                CheckField(middleName, ref middleNameIsCorrect);
            }
        }
        #endregion

        #region Номер телефона
        private string phoneNumber;
        private bool phoneNumberIsCorrect = false;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            { 
                Set(ref phoneNumber, value, nameof(PhoneNumber));
                CheckField(phoneNumber, ref phoneNumberIsCorrect);
            }
        }
        #endregion

        #region Номер и серия пасспорта
        private string passportNumber;
        private bool passportNumberIsCorrect = false;
        public string PassportNumber
        {
            get { return passportNumber; }
            set
            {
                Set(ref passportNumber, value, nameof(PassportNumber));
                CheckField(passportNumber, ref passportNumberIsCorrect);
            }
        }
        #endregion

        private readonly IMessageBus _MessageBus = null!;
        public Client client = null!;

        #region Создание нового клиента
        public ICommand AddNewClientCommand { get; }
        private bool CanAddNewClientCommandExecute(object p) => 
            lastNameIsCorrect && firstNameIsCorrect && middleNameIsCorrect && phoneNumberIsCorrect && passportNumberIsCorrect;

        private void OnAddNewClientCommandExecuted(object p)
        {
            client = new(0, lastName, firstName, middleName, phoneNumber, passportNumber);
            _MessageBus.Send(client);
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        #region Закрытие окна
        public ICommand CloseCommand { get; }
        private bool CanCloseCommandExecute(object p) => true;

        private void OnCloseCommandExecuted(object p)
        {
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        private void CheckField(string field, ref bool fieldIsCorrect)
        {
            if (field.Length > 0 && field.Length <= 40)
            {
                fieldIsCorrect = true;
                return;
            }
            fieldIsCorrect = false;
        }

        public AddNewClientViewModel()
        {
            AddNewClientCommand = new BaseCommand(OnAddNewClientCommandExecuted, CanAddNewClientCommandExecute);
            CloseCommand = new BaseCommand(OnCloseCommandExecuted, CanCloseCommandExecute);
        }

        public AddNewClientViewModel(IMessageBus messagebus) : this()
        {
            _MessageBus = messagebus;
        }
    }
}
