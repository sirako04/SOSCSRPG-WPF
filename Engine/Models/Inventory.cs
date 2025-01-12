﻿using Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Engine.Models
{
    public class Inventory
    {
        #region Backing variables
        private readonly List<GameItem> _backingInventory = new List<GameItem>();
        private readonly List<GroupedInventoryItem> _backingGroupedInventoryItems = new List<GroupedInventoryItem>();
        #endregion

        #region Properties

        public IReadOnlyList<GameItem> Items => _backingInventory.AsReadOnly();
        [JsonIgnore]
        public IReadOnlyList<GroupedInventoryItem> GroupedInventory => _backingGroupedInventoryItems.AsReadOnly();
        [JsonIgnore]
        public IReadOnlyList<GameItem> Weapons => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Weapon).AsReadOnly();
        [JsonIgnore]
        public IReadOnlyList<GameItem> Consumables =>
           _backingInventory.ItemsThatAre(GameItem.ItemCategory.Consumable).AsReadOnly();
        
        [JsonIgnore]
        public bool HasConsumable => Consumables.Any();
        #endregion
        public Inventory(IEnumerable<GameItem> items = null) 
        {
            if (items == null)
            {
                return;
            }
            foreach (GameItem item in items)
            {
                _backingInventory.Add(item);
                AddItemToGroupedInventory(item);
            }
        }
        #region Public functions
        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items)
        {
            return items.All(item => Items.Count(i => i.ItemTypeID == item.ItemID) >= item.Quantity);
        }
        #endregion
        #region Private functions
        // REFACTOR: Look for a better way to do this (extension method?)
        private void AddItemToGroupedInventory(GameItem item)
        {
            if (item.IsUnique)
            {
                _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item));
            }
            else
            {
                if (_backingGroupedInventoryItems.All(gi => gi.Item.ItemTypeID != item.ItemTypeID))
                {
                    _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 0));
                }
                _backingGroupedInventoryItems.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }
        }
        #endregion
    }
}
