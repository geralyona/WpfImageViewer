using System;

namespace WpfImagesViewer.Interfaces
{
    public interface IUIDispatcher
    {
        void Invoke(Action action);

        void VerifyAccess();
    }
}