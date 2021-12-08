namespace AttackSystem.Weapon
{

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
        
    }
}