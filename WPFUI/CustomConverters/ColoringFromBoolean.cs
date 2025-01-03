using Engine.Models;
using Engine.Services;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFUI.CustomConverters
{
    public class ColoringFromBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = value.ToString(), // Text anzeigen
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            };
            GameSession gameSession = SaveGameService.LoadLastSaveOrCreateNew();

            Player player = new Player("", "", 0, gameSession.CurrentPlayer.MaximumHitPoints, (int)value, 0, 0);

            if (player.LowHealth) 
            {
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(139, 128, 0));
            }
            else if (player.CriticalHealth)
            {
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(135, 0, 0));                    
            }
            return textBlock;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return null;
        }
    }
}
