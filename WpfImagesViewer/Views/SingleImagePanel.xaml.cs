using System.Windows;
using System.Windows.Controls;

namespace WpfImagesViewer.Views
{
    /// <summary>
    /// Interaction logic for SingleImagePanel.xaml
    /// </summary>
    public partial class SingleImagePanel : UserControl
    {
        public SingleImagePanel()
        {
            InitializeComponent();
        }

        private void SingleImagePanel_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as bool?;
            if (newValue == true)
                Focus();
        }
    }
}
