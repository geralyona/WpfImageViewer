using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfImagesViewer.Interfaces;

namespace WpfImagesViewer.Services
{
    class ImageLoader : IImageLoader
    {
        private readonly object _sync = new object();

        private volatile Task<BitmapImage> _loadingTask;

        public Task<BitmapImage> LoadImage(string uri, int? pixelHeight = null, int? pixelWidth = null)
        {
            lock (_sync)
            {
                if (_loadingTask != null)
                    return (_loadingTask.ContinueWith(p => LoadImagePreviewCore(uri, pixelHeight, pixelWidth)));

                return _loadingTask = Task.Run(() => LoadImagePreviewCore(uri, pixelHeight, pixelWidth));
            }
        }
        
        private BitmapImage LoadImagePreviewCore(string uri, int? pixelHeight, int? pixelWidth)
        {
            var image = new BitmapImage();

            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(uri);

            if (pixelHeight != null)
                image.DecodePixelHeight = pixelHeight.Value;

            if (pixelWidth != null)
                image.DecodePixelWidth = pixelWidth.Value;

            image.EndInit();

            image.Freeze();
            return image;
        }
    }
}