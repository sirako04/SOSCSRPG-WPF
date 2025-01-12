﻿using Engine;
using Engine.Models;
using Engine.ViewModels;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using WPFUI.Windows;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for TradeScreen.xaml
    /// </summary>
    public partial class TradeScreen : Window
    {
        private const string BGM_FILE = @"D:\C# WPF RPG\SOSCSRPG\Engine\Music\ShopMusic.wav";
        public GameSession Session => DataContext as GameSession;
        public TradeScreen()
        {
            InitializeComponent();

        }

        private void OnClick_Sell(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;
            
            if (groupedInventoryItem.Item.IsUnique)
            {
                YesOrNoWindow window = new YesOrNoWindow($"Selling {groupedInventoryItem.Item.Name}? ", $"Do you really wanna sell your {groupedInventoryItem.Item.Name}?")
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ShowInTaskbar = false           
                };

                window.ShowDialog();
                if (!window.ClickedYes)
                {
                    return;
                }
            }
            if (groupedInventoryItem != null)
            {
                Session.CurrentPlayer.ReceiveGold(groupedInventoryItem.Item.Price);
                Session.CurrentTrader.AddItemToInventory(groupedInventoryItem.Item);
                Session.CurrentPlayer.RemoveItemFromInventory(groupedInventoryItem.Item);
            }
            return;
        }

        private void OnClick_Buy(object sender, RoutedEventArgs e)
        {
            GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

            if (groupedInventoryItem != null)
            {
               
                if (Session.CurrentPlayer.Gold >= groupedInventoryItem.Item.Price)
                {
                    Session.CurrentPlayer.SpendGold(groupedInventoryItem.Item.Price);
                    Session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.Item);
                    Session.CurrentPlayer.AddItemToInventory(groupedInventoryItem.Item);
                }
                else
                {
                    MessageBox.Show("You dont have enough gold");
                }
            }
            return;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
            Sound.Stop(BGM_FILE);
        }   
    }
}
