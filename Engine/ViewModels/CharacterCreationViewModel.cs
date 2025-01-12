﻿using Engine.Models;
using Engine.Services;
using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ViewModels
{
    public class CharacterCreationViewModel : Notification
    {
        private Race _selectedRace;
        public GameDetails GameDetails { get; }
        public Race SelectedRace 
        {
            get => _selectedRace; 
            set 
            {
                _selectedRace = value; 
                OnPropertyChanged();
            }

        }
        public string Name {  get; set; }
        public ObservableCollection<PlayerAttribute> PlayerAttributes { get; set; } =
            new ObservableCollection<PlayerAttribute>();
        public bool HasRaces => GameDetails.Races.Any();
        public bool HasRaceAttributeModifiers => HasRaces && 
            GameDetails.Races.Any(r=> r.PlayerAttributeModifiers.Any());

        public CharacterCreationViewModel()
        {
            GameDetails = GameDetailsService.ReadGameDetails();
            if (HasRaces)
            {
                SelectedRace = GameDetails.Races.First();
            }

            RollNewCharacter();
        }
        public void RollNewCharacter()
        {
            PlayerAttributes.Clear();
            foreach (PlayerAttribute playerAttribute in GameDetails.PlayerAttributes)
            {
                playerAttribute.ReRoll();
                PlayerAttributes.Add(playerAttribute);
            }
            ApplyAttributeModifiers();
        }
        public void ApplyAttributeModifiers()
        {
            foreach (PlayerAttribute playerAttribute in PlayerAttributes)
            {
                var attributeRaceModifier = 
                    SelectedRace.PlayerAttributeModifiers
                    .FirstOrDefault(pam => pam. AttributeKey.Equals(playerAttribute.Key));
                playerAttribute.ModifiedValue =
                    playerAttribute.BaseValue + (attributeRaceModifier?.Modifier ?? 0);
            }
        }
        public Player GetPlayer()
        {
           Player player = new Player(Name, 0, 15, 15, PlayerAttributes, 50);

            // give player default inventory items 
            player.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            player.LearnRecipe(RecipeFactory.RecipeByID(1));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            player.AddItemToInventory(ItemFactory.CreateGameItem(3003));

            return player;
        }
    }
}
;