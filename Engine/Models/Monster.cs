

namespace Engine.Models
{
    public class Monster :LivingEntity
    {
        public string ImageName { get; set; }             
        public int MaxDamage {get; set;}
        public int MinDamage {get; set;}
        public int RewardExperiencePoints { get; private set; }
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
