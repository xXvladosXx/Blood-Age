namespace InventorySystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;
    using UnityEngine;

    public abstract class StatModifItem : Item, IModifier
    {
        [SerializeField] protected float _damageBonus = 0;
        [SerializeField] protected float _healthBonus = 0;
        [SerializeField] protected float _criticalDamage = 0;
        [SerializeField] protected float _criticalChance;

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
                    _ => throw new ArgumentOutOfRangeException(nameof(characteristics), characteristics, null)
                };
            }


            return characteristics.Select(BonusTo);
        }
    }

}

