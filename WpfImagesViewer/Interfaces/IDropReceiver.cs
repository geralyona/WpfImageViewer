namespace WpfImagesViewer.Interfaces
{
    public interface IDropReceiver
    {
        void OnDrop(string[] data);

        bool CanDrop(string[] data);
    }
}