﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
                convertedValue = "NO";
            }      
            else
            {
                convertedValue = "YES";
            }

            return convertedValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
