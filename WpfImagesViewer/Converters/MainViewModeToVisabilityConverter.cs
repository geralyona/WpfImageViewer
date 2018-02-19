using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WpfImagesViewer.Common;

namespace WpfImagesViewer.Converters
{
    public class MainVisualModeToVisibilityConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MainViewMode local && parameter is MainViewMode mode)
            {
                return local == mode ? Visibility.Visible : Visibility.Hidden;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility local && parameter is MainViewMode mode)
            {
                if (local == Visibility.Visible)
                    return mode;

                return mode == MainViewMode.Many ? MainViewMode.Single : MainViewMode.Many;
            }

            return MainViewMode.None;
        }
    }
}