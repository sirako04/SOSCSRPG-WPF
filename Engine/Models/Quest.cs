﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{

    public class Quest
    {
        public int ID { get; }
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public List<ItemQuantity> ItemsToComplete { get; }
        [JsonIgnore]
        public int RewardEXP { get; }
        [JsonIgnore]
        public int RewardGold { get; }
        [JsonIgnore]
        public List<ItemQuantity> RewardItems { get; }
        [JsonIgnore]
        public string ToolTipContents => Description + Environment.NewLine +
            "Items to complete the quest" + Environment.NewLine +
            "===========================" + Environment.NewLine +
            string.Join(Environment.NewLine,ItemsToComplete.Select(i =>
            i.QuantityItemDescription)) +
            Environment.NewLine + Environment.NewLine +
            "Rewards\r\n" +
             "===========================" + Environment.NewLine +
            $"{RewardEXP} experience points" + Environment.NewLine +
            $"{RewardGold} gold pieces" + Environment.NewLine +
            string.Join(Environment.NewLine,RewardItems.Select(i =>
            i.QuantityItemDescription));
        
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
