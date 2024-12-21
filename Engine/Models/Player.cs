using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        private string _characterClass;
        private int _experiencePoints;
        public string CharacterClass
        {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }

        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged();

                SetLevelAndMaximumHitPoints();
            }
        }
       
        public ObservableCollection<QuestStatus> Quests { get; }
        public ObservableCollection<Recipe> Recipes { get; }

        public event EventHandler OnLeveledUp;
        public Player(string name, string characterClass, int experiencePoints,
                       int maximumHitPoints, int currentHitPoints, int gold)
            : base(name, maximumHitPoints, currentHitPoints, gold)
        {
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;
            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }
        public void AddExperience(int expToAdd)
        {
            ExperiencePoints += expToAdd;
        }
        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.ID == recipe.ID))
            {        
                Recipes.Add(recipe);
            }
        }
        public void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            Level = (ExperiencePoints / 80) + 1;
            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 15;
                CompletelyHeal();
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }
}
