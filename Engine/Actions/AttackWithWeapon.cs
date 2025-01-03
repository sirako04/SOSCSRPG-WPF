using Engine.Models;
using Engine.Services;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly int _maximumDamage;
        private readonly int _minimumDamage;
        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage) :base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($" {itemInUse.Name} is not a weapon");
            }

            if (minimumDamage < 0)
            {
                throw new ArgumentException("MinimumDamage must be 0 or larger");
            }
            if (maximumDamage < minimumDamage )
            {
                throw new ArgumentException("MaximumDamage should be greater than minimumDamage");
            }
            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorname =(actor is Player)? "You": $"The {actor.Name.ToLower()}";
            string targetname =(target is Player)? "you": $"the {target.Name.ToLower()}";

            if (CombatService.AttackSucceeded(actor, target))
            {
                int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);
                ReportResult($"{actorname} hit {targetname} for {damage} point{(damage > 1 ? "s" : "")}.");
                
                target.TakeDamage(damage);
                                    
            }
            else
            {
                ReportResult($"{actorname} missed {targetname}. ");
            }
        }
    }
}
