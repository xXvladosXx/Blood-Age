namespace AttackSystem.Weapon
{
    using System.Text;
    using InventorySystem;
    using UnityEngine;

    public abstract class StandardWeapon : StatModifItem
    {
        [SerializeField] protected bool _rightHanded;
        public bool IsRightHand => _rightHanded;
        
        [SerializeField] protected float _attackRange;
        public float GetAttackDistance => _attackRange;


        [SerializeField] protected RuntimeAnimatorController _animatorController;
        
        [SerializeField] protected GameObject _weaponPrefab;
        [SerializeField] protected bool _isRanged;
        public GameObject GetPrefab => _weaponPrefab;
        public bool GetIsRanged => _isRanged;

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

            if (_healthBonus != 0)
            {
                stringBuilder.Append("Health: ").Append(_healthBonus).AppendLine();
            }

            return stringBuilder.ToString();
        }

    }
}