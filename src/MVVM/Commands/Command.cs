using System;
using System.Windows.Input;

namespace CodeMonkeys.MVVM.Commands
{
    public class Command :
        ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;


        public Command(
            Action<object> execute)
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
            Action<object> execute,
            Predicate<object> canExecute)
          : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException(
                    nameof(canExecute));

            _canExecute = canExecute;
        }

        public Command(
            Action execute,
            Func<bool> canExecute)
          : this(
                parameter => execute(),
                parameter => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(
                    nameof(execute));

            if (canExecute == null)
                throw new ArgumentNullException(
                    nameof(canExecute));
        }

        public void Execute(
            object parameter)
        {
            _execute(
                parameter);
        }

        public bool CanExecute(
            object parameter)
        {
            if (_canExecute != null)
                return _canExecute(
                    parameter);

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