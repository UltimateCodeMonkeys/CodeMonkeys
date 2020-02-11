using System;
using System.Windows.Input;

namespace CodeMonkeys.MVVM.Commands
{
    public class Command<TParameter> :
        ICommand
    {
        private readonly Action<TParameter> _execute;
        private readonly Predicate<TParameter> _canExecute;

        public event EventHandler CanExecuteChanged;


        public Command(
            Action<TParameter> execute)
        {
            if (execute == null)
                throw new ArgumentNullException(
                    nameof(execute));

            _execute = execute;
        }

        public Command(
            Action execute)
          : this(parameter => execute())
        {
            if (execute == null)
                throw new ArgumentNullException(
                    nameof(execute));
        }

        public Command(
            Action<TParameter> execute,
            Predicate<TParameter> canExecute)
          : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException(
                    nameof(canExecute));

            _canExecute = canExecute;
        }

        public Command(
            Action<TParameter> execute,
            Func<bool> canExecuteFunc)
            : this(execute)
        {
            if (canExecuteFunc == null)
                throw new ArgumentNullException(
                    nameof(canExecuteFunc));

            _canExecute = parameter => canExecuteFunc();
        }

        public Command(
            Action execute,
            Func<bool> canExecuteFunc)
          : this(
                parameter => execute(),
                parameter => canExecuteFunc())
        {
            if (execute == null)
                throw new ArgumentNullException(
                    nameof(execute));

            if (canExecuteFunc == null)
                throw new ArgumentNullException(
                    nameof(canExecuteFunc));
        }

        public void Execute(
            object parameter)
        {
            _execute(
                (TParameter)parameter);
        }

        public bool CanExecute(
            object parameter)
        {
            if (_canExecute != null)
                return _canExecute(
                    (TParameter)parameter);

            return true;
        }


        public void UpdateCanExecute()
        {
            var threadSafeCall = CanExecuteChanged;

            threadSafeCall?.Invoke(
                this,
                EventArgs.Empty);
        }
    }
}