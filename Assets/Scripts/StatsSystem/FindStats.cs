namespace DefaultNamespace
{
    using System.Linq;
    using UnityEngine;

    public class FindStats : MonoBehaviour
    {
        [SerializeField] private StarterCharacterData _starterCharacterData;
        [SerializeField] private Class _character = Class.Warrior;
        public Class GetClass => _character;

        public float GetStat(Class classChooser, Characteristics characteristics)
        {
            float starterValue = _starterCharacterData.ReturnLevelValueCharacteristics(classChooser, characteristics, 1);
            float valueWithBonus = GetBonus(characteristics) + starterValue;

            return valueWithBonus;
        }

        private float GetBonus(Characteristics characteristic)
        {
            bool IsBonusAssignToCharacteristics(IBonus bonus) 
                => (bonus, characteristic) switch
                {
                    (HealthBonus b, Characteristics.Health) => true,
                    (DamageBonus b, Characteristics.Damage) => true,
                    (CriticalChanceBonus b, Characteristics.CriticalChance) => true,
                    (CriticalDamageBonus b, Characteristics.CriticalDamage) => true,
                    (MovementSpeedBonus b, Characteristics.MovementSpeed) => true,
                    (AttackSpeedBonus b, Characteristics.AttackSpeed) => true,
                    (ManaRegenerationBonus b, Characteristics.ManaRegeneration) => true,
                    (HealthRegenerationBonus b, Characteristics.HealthRegeneration) => true,
                    _ => false
                };

            return GetComponents<IModifier>()
                .SelectMany(x => x.AddBonus(new[] { characteristic }))
                .Where(IsBonusAssignToCharacteristics)
                .Sum(x => x.Value);
        }

        public void AddTemporaryBonus(Characteristics characteristic, float value)
        {
            
        }
    }
}