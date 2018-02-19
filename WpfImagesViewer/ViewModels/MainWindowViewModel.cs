using System.Collections;
using System.Linq;
using Ninject;
using WpfImagesViewer.Common;
using WpfImagesViewer.Interfaces;

namespace WpfImagesViewer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IDropReceiver
    {
        private MainViewMode _viewMode = MainViewMode.Many;
        public MainViewMode ViewMode
        {
            get { return _viewMode; }
            private set
            {
                _viewMode = value;
                OnPropertyChanged();
            }
        }

        private readonly ImageCollectionViewModel _items;
        public IEnumerable Items
        {
            get { return _items; }
        }

        public RelayCommand OpenImagesCommand { get; }
        public RelayCommand ReturnToManyImagesCommand { get; }

        [Inject]
        public IFileDialogService FileDialogService { private get; set; }

        public OpenSingleViewCommand OpenSingleCommand { get; set; }

        public MainWindowViewModel(OpenSingleViewCommand openSingleViewCommand, IViewModelFactory viewModelFactory)
        {
            _items = viewModelFactory.CreateImageCollectionViewModel();

            OpenImagesCommand = new RelayCommand(OnOpenImages, CanOpenImages);
            ReturnToManyImagesCommand = new RelayCommand(OnReturnToManyImages, CanReturnToManyImages);

            OpenSingleCommand = openSingleViewCommand;
            OpenSingleCommand.Define(OnOpenSingleImage, CanOpenSingleImage);
        }

        private void OnReturnToManyImages(object o)
        {
            ViewMode = MainViewMode.Many;
        }

        private bool CanReturnToManyImages()
        {
            return ViewMode == MainViewMode.Single;
        }

        private bool CanOpenSingleImage()
        {
            return ViewMode == MainViewMode.Many;
        }

        private void OnOpenSingleImage(object model)
        {
            ViewMode = MainViewMode.Single;
            _items.OnSelectImage(model as ImageViewModel);
        }

        private bool CanOpenImages()
        {
            return ViewMode == MainViewMode.Many;
        }

        private async void OnOpenImages()
        {
            var x = FileDialogService.GetNewImageFileNames();
            await _items.AddImages(x);
        }

        public async void OnDrop(string[] data){

            if (CanDrop(data))
                await _items.AddImages(data);
        }

        public bool CanDrop(string[] data)
        {
            return data.All(FileDialogService.IsImageFileSupported);
        }
    }
}