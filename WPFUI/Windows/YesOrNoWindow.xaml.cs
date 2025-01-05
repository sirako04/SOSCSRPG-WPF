using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPFUI.Windows
{
    /// <summary>
    /// Interaction logic for YesOrNoWindow.xaml
    /// </summary>
    public partial class YesOrNoWindow : Window
    {
        public bool ClickedYes { get; private set; }
        public YesOrNoWindow(string title, string message)
        {
            InitializeComponent();
            Title = title;
            Message.Content = message;

            var fadeInStoryboard = (Storyboard)this.Resources["WindowFadeInStoryboard"];
            fadeInStoryboard.Begin(this);
        }
        private void Yes_OnClick(object sender, RoutedEventArgs e)
        {
            ClickedYes = true;
            Close();
        }
        private void No_OnClick(object sender, RoutedEventArgs e)
        {
            ClickedYes = false;
            Close();
        }
    }
}
