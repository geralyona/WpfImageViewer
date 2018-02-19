using System;
using WpfImagesViewer.Annotations;

namespace WpfImagesViewer.Common
{
    /// <summary>
    /// Command for open view in single mode
    /// </summary>
    public class OpenSingleViewCommand : ExternalRelayCommand
    { }

    /// <summary>
    /// Relay command with option to define Execute and CanExecute
    /// </summary>
    public class ExternalRelayCommand : RelayCommand
    {
        private bool _isDefined;

        public bool Define([NotNull] Action<object> execute, Func<bool> canExecute = null)
        {
            if (_isDefined) return false;

            _isDefined = true;

            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;

            return true;
        }
    }
}