using Engine.Models;
using Engine.Services;
using Engine.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WPFUI;

namespace WPFUI.CustomConverters
{
    public class ColoringFromBoolean : IMultiValueConverter
    {
        //  private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 ||
                !int.TryParse(values[0]?.ToString(), out int currentHitPoints) ||
                !int.TryParse(values[1]?.ToString(), out int maximumHitPoints))
            {
                return new SolidColorBrush(Colors.Black); // Default Farbe
            }

        bool LowHealth = (int)(maximumHitPoints / 2) >= currentHitPoints && (int)(maximumHitPoints / 4) < currentHitPoints;
        
        bool CriticalHealth = (int)(maximumHitPoints / 4) >= currentHitPoints;
        
            if (LowHealth) 
            {
               return new SolidColorBrush(Color.FromRgb(139, 128, 0));
                
            }
            else if (CriticalHealth)
            {
                return new SolidColorBrush(Color.FromRgb(135, 0, 0));   
                
            }

             return new SolidColorBrush(Colors.Black);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
