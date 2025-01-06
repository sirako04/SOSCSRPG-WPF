﻿using Engine.Services;
using Engine.Factories;
using Engine.Actions;
using System.Collections.Generic;

namespace Engine.Models
{
    public class Monster :LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable =
            new List<ItemPercentage>();
        public int ID { get; }
        public string ImageName { get; }
        public int RewardExperiencePoints { get; }
        public Monster(int id, string name, string imageName,
                       int maximumHitPoints,int dexterity,
                       GameItem currentWeapon,
                       int rewardExperiencePoints, int gold) :
            base(name, maximumHitPoints, maximumHitPoints, dexterity, gold)
        {
            ID = id;
            ImageName = imageName;
            CurrentWeapon = currentWeapon;
            RewardExperiencePoints = rewardExperiencePoints;
        }
        public void AddItemToLootTable(int id, int percentage)
        {
            // Remove entry from table if it already contains entry with ID
            _lootTable.RemoveAll(ip => ip.ID == id);

            _lootTable.Add(new ItemPercentage(id, percentage));
        }
        public Monster GetNewInstance()
        {
            Monster newMonster =
                new Monster(ID, Name, ImageName, MaximumHitPoints,Dexterity,
                            CurrentWeapon, RewardExperiencePoints, Gold);

            foreach (ItemPercentage itemPercentage in _lootTable)
            {
                // Cloning loot table 
                newMonster.AddItemToLootTable(itemPercentage.ID, itemPercentage.Percentage);
                // Populating the new Monster`s Inventory using the loot table
                if (DiceService.Instance.Roll(100).Value <= itemPercentage.Percentage)
                {
                    newMonster.AddItemToInventory
                   (ItemFactory.CreateGameItem(itemPercentage.ID));
                }
            }

            return newMonster;
        }
    }
}
