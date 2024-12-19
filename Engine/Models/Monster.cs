

namespace Engine.Models
{
    public class Monster :LivingEntity
    {
        public string ImageName { get; }             
        public int MaxDamage {get; }
        public int MinDamage {get; }
        public int RewardExperiencePoints { get; }
        public Monster(string name, string imageName, int maximumHitPoints,
            int currentHitPoints, int minDamage, int maxDamage, int rewardExperiencePoints, int gold) :
            base(name, maximumHitPoints, currentHitPoints, gold)
        {
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";          
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            RewardExperiencePoints = rewardExperiencePoints;
           
        }
    }
}
