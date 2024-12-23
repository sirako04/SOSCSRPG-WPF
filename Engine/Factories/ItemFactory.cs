using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using Engine.Models;
using Engine.Actions;
using Engine.Shared;
using System.Runtime.CompilerServices;
namespace Engine.Factories
{
    public static class ItemFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\GameItems.xml";

       private static readonly List<GameItem> _stdGameItems = new List<GameItem>();

        static ItemFactory() 
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/Weapons/Weapon"));
                LoadItemsFromNodes(data.SelectNodes("/GameItems/HealingItems/HealingItem"));
                LoadItemsFromNodes
                (data.SelectNodes("/GameItems/MiscellaneousItems/MiscellaneousItem"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }
        public static GameItem CreateGameItem(int itemTypeID) 
        {
           return  _stdGameItems.FirstOrDefault
           (item => item.ItemTypeID == itemTypeID)?.Clone();        
        }
        public  static string ItemName(int itemTypeID)
        {
            return _stdGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID)?.Name ?? "";
        }
        private static void LoadItemsFromNodes(XmlNodeList nodes) 
        {
            if (nodes == null)
            {
                return;
            }
            foreach (XmlNode node in nodes) 
            {
               GameItem.ItemCategory itemCategory = DetermineItemCategory(node.Name);
                GameItem gameItem = new GameItem(itemCategory,
                    node.AttributeAsInt("ID"),
                    node.AttributeAsString("Name"),
                    node.AttributeAsInt("Price"),
                    itemCategory == GameItem.ItemCategory.Weapon);
                if (itemCategory == GameItem.ItemCategory.Weapon)
                {
                    gameItem.Action = new AttackWithWeapon(gameItem,
                        node.AttributeAsInt("MinDamage"),
                        node.AttributeAsInt("MaxDamage"));
                }
                else if (itemCategory == GameItem.ItemCategory.Consumable)
                {
                    gameItem.Action = new Heal(gameItem, node.AttributeAsInt("HitPointsToHeal"));
                }
                _stdGameItems.Add(gameItem);
            }
        }
        private static GameItem.ItemCategory DetermineItemCategory(string itemType)
        {
            switch (itemType)
            {
                case "Weapon":
                    return GameItem.ItemCategory.Weapon;
                case "HealingItem":
                    return GameItem.ItemCategory.Consumable;
                default:
                    return GameItem.ItemCategory.Miscellaneous;
            }
        }
    }
}
