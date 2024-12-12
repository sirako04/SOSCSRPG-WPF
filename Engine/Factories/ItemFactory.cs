using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
namespace Engine.Factories
{
    public static class ItemFactory
    {
       private static List<GameItem> _stdGameItems = new List<GameItem>();

        static ItemFactory() 
        {
            _stdGameItems.Add(new Weapon(1001,"Pointy Stick",1,1,2));
            _stdGameItems.Add(new Weapon(1002, "Rusty Sword", 5, 1, 3));
            _stdGameItems.Add(new GameItem(9001, "Snake fang", 1));
            _stdGameItems.Add(new GameItem(9002, "Snakeskin", 3));
        }
        public static GameItem CreateGameItem(int itemTypeID) 
        {
            GameItem standardItem = _stdGameItems.FirstOrDefault
                         (item => item.ItemTypeID == itemTypeID);
            if (standardItem != null) 
            {
                return standardItem.Clone();
            }

            return null;
        }
    }
}
