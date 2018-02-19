using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Ninject;
using WpfImagesViewer.Annotations;
using WpfImagesViewer.Common;
using WpfImagesViewer.Interfaces;
using WpfImagesViewer.Models;

namespace WpfImagesViewer.ViewModels
{
    public class ImageCollectionViewModel : ObservableCollection<ImageViewModel>
    {
        private const int ImageLoadingBuffer = 5;

        [Inject]
        public IViewModelFactory ViewModelFactory { private get; set; }

        [Inject]
        public IUIDispatcher UIDispatcher { private get; set; }

        public RelayCommand NextImageCommand { get; }

        public RelayCommand PrevImageCommand { get; }

        private readonly List<ImageModel> _collection;

        private ImageViewModel _selectItem;
        public ImageViewModel SelectItem
        {
            get { return _selectItem; }
            private set
            {
                _selectItem = value;
                OnPropertyChanged();
            }
        }

        private int _selectIndex = -1;
        private int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                if (_selectIndex == value)
                    return;

                _selectIndex = value;
                SelectItem = _selectIndex == -1 ? null : this[_selectIndex];
            }
        }

        public ImageCollectionViewModel()
        {
            _collection = new List<ImageModel>();
            NextImageCommand = new RelayCommand(OnNextImageCommand, CanNextImageCommand);
            PrevImageCommand = new RelayCommand(OnPrevImageCommand, CanPrevImageCommand);
        }

        private bool CanPrevImageCommand()
        {
            return SelectIndex >= 1 && SelectIndex <= Count - 1;
        }

        private void OnPrevImageCommand()
        {
            if (!CanPrevImageCommand()) throw new InvalidOperationException();
            UnloadImage(SelectIndex + ImageLoadingBuffer);
            SelectIndex = SelectIndex - 1;
            LoadImage(SelectIndex - ImageLoadingBuffer);
        }

        private bool CanNextImageCommand()
        {
            return SelectIndex >= 0 && SelectIndex <= Count - 2;
        }

        private void OnNextImageCommand()
        {
            if (!CanNextImageCommand()) throw new InvalidOperationException();
            UnloadImage(SelectIndex - ImageLoadingBuffer);
            SelectIndex = SelectIndex + 1;
            LoadImage(SelectIndex + ImageLoadingBuffer);
        }

        private bool Contains(string filePath)
        {
            return _collection.Exists(m => string.Equals(m.FilePath, filePath));
        }

        public async void OnSelectImage(ImageViewModel image)
        {
            var selectItem = SelectItem;
            if (selectItem != null && selectItem != image)
            {
                UnloadImage(SelectIndex);
                for (int i = 1; i < ImageLoadingBuffer; i++)
                {
                    UnloadImage(SelectIndex - 1);
                    UnloadImage(SelectIndex + 1);
                }
            }

            if (image == null)
            {
                SelectIndex = -1;
                return;
            }

            var selectedIndex = IndexOf(image);
            if(selectedIndex < 0) throw new ArgumentOutOfRangeException();

            SelectIndex = selectedIndex;

            await LoadImage(selectedIndex);
            CommandManager.InvalidateRequerySuggested();
            for (int i = 1; i <= ImageLoadingBuffer; i++)
            {
                LoadImage(selectedIndex + i);
                LoadImage(selectedIndex - i);
            }
        }

        private async Task LoadImage(int index)
        {
            if (index >= 0 && index < Count)
                await this[index].LoadImage().ConfigureAwait(false);
        }

        private void UnloadImage(int index)
        {
            if (index >= 0 && index < Count)
                this[index].UnloadImage();
        }

        public async Task AddImages(IEnumerable<string> filePathes)
        {
            var newVMs = new List<ImageViewModel>();
            foreach (var filePath in filePathes)
            {
                if (Contains(filePath))
                    continue;

                var image = new ImageModel(filePath);
                _collection.Add(image);
                var vm = ViewModelFactory.CreateImageViewModel(image);
                
                Add(vm);
                newVMs.Add(vm);
            }

            foreach (var imageViewModel in newVMs)
            {
                await imageViewModel.LoadPreviewImage().ConfigureAwait(true);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            UIDispatcher.Invoke(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)));
        }
    }
}