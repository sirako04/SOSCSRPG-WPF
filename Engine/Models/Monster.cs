using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Monster :Notification
    {
        private int _hitPoints;
        private int _maxHitPoints;
        public string Name { get; private set; }
        public string ImageName { get; set; }
        public int MaximumHitPoints
        {
            get { return _maxHitPoints; }
            set
            {
                _maxHitPoints = value;
                OnPropertyChanged(nameof(MaximumHitPoints));  
            }
        }      
        public int HitPoints 
        {
            get {  return _hitPoints; }
            set 
            {
                _hitPoints = value;
                OnPropertyChanged(nameof(HitPoints));
            } 
        }
        public int MaxDamage {get; set;}
        public int MinDamage {get; set;}
        public int RewardExperiencePoints { get; private set; }
        public int RewardGold { get; private set; }
        public ObservableCollection<ItemQuantity> Inventory {  get; set; }
        public Monster(string name, string imageName, int maximumHitPoints,
            int hitPoints, int minDamage, int maxDamage, int rewardExperiencePoints, int rewardGold)
        {
            Name = name;
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            MaximumHitPoints = maximumHitPoints;
            HitPoints = hitPoints;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;

            Inventory = new ObservableCollection<ItemQuantity>();
        }
    }
}
