using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Pithline.FMS.DocumentDelivery.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var format = (string)parameter;
            if (value == null) return string.Empty;
            var dt = (DateTime)value;

            return string.Format(((format == "d") ? "{0:dd/MM/yyyy}" : "{0:HH:mm}"), dt);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
