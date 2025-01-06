using Engine.Factories;
using Engine.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get;}
        public int YCoordinate { get;}
        [JsonIgnore]
        public string Name { get; }
        [JsonIgnore]
        public string Description { get; }
        [JsonIgnore]
        public string ImageName { get; }
        [JsonIgnore]
        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();
        [JsonIgnore]
        public List<MonsterEncounter> MonstersHere { get; }
            = new List<MonsterEncounter>();
        [JsonIgnore]
        public Trader TraderHere { get; set; }

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

            int randomNumber = DiceService.Instance.Roll(totalChances, 1).Value;

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
