using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeMonkeys.MVVM.Commands
{
    public class AsyncCommand<TParameter> :
        ICommand
    {
        private readonly Func<TParameter, Task> _executeFunc;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;


        public AsyncCommand(
            Func<TParameter, Task> executeFunc)
        {
            if (executeFunc == null)
                throw new ArgumentNullException(
                    nameof(executeFunc));

            _executeFunc = executeFunc;
        }

        public AsyncCommand(
            Func<Task> executeFunc)
          : this(parameter => executeFunc())
        {
            if (executeFunc == null)
                throw new ArgumentNullException(
                    nameof(executeFunc));
        }

        public AsyncCommand(
            Func<TParameter, Task> executeFunc,
            Predicate<object> canExecute)
          : this(executeFunc)
        {
            if (canExecute == null)
                throw new ArgumentNullException(
                    nameof(canExecute));

            _canExecute = canExecute;
        }

        public AsyncCommand(
            Func<Task> executeFunc,
            Func<bool> canExecuteFunc)
          : this(
                aiObject => executeFunc(),
                aiObject => canExecuteFunc())
        {
            if (executeFunc == null)
                throw new ArgumentNullException(
                    nameof(executeFunc));

            if (canExecuteFunc == null)
                throw new ArgumentNullException(
                    nameof(canExecuteFunc));
        }

        public async Task Execute(
            TParameter parameter)
        {
            if (!CanExecute(
                parameter))
                return;

            await _executeFunc(
                parameter);
        }

        async void ICommand.Execute(
            object parameter)
        {
            await Execute(
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