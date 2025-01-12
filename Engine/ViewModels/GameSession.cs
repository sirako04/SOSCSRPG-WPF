﻿using Engine.Models;
using System;
using System.Linq;
using Engine.Factories;
using Engine.EventArgs;
using Engine.Services;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Engine.ViewModels
{
    public class GameSession : Notification
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private const string BattleTheme = @"D:\C# WPF RPG\SOSCSRPG\Engine\Music\TownSlowed.wav";
        private const string BGM_FILE = @"D:\C# WPF RPG\SOSCSRPG\Engine\Music\Muladhara.wav";

        #region Properties
        private GameDetails _gameDetails;

        private Battle _currentBattle;
        private Trader _currentTrader;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Player _currentPlayer;

        [JsonIgnore]
        public GameDetails GameDetails
        {
            get { return _gameDetails; }
            set
            {
                _gameDetails = value;
                OnPropertyChanged();
            }
        }  
        [JsonIgnore]
        public World CurrentWorld { get; }
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled -= OnPlayerKilled;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnPlayerKilled;
                }

            }
        }
        [JsonIgnore]
        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                if (_currentBattle != null)
                {
                    _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                    _currentBattle.Dispose();
                    _currentBattle = null;
                }
                _currentMonster = value;

                if (CurrentMonster != null)
                {
                    _currentBattle = new Battle(CurrentPlayer, CurrentMonster);
                    _currentBattle.OnCombatVictory += OnCurrentMonsterKilled;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }
        [JsonIgnore]
        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }
            
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(HasLocationToWest));

                CompleteQuestsAtLocation();
                GivePlayerQuestsAtLocation();
                CurrentMonster = CurrentLocation.GetMonster();
                CurrentTrader = CurrentLocation.TraderHere;

                if(CurrentLocation == CurrentWorld.LocationAt(0, -1))
                {
                    Sound.Stop(BGM_FILE);
                    Sound.Playing(BGM_FILE);
                }
                else
                {
                    Sound.Stop(BGM_FILE);
                }



            }
        }
        
        [JsonIgnore]
        public bool HasLocationToNorth => CurrentWorld.LocationAt
            (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        
        [JsonIgnore]
        public bool HasLocationToEast => CurrentWorld.LocationAt
            (CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;

        [JsonIgnore]
        public bool HasLocationToSouth => CurrentWorld.LocationAt
            (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        
        [JsonIgnore]
        public bool HasLocationToWest => CurrentWorld.LocationAt
            (CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        [JsonIgnore]
        public bool HasMonster => CurrentMonster != null;
        [JsonIgnore]
        public bool HasTrader => CurrentTrader != null;
        #endregion
        public GameSession(Player player, int xCoordinate, int yCoordinate)
        {
            PopulateGameDetails();

            CurrentWorld = WorldFactory.CreateWorld();
            CurrentPlayer = player;
            CurrentLocation = CurrentWorld.LocationAt(xCoordinate, yCoordinate);
        }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt
                (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt
                (CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt
                (CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt
                (CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }
        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete
                = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.Inventory.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        // remove the quest completion items from the player inventory
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);
                        _messageBroker.RaiseMessage("");
                        _messageBroker.RaiseMessage($" You completed the {quest.Name} quest.");

                        _messageBroker.RaiseMessage($" You receive {quest.RewardEXP} EXP.");
                        CurrentPlayer.AddExperience(quest.RewardEXP);

                        _messageBroker.RaiseMessage($" You receive {quest.RewardGold} gold.");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewarditem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                            _messageBroker.RaiseMessage($" You receive a {rewarditem.Name}");
                            CurrentPlayer.AddItemToInventory(rewarditem);
                        }
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }
        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                    _messageBroker.RaiseMessage("");
                    _messageBroker.RaiseMessage($"You receive the '{quest.Name}' quest");
                    _messageBroker.RaiseMessage(quest.Description);
                    _messageBroker.RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        _messageBroker.RaiseMessage($"{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    _messageBroker.RaiseMessage("And you will receive:");
                    _messageBroker.RaiseMessage($"  {quest.RewardEXP} experience points");
                    _messageBroker.RaiseMessage($"  {quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        _messageBroker.RaiseMessage($"{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                }
            }
        }
       
        public void AttackCurrentMonster()
        {
            Sound.Playing(BattleTheme);
            _currentBattle?.AttackOpponent();
        }
        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable != null)
            {
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed += OnConsumableActionPerformed;
                }
                CurrentPlayer.UseCurrentConsumable();
                if (_currentBattle == null)
                {
                    CurrentPlayer.OnActionPerformed -= OnConsumableActionPerformed;
                }
            }
        }
        public void CraftItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);
                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        _messageBroker.RaiseMessage($"You crafted 1 {outputItem.Name}");
                    }
                }
            }
            else
            {
                _messageBroker.RaiseMessage("You don`t have the required ingredients:");
                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    _messageBroker.RaiseMessage($"{itemQuantity.Quantity}" +
                        $"{ItemFactory.ItemName(itemQuantity.ItemID)}");
                }
            }
        }
        private void PopulateGameDetails()
        {
            GameDetailsService.ReadGameDetails();
        }
        private void OnPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            Sound.Stop(BattleTheme);
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage(" You have been killed. ");
            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }
        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            Sound.Stop(BattleTheme);
            Thread.Sleep(100);
            CurrentMonster = CurrentLocation.GetMonster();
        }
        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage(" YOU LEVELED UP!! ");
            _messageBroker.RaiseMessage("");
            _messageBroker.RaiseMessage($"You are now Level {CurrentPlayer.Level}!");
            _messageBroker.RaiseMessage("");
        }
        private void OnConsumableActionPerformed(object sender, string result)
        {
            _messageBroker.RaiseMessage(result);
        }
        public void PlayingMusic(string filepath)
        {
            Sound.Playing(filepath);
        }
    }
}
