namespace AttackSystem.Weapon
{
    using DefaultNamespace;
    using InventorySystem;
    using UnityEditor.Animations;
    using UnityEngine;

    public abstract class StandardWeapon : Item
    {
        [SerializeField] protected PlayerPassiveSkillBonus[] _playerPassiveSkillBonus;
        public PlayerPassiveSkillBonus[] GetBonuses => _playerPassiveSkillBonus;
        
        [SerializeField] protected bool _rightHanded;
        public bool IsRightHand => _rightHanded;
        [SerializeField] protected float _damage;
        public float GetAttackDamage => _damage;
        
        [SerializeField] protected float _attackRange;
        public float GetAttackDistance => _attackRange;

        [SerializeField] protected float _criticalChance;
        public float GetCriticalChance => _criticalChance;
        [SerializeField] protected float _criticalDamage;
        public float GetCriticalDamage => _criticalDamage;

        [SerializeField] protected float _healthBonus;
        public float GetHealthBonus => _healthBonus;

        [SerializeField] protected AnimatorController _animatorController;
        
        [SerializeField] protected GameObject _weaponPrefab;
        [SerializeField] protected bool _isRanged;
        public GameObject GetPrefab => _weaponPrefab;
        public bool GetIsRanged => _isRanged;

        public abstract void EquipWeapon(Animator animator);
    }
}