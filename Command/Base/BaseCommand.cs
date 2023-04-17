using System;
using System.Windows.Input;

namespace WPFBankDepartmentMVVM.Command.Base
{
    internal class BaseCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public BaseCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            canExecute = CanExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => execute(parameter);
    }
}
