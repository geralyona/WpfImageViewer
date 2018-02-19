using System;
using System.Windows;
using WpfImagesViewer.Interfaces;

namespace WpfImagesViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (!(DataContext is IDropReceiver receiver))
                throw new NotSupportedException();

            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            receiver.OnDrop(files);
        }

        private void UIElement_OnDragOver(object sender, DragEventArgs e)
        {
            if (DataContext is IDropReceiver receiver)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (receiver.CanDrop(files))
                    {
                        e.Effects = DragDropEffects.Copy;
                        return;
                    }
                }
            }

            e.Handled = true;
            e.Effects = DragDropEffects.None;
        }
    }
}
