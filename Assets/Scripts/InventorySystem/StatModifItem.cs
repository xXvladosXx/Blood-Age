namespace InventorySystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DefaultNamespace;

    public abstract class StatModifItem : Item, IModifier
    {
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