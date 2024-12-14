using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public List<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();
        public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();

        public Location(int xCoordinate, int yCoordinate,
        string name, string description, string imageName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = description;
            ImageName = imageName;
        }
        public void AddMonster(int monsterID, int chanceofEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterID == monsterID))
            {
                MonstersHere.First
                (m => m.MonsterID == monsterID).ChanceOfEncountering = chanceofEncountering;
            }
            else
            {
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceofEncountering));
            }
        }
        public Monster GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }
            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);

            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);

            int runningTotal = 0;
            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;
                if (randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterID);
                }
            }
            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);
        }
    }
}
