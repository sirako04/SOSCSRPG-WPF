﻿using Engine.Models;
using Engine.Services;
using System;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly string _damageDice;
       
        public AttackWithWeapon(GameItem itemInUse, string damageDice) 
            :base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($" {itemInUse.Name} is not a weapon");
            }
            if (string.IsNullOrWhiteSpace(damageDice))
            {
                throw new ArgumentException("damageDice must be valid dice notation");
            }

            _damageDice = damageDice;
            
        }
        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorname =(actor is Player)? "You": $"The {actor.Name.ToLower()}";
            string targetname =(target is Player)? "you": $"the {target.Name.ToLower()}";

            if (CombatService.AttackSucceeded(actor, target))
            {
                int damage = (int)DiceService.Instance.Roll(_damageDice).Value;
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
