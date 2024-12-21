using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Actions;
namespace Engine.Factories
{
    public static class ItemFactory
    {
       private static readonly List<GameItem> _stdGameItems = new List<GameItem>();

        static ItemFactory() 
        {
            BuildWeapon(1001, "Pointy Stick", 1, 1, 2);
            BuildWeapon(1002, "Rusty Sword", 5, 2, 4);
            BuildWeapon(1501, "snake fangs", 0, 0, 2);
            BuildWeapon(1502, "rat claws", 0, 0, 2);
            BuildWeapon(1503, "spider fangs", 0, 1, 4);

            BuildHealItem(2001, "Granola bar", 5, 4);
            BuildMiscellaneousItem(3001, "Oats", 1);
            BuildMiscellaneousItem(3002, "Honey", 2);
            BuildMiscellaneousItem(3003, "Raisins", 2);

            BuildMiscellaneousItem(9001, "Snake fang", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", 3);
            BuildMiscellaneousItem(9003, "Rat tail", 1);
            BuildMiscellaneousItem(9004, "Rat fur", 2);
            BuildMiscellaneousItem(9005, "Spider fang", 2);
            BuildMiscellaneousItem(9006, "Spider silk", 4);
        }
        public static GameItem CreateGameItem(int itemTypeID) 
        {
           return  _stdGameItems.FirstOrDefault
           (item => item.ItemTypeID == itemTypeID)?.Clone();        
        }
        private static void BuildMiscellaneousItem(int id, string name, int price)
        {

            _stdGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name
                , price));
        }
        private static void BuildWeapon(int id, string name, int price, int minimumDamage, int maximumDamage)
        {
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price, true);
            weapon.Action = new AttackWithWeapon(weapon, minimumDamage, maximumDamage);

            _stdGameItems.Add(weapon);
        }
        private static void BuildHealItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            item.Action = new Heal(item, hitPointsToHeal);

            _stdGameItems.Add(item);
        }
        public  static string ItemName(int itemTypeID)
        {
            return _stdGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID)?.Name ?? "";
        }
    }
}
