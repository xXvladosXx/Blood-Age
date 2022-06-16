using System;
using System.Text;
using Extensions;
using Resistance;
using UnityEngine;

namespace InventorySystem.Items.Weapon
{
    public abstract class StandardWeapon : StatModifInventoryItem
    {
        [SerializeField] protected RPGCharacterAnims.Weapon _weaponType;
        [SerializeField] protected bool _rightHanded;
        [SerializeField] protected float _attackRange;
        [SerializeField] protected RuntimeAnimatorController _animatorController;
        [SerializeField] private SerializableDictionary _additionalDamage;
        [SerializeField] protected GameObject _weaponPrefab;
        public GameObject GetPrefab => _weaponPrefab;
        public float GetAttackDistance => _attackRange;
        public bool IsRightHand => _rightHanded;
        public RPGCharacterAnims.Weapon GetWeaponType => _weaponType;
        public SerializableDictionary GetDamageDictionary => _additionalDamage;
        
        public abstract void EquipWeapon(Animator animator);

        public override string ItemInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (_damageBonus != 0)
            {
                stringBuilder.Append("Damage: ").Append(_damageBonus).AppendLine();
            }

            if (_criticalChance != 0)
            {
                stringBuilder.Append("Cr. chance: ").Append(_criticalChance).AppendLine();
            }

            if (_criticalDamage != 0)
            {
                stringBuilder.Append("Cr. damage: ").Append(_criticalDamage).AppendLine();
            }

            if (_accuracyBonus != 0)
            {
                stringBuilder.Append("Accuracy: ").Append(_accuracyBonus).AppendLine();
            }

            if (_attackSpeedBonus != 0)
            {
                stringBuilder.Append("At. speed: ").Append(_attackSpeedBonus).AppendLine();
            }

            stringBuilder.AppendLine();
            foreach (var damage in _additionalDamage)
            {
                switch (damage.Key)
                {
                    case DamageType.Fire:
                        if (damage.Value != 0)
                            stringBuilder.Append("<color=#FF8560>").Append("Fire: ").Append(damage.Value).Append("</color>").AppendLine();
                        break;
                    case DamageType.Ice:
                        if (damage.Value != 0)
                            stringBuilder.Append("<color=#B8F3FF>").Append("Ice: ").Append(damage.Value).Append("</color>").AppendLine();
                        break;
                    case DamageType.Physical:
                        if (damage.Value != 0)
                            stringBuilder.Append("<color=#C1C1C1>").Append("Physical: ").Append(damage.Value).Append("</color>").AppendLine();
                        break;
                    case DamageType.Ground:
                        if (damage.Value != 0)
                            stringBuilder.Append("<color=#7B432C>").Append("Ground: ").Append(damage.Value).Append("</color>").AppendLine();
                        break;
                    case DamageType.Thunder:
                        if (damage.Value != 0)
                            stringBuilder.Append("<color=#B8F3FF>").Append("Thunder: ").Append(damage.Value).Append("</color>").AppendLine();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return stringBuilder.ToString();
        }

    }
}