using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Mp3Tagger.Models;

namespace Mp3Tagger.Converters
{
    public class BitrateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridComposition input = value as DataGridComposition;
            
            if (input.Bitrate >= 320)
            {
                return Brushes.ForestGreen;
            }
            if (input.Bitrate < 320 && input.Bitrate >= 256)
            {
                return Brushes.DodgerBlue;
            }
            if(input.Bitrate < 265 && input.Bitrate >= 128)
            {
                return Brushes.DarkOrange;
            }
            if (input.Bitrate < 128 && input.Bitrate >= 1)
            {
                return Brushes.Firebrick;
            }
            return Brushes.BlueViolet;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
