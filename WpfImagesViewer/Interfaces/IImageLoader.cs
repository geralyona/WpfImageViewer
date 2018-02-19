using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfImagesViewer.Interfaces
{
    public interface IImageLoader
    {
        Task<BitmapImage> LoadImage(string uri, int? pixelHeight = null, int? pixelWidth = null);
    }
}