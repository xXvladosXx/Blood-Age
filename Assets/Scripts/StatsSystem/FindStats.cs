namespace DefaultNamespace
{
    using System.Linq;
    using DefaultNamespace.Entity;
    using UnityEngine;

    public class FindStats : MonoBehaviour
    {
        [SerializeField] private StarterCharacterData _starterCharacterData;
        [SerializeField] private Class _character = Class.Warrior;

        public float GetStat(Characteristics characteristics)
        {
            float starterValue = _starterCharacterData.ReturnLevelValueCharacteristics(_character, characteristics, 1);
            float valueWithBonus = GetBonus(characteristics) + starterValue;

            return valueWithBonus;
        }

        private float GetBonus(Characteristics characteristic)
        {
            bool IsBonusAssignableToCharacteristics(IBonus bonus) 
                => (bonus, characteristic) switch
                {
                    (HealthBonus b, Characteristics.Health) => true,
                    (DamageBonus b, Characteristics.Damage) => true,
                    (CriticalChanceBonus b, Characteristics.CriticalChance) => true,
                    (CriticalDamageBonus b, Characteristics.CriticalDamage) => true,
                    _ => false
                };

            return GetComponents<IModifier>()
                .SelectMany(x => x.AddBonus(new[] { characteristic }))
                .Where(IsBonusAssignableToCharacteristics)
                .Sum(x => x.Value);
        }
    }
}