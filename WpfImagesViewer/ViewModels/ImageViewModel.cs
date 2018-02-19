using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Ninject;
using WpfImagesViewer.Common;
using WpfImagesViewer.Interfaces;
using WpfImagesViewer.Models;

namespace WpfImagesViewer.ViewModels
{
    public class ImageViewModel : BaseViewModel
    {
        private const string LoadingImagePath = @"pack://application:,,,/Images/loading.png";
        private const string ErrorImagePath = @"pack://application:,,,/Images/error.png";
        private static readonly BitmapImage LoadingImage;
        private static readonly BitmapImage ErrorImage;

        private readonly object _syncImage = new object();
        private readonly object _syncImagePreview = new object();

        private readonly ImageModel _imageModel;
        private bool _isImagePreviewLoadingRequested;
        private bool _isImageLoadingRequested;

        [Inject]
        public IImageLoader ImageLoader { private get; set; }

        [Inject]
        public IUIDispatcher UIDispatcher { private get; set; }

        [Inject]
        public OpenSingleViewCommand OpenSingleCommand { get; set; }

        public RelayCommand BlurCommand { get; }

        private ImageLoadingStatus _loadingStatus = ImageLoadingStatus.None;

        public ImageLoadingStatus LoadingStatus
        {
            get { return _loadingStatus;}
            private set
            {
                _loadingStatus = value;
                UIDispatcher.Invoke(() =>OnPropertyChanged());
            }
        }

        static ImageViewModel()
        {
            LoadingImage = new BitmapImage(new Uri(LoadingImagePath, UriKind.Absolute));
            ErrorImage = new BitmapImage(new Uri(ErrorImagePath, UriKind.Absolute));
        }

        public ImageViewModel(ImageModel data)
        {
            _imageModel = data;
            _loadingStatus = ImageLoadingStatus.None;
            BlurCommand = new RelayCommand(OnBlurCommand, CanBlurCommand);
        }

        private bool CanBlurCommand()
        {
            return LoadingStatus == ImageLoadingStatus.Done;
        }

        private void OnBlurCommand()
        {
            Blur();
        }

        public int PreviewHeight { get; set; } = 100;

        public int PreviewWidth { get; set; } = 150;

        public int Height { get; set; } = 1000;

        public string FileName
        {
            get
            {
                return null != _imageModel ? _imageModel.FilePath : string.Empty;
            }
        }

        public string Name
        {
            get
            {
                return null != _imageModel ? _imageModel.Name : string.Empty;
            }
        }

        private BitmapSource _previewImage;

        public BitmapSource PreviewImage
        {
            get { return _previewImage ?? LoadingImage; }
            set
            {
                _previewImage = value;
                UIDispatcher.Invoke(() => OnPropertyChanged());
            }
        }

        public async Task LoadPreviewImage()
        {
            lock (_syncImagePreview)
            {
                // If image was unloaded while waiting for image loading - we should ignore loading result
                if (_isImagePreviewLoadingRequested)
                    return;

                _isImagePreviewLoadingRequested = true;
            }

            try
            {
                PreviewImage = await ImageLoader.LoadImage(FileName, pixelHeight: PreviewHeight).ConfigureAwait(true);
            }
            catch (Exception)
            {
                PreviewImage = ErrorImage;
            }
        }

        private BitmapSource _image;

        public BitmapSource Image
        {
            get
            {
                return _image ?? LoadingImage;
            }
            set
            {
                lock (_syncImage)
                {
                    if (!_isImageLoadingRequested && value != null)
                        return;
                }

                _image = value;
                UIDispatcher.Invoke(() => OnPropertyChanged());
            }
        }

        public async Task LoadImage()
        {
            lock (_syncImage)
            {
                if (_isImageLoadingRequested)
                    return;

                _isImageLoadingRequested = true;
            }
           
            try
            {
                LoadingStatus = ImageLoadingStatus.Loading;
                Image = await ImageLoader.LoadImage(FileName, pixelHeight: Height).ConfigureAwait(true);
                LoadingStatus = ImageLoadingStatus.Done;
            }
            catch (Exception)
            {
                Image = ErrorImage;
                LoadingStatus = ImageLoadingStatus.Error;
            }
        }

        public void UnloadImage()
        {
            lock (_syncImage)
            {
                if (!_isImageLoadingRequested)
                    return;

                _isImageLoadingRequested = false;
                Image = null;
                LoadingStatus = ImageLoadingStatus.None;
            }
        }

        public void Blur()
        {
            Image = new WriteableBitmap(Image).Convolute(WriteableBitmapExtensions.KernelGaussianBlur5x5);
        }
    }
}