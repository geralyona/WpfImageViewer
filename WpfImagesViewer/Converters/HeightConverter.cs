using System;
using System.Windows.Data;

namespace WpfImagesViewer.Converters
{
    public class HeightConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;
            return (double)value / 8 / 1.2f;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}