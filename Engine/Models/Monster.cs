

namespace Engine.Models
{
    public class Monster :LivingEntity
    {
        public string ImageName { get; set; }             
        public int MaxDamage {get; set;}
        public int MinDamage {get; set;}
        public int RewardExperiencePoints { get; private set; }
        public Monster(string name, string imageName, int maximumHitPoints,
            int hitPoints, int minDamage, int maxDamage, int rewardExperiencePoints, int rewardGold)
        {
            Name = name;
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = hitPoints;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            Gold = rewardGold;
        }
    }
}
