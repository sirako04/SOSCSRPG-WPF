﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Engine.Models;
using Engine.Shared;
using System;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";
        private static readonly List<Monster> _baseMonsters = new List<Monster>();
        static MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                string rootImagePath = data.SelectSingleNode("Monsters")
                    .AttributeAsString("RootImagePath");

                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Monster"),
                    rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($" file : {GAME_DATA_FILENAME} not found ! "); 
            }
        }
        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null)
            {
                return;
            }
            foreach (XmlNode node in nodes)
            {
                Monster monster = new Monster(node.AttributeAsInt("ID"),
                                node.AttributeAsString("Name"),
                                $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                                node.AttributeAsInt("MaximumHitPoints"),
                                Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText),
                                ItemFactory.CreateGameItem(node.AttributeAsInt("WeaponID")),
                                node.AttributeAsInt("RewardXP"),
                                node.AttributeAsInt("Gold"));
                XmlNodeList lootItemNodes = 
                    node.SelectNodes("./LootItems/LootItem");
                if (lootItemNodes != null)
                {
                    foreach (XmlNode lootItemNode in lootItemNodes)
                    {
                        monster.AddItemToLootTable(lootItemNode.AttributeAsInt("ID"),
                            lootItemNode.AttributeAsInt("Percentage"));
                    }
                }
                _baseMonsters.Add(monster);
            }
        }
        public static Monster GetMonster(int id)
        {
            return _baseMonsters.FirstOrDefault
                (m => m.ID == id)?.GetNewInstance();
        }
    }
}
