using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPFUI.CustomConverters
{
    public class BooleanToText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string convertedValue = value.ToString();

            if (convertedValue =="False")
            {
                convertedValue = "Nop";
            }      
            else
            {
                convertedValue = "Yes";
            }

            return convertedValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
