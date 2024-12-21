﻿using System.Collections.Generic;

namespace Engine.Models
{

    public class Quest
    {
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }

        public List<ItemQuantity> ItemsToComplete { get; }

        public int RewardEXP { get; }
        public int RewardGold { get; }
        public List<ItemQuantity> RewardItems { get; }
        public Quest(int id, string name, string description,
        List<ItemQuantity> itemsToComplete, int rewardEXP, int rewardGold,
        List<ItemQuantity> rewardItems) 
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardEXP = rewardEXP;
            RewardGold = rewardGold;
            RewardItems = rewardItems;
        }
    }
}
