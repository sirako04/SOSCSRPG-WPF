using System;
using System.Collections.Generic;
using System.Linq;

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
