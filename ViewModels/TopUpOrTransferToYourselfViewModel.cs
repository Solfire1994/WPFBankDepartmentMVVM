using System;
using System.Windows;
using System.Windows.Input;
using WPFBankDepartmentMVVM.Command.Base;
using WPFBankDepartmentMVVM.Services;
using WPFBankDepartmentMVVM.ViewModels.Base;

namespace WPFBankDepartmentMVVM.ViewModels
{
    internal class TopUpOrTransferToYourselfViewModel : DialogViewModel, IDisposable
    {
        private readonly IMessageBus _MessageBus = null!;
        private readonly IDisposable _SubscriptionValue = null!;
        private bool IsValueCorrect = false;
        
        #region Заголовок
        private string title;

        public string Title
        {
            get { return title; }
            set => Set(ref title, value, nameof(Title));
        }
        #endregion

        #region Данные текст блока
        private string statusBar;

        public string StatusBar
        {
            get { return statusBar; }
            set => Set(ref statusBar, value, nameof(StatusBar));
        }
        #endregion

        #region Контент кнопки
        private string buttonBar;

        public string ButtonBar
        {
            get { return buttonBar; }
            set => Set(ref buttonBar, value, nameof(ButtonBar));
        }
        #endregion

        #region Доступная сумма
        private uint maxValue;

        public uint MaxValue
        {
            get { return maxValue; }
            set => Set(ref maxValue, value, nameof(MaxValue));
        }
        #endregion

        #region Сумма
        private string _value;

        public string Value
        {
            get { return _value; }
            set 
            {                
                Set(ref _value, value, nameof(Value));
                CheckValue();
            }
        }
        #endregion

        #region Обозначение проверки корректности ввода
        private Thickness correctWeight;

        public Thickness CorrectWeight
        {
            get { return correctWeight; }
            set => Set(ref correctWeight, value, nameof(CorrectWeight));
        }
        #endregion

        #region Перечислить
        public ICommand Command { get; }
        private bool CanCommandExecute(object p) => IsValueCorrect;
        

        private void OnCommandExecuted(object p)
        {
            _MessageBus.Send(uint.Parse(Value));
            OnDialogComplete(EventArgs.Empty);
        }
        #endregion

        #region Метод для передачи между окнами суммы на счете        
        private void OnReceiveValue(uint message)
        {
            Title = "Окно перевода на свой счет";
            MaxValue = message;
            StatusBar = $"Для перевода введите сумму не более {message} рублей";
            ButtonBar = "Перевести";
            if (message == 0)
            {
                Title = "Окно пополнения счета";
                MaxValue = uint.MaxValue;
                StatusBar = "Для пополнения счета введите сумму цифрами:";
                ButtonBar = "Пополнить";
            }
            
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _SubscriptionValue.Dispose();
        }
        #endregion

        #region Проверка корректности ввода
        public void CheckValue()
        {
            if (uint.TryParse(_value, out uint res) && res <= maxValue)
            {
                IsValueCorrect = true;
                CorrectWeight = new(0);
            }
            else
            {
                IsValueCorrect = false;
                CorrectWeight = new(1.5);
            }

        }
        #endregion

        public TopUpOrTransferToYourselfViewModel()
        {
            Command = new BaseCommand(OnCommandExecuted, CanCommandExecute);            
        }

        public TopUpOrTransferToYourselfViewModel(IMessageBus messageBus) : this()
        {
            _MessageBus = messageBus;
            _SubscriptionValue = messageBus.RegesterHandler<uint>(OnReceiveValue);
        }
    }
}
