using System;
using System.Collections.Generic;
using System.Linq;
using StatsSystem;
using StatsSystem.Bonuses;
using UnityEngine;

namespace InventorySystem.Items
{
    public abstract class StatModifInventoryItem : InventoryItem, IModifier
    {
        [SerializeField] protected float _damageBonus = 0;
        [SerializeField] protected float _healthBonus = 0;
        [SerializeField] protected float _criticalDamage = 0;
        [SerializeField] protected float _criticalChance;
        [SerializeField] protected float _attackSpeedBonus;
        [SerializeField] protected float _movementSpeedBonus;
        [SerializeField] protected float _healthRegenerationBonus;
        [SerializeField] protected float _manaBonus;
        [SerializeField] protected float _manaRegenerationBonus;
        [SerializeField] protected float _evasionBonus;
        [SerializeField] protected float _accuracyBonus;

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IBonus BonusTo(Characteristics characteristics)
            {
                return characteristics switch
                {
                    Characteristics.Damage => new DamageBonus(_damageBonus),
                    Characteristics.Health => new HealthBonus(_healthBonus),
                    Characteristics.CriticalChance => new CriticalChanceBonus(_criticalChance),
                    Characteristics.CriticalDamage => new CriticalDamageBonus(_criticalDamage),
                    Characteristics.DeathExperience => new DeathExperienceBonus(0),
                    Characteristics.ExperienceToLevelUp => new DeathExperienceBonus(0),
                    Characteristics.AttackSpeed => new AttackSpeedBonus(_attackSpeedBonus),
                    Characteristics.MovementSpeed => new MovementSpeedBonus(_movementSpeedBonus),
                    Characteristics.ManaRegeneration => new ManaRegenerationBonus(_manaRegenerationBonus),
                    Characteristics.HealthRegeneration => new HealthRegenerationBonus(_healthRegenerationBonus),
                    Characteristics.Mana => new ManaBonus(_manaBonus),
                    Characteristics.Evasion => new EvasionBonus(_evasionBonus),
                    Characteristics.Accuracy => new AccuracyBonus(_accuracyBonus),
                    _ => throw new ArgumentOutOfRangeException(nameof(characteristics), characteristics, null)
                };
            }


            return characteristics.Select(BonusTo);
        }
    }

}

