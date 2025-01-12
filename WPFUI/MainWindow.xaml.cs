﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Engine.EventArgs;
using Engine.Models;
using Engine.ViewModels;
using Engine.Services;
using System.ComponentModel;
using Microsoft.Win32;
using WPFUI.Windows;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private const string SAVE_GAME_FILE_EXTENSION = "soscsrpg";

        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private const string BGM_FILE = @"D:\C# WPF RPG\SOSCSRPG\Engine\Music\ShopMusic.wav";
       
        private  GameSession _gameSession;

        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();
        public MainWindow(Player player, int xLocation = 0, int yLocation = 0)
        {

            InitializeComponent();

            InitializeUserInputActions();

            SetActiveGameSessionTo(new GameSession(player, xLocation, yLocation));
        }
       
        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }

        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }

        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }

        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }
        private void OnClick_AttackMonster(object sender, RoutedEventArgs e)
        {
            _gameSession.AttackCurrentMonster();
        }
        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();
        }
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd();
        }

        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            if (_gameSession.CurrentTrader != null)
            {

                TradeScreen tradeScreen = new TradeScreen
                {
                    Owner = this,
                    DataContext = _gameSession
                };
                _gameSession.PlayingMusic(BGM_FILE);
                tradeScreen.ShowDialog();
            }
        }
        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }
        private void InitializeUserInputActions()
        {
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.Enter, () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.I, () => SetTabFocusTo("InventoryTabItem"));
            _userInputActions.Add(Key.Q, () => SetTabFocusTo("QuestsTabItem"));
            _userInputActions.Add(Key.R, () => SetTabFocusTo("RecipesTabItem"));
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));
        }
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputActions.ContainsKey(e.Key))
            {
                _userInputActions[e.Key].Invoke();
            }
        }
        private void SetTabFocusTo(string tabName)
        {
            foreach (object item in PlayerDataTabControl.Items)
            {
                if (item is TabItem tabItem) //  it means tabItem = item as TabItem (object)
                {
                    if (tabItem.Name == tabName)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
                }
            }
        }
        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;
            _gameSession = gameSession;
            DataContext = _gameSession;

            GameMessages.Document.Blocks.Clear();

            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }
       

        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            Startup startup = new Startup();
            startup.Show();
            Close();
        }

        private void LoadGame_OnClick(object sender, RoutedEventArgs e)
        {
          /*  OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SetActiveGameSessionTo(SaveGameService.LoadLastSaveOrCreateNew
                    (openFileDialog.FileName));
            }
            return;*/
        }

        private void SaveGame_OnClick(object sender, RoutedEventArgs e)
        {
            SaveGame();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AskToSave();
        }
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                SaveGameService.Save(_gameSession, saveFileDialog.FileName);
            }
        }
        private void AskToSave()
        {
            YesOrNoWindow message = new YesOrNoWindow
                ("Save Game", "Do you wanna save your current game?");
            message.Owner = GetWindow(this);
            message.ShowDialog();

            if (message.ClickedYes)
            {
                SaveGame();
            }
        }
    }
}
