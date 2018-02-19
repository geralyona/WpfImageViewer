using System;
using System.Windows.Media;
using System.Windows.Data;

namespace WpfImagesViewer.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            double angle = 0;
            if (value != null)
                angle = double.Parse(value.ToString(),
                    System.Globalization.NumberStyles.Float);

            double dpar = 0;
            if (parameter != null)
                dpar = double.Parse(parameter.ToString(),
                    System.Globalization.NumberStyles.Float);

            angle = 360 - angle;

            angle += 360 * dpar / 10f;
            angle = angle % 360;
            angle = angle / 360;

            byte r0 = 92;
            byte g0 = 150;
            byte b0 = 201;

            byte rr = (byte)(r0 * angle + 255 * (1 - angle));
            byte gg = (byte)(g0 * angle + 255 * (1 - angle));
            byte bb = (byte)(b0 * angle + 255 * (1 - angle));

            return Color.FromRgb(rr, gg, bb).ToString();
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