using System;
using System.Windows.Threading;
using WpfImagesViewer.Annotations;
using WpfImagesViewer.Interfaces;

namespace WpfImagesViewer.Services
{
    public class UIDispatcher : IUIDispatcher
    {
        private Dispatcher _dispatcher;

        public void Init([NotNull] Dispatcher dispatcher)
        {
            if (dispatcher == null) throw new ArgumentNullException(nameof(dispatcher));

            if (_dispatcher == null)
            {
                _dispatcher = Dispatcher.CurrentDispatcher;
            }
            else
            {
                if (_dispatcher != Dispatcher.CurrentDispatcher)
                    throw new InvalidOperationException();
            }
        }

        public void Invoke(Action action)
        {
            if (_dispatcher.CheckAccess())
            {
                action();
                return;
            }

            _dispatcher.Invoke(action);
        }

        public void VerifyAccess()
        {
            _dispatcher.VerifyAccess();
        }
    }
}