using System.Collections.Generic;

namespace WpfImagesViewer.Interfaces
{
    public interface IFileDialogService
    {
        IEnumerable<string> GetNewImageFileNames();

        bool IsImageFileSupported(string path);
    }
}