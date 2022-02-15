using System;
using System.Text;
using Resistance;
using UnityEngine;

namespace InventorySystem.Items.Armor
{
    public abstract class StandardArmor : StatModifInventoryItem, IEquipable
    {
        [SerializeField] protected DamageResistance.Resistance[] _damageResistances;
        public DamageResistance.Resistance[] GetDamageResistances => _damageResistances;
        public InventoryItem GetItem()
        {
            return this;
        }
        
        public override string ItemInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (_healthBonus != 0)
            {
                stringBuilder.Append("Health: ").Append(_healthBonus).AppendLine();
            }
            
            if (_healthRegenerationBonus != 0)
            {
                stringBuilder.Append("Health reg: ").Append(_healthRegenerationBonus).AppendLine();
            }

            if (_manaBonus != 0)
            {
                stringBuilder.Append("Mana: ").Append(_manaBonus).AppendLine();
            }
            
            if (_manaRegenerationBonus != 0)
            {
                stringBuilder.Append("Mana reg: ").Append(_manaRegenerationBonus).AppendLine();
            }

            if (_evasionBonus != 0)
            {
                stringBuilder.Append("Evasion: ").Append(_evasionBonus).AppendLine();
            }

            stringBuilder.AppendLine();
            if(_damageResistances.Length == 0) return stringBuilder.ToString();
            
            stringBuilder.Append("Resistance:").AppendLine();
            foreach (var damage in _damageResistances)
            {
                switch (damage.DamageType)
                {
                    case DamageType.Fire:
                        if (damage.DamageResistance != 0)
                            stringBuilder.Append("<color=#FF8560>").Append("Fire: ").Append(damage.DamageResistance*100).Append("%").Append("</color>").AppendLine();
                        break;
                    case DamageType.Ice:
                        if (damage.DamageResistance != 0)
                            stringBuilder.Append("<color=#B8F3FF>").Append("Ice: ").Append(damage.DamageResistance*100).Append("%").Append("</color>").AppendLine();
                        break;
                    case DamageType.Physical:
                        if (damage.DamageResistance != 0)
                            stringBuilder.Append("<color=#C1C1C1>").Append("Physical: ").Append(damage.DamageResistance*100).Append("%").Append("</color>").AppendLine();
                        break;
                    case DamageType.Ground:
                        if (damage.DamageResistance != 0)
                            stringBuilder.Append("<color=#7B432C>").Append("Ground: ").Append(damage.DamageResistance*100).Append("%").Append("</color>").AppendLine();
                        break;
                    case DamageType.Thunder:
                        if (damage.DamageResistance != 0)
                            stringBuilder.Append("<color=#B8F3FF>").Append("Thunder: ").Append(damage.DamageResistance).Append("%").Append("</color>").AppendLine();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
            
            return stringBuilder.ToString();
        }
    }
}