using System;
using System.Windows.Input;
using WpfImagesViewer.Annotations;

namespace WpfImagesViewer.Common
{
    public class RelayCommand : ICommand
    {
        protected Action<object> _execute;
        protected Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected RelayCommand()
        {
            _execute = null;
            _canExecute = null;
        }

        protected RelayCommand(Func<bool> canExecute) : this()
        {
            _canExecute = canExecute;
        }

        public RelayCommand([NotNull] Action<object> execute, Func<bool> canExecute = null) 
            : this(canExecute)
        {
            _execute = execute ??
                       throw new ArgumentNullException(nameof(execute));
        }

        public RelayCommand([NotNull] Action execute, Func<bool> canExecute = null)
            : this(canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));
            _execute = o => execute();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            if (_execute == null)
                throw new InvalidOperationException();

            _execute(parameter);
        }
    }
    
}