using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Mp3Tagger.Converters
{
    public class EmptyCellToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || ((ContentPresenter)value).Content == null || String.IsNullOrWhiteSpace(((TextBlock) ((ContentPresenter) value)?.Content)?.Text))
            {
                return Brushes.BlueViolet;
            }

            return Application.Current.Resources["DataGridBackground"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
