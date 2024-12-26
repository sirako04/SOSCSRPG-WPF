using Engine.Models;
using Engine.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Traders.xml";
        private static readonly List<Trader> _traders = new List<Trader>();
        static TraderFactory() 
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadTradersFromNodes(data.SelectNodes("Traders/Trader"));
            }
            else
            {
               throw new FileNotFoundException($"{GAME_DATA_FILENAME} not found!!");
            }
        }
        private static void LoadTradersFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                Trader trader = new Trader(node.AttributeAsInt("ID"),
                    node.SelectSingleNode("./Name")?.InnerText ?? "");
                foreach (XmlNode childnode in node.SelectNodes("./InventoryItems/Item"))
                {
                    int quantity = childnode.AttributeAsInt("Quantity");
                    for (int i = 0; i < quantity; i++)
                    {
                        trader.AddItemToInventory(ItemFactory.CreateGameItem(childnode.AttributeAsInt("ID")));
                    }
                }
                _traders.Add(trader);
            }
        }
        public static Trader GetTraderByID(int id)
        {
            return _traders.FirstOrDefault(t => t.ID == id);
        }     
    }
}
