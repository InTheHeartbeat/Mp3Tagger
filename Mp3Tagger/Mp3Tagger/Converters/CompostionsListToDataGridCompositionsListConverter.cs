using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Mp3Tagger.Kernel.Models;
using Mp3Tagger.Models;

namespace Mp3Tagger.Converters
{
    public class CompostionsListToDataGridCompositionsListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return new List<DataGridComposition>();
            return ((ObservableCollection<Composition>) value).Select(c => new DataGridComposition(c));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
