﻿using Engine.Models;
using System;

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

            if (_minimumDamage < 0)
            {
                throw new ArgumentException("MinimumDamage must be 0 or larger");
            }
            if (_maximumDamage < _minimumDamage )
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

            int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);
            if (damage == 0)
            {
                ReportResult($"{actorname} missed {targetname}. ");                     
            }
            else
            {
                ReportResult($"{actorname} hit {targetname} for {damage} point{(damage >1? "s":"")}.");
                target.TakeDamage(damage);
            }
        }
    }
}
