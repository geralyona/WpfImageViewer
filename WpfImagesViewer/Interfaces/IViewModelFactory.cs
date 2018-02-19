using WpfImagesViewer.Models;
using WpfImagesViewer.ViewModels;

namespace WpfImagesViewer.Interfaces
{
    public interface IViewModelFactory
    {
        ImageCollectionViewModel CreateImageCollectionViewModel();

        ImageViewModel CreateImageViewModel(ImageModel data);
    }
}