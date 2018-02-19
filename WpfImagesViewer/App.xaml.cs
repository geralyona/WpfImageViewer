using System.Windows;
using Ninject;
using Ninject.Extensions.Factory;
using WpfImagesViewer.Common;
using WpfImagesViewer.Interfaces;
using WpfImagesViewer.Services;
using WpfImagesViewer.ViewModels;

namespace WpfImagesViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _container = new StandardKernel();
            _container.Bind<IViewModelFactory>().ToFactory();
            _container.Bind<IFileDialogService>().To<FileDialogService>().InSingletonScope();
            _container.Bind<IImageLoader>().To<ImageLoader>().InSingletonScope();
            _container.Bind<OpenSingleViewCommand>().ToSelf().InSingletonScope();

            var uiDispatcher = new UIDispatcher();
            _container.Bind<IUIDispatcher>().ToConstant(uiDispatcher);

            var window = _container.Get<MainWindow>();
            uiDispatcher.Init(window.Dispatcher);
            window.DataContext = _container.Get<MainWindowViewModel>();
            Current.MainWindow = window;

            Current.MainWindow.Show();
        }
    }
}
