using Entity;
using UnityEngine;

namespace InventorySystem.Items
{
    [CreateAssetMenu (menuName = "Inventory/Potion/SpeedPotion")]
    public class AttackSpeedPotion : Potion
    {
        [SerializeField] private float _attackSpeedMultiplier;
        [SerializeField] private float _time;

        private string _attackSpeed = "AttackSpeedMultiplier";
        private float _startAttackSpeed;

        private LTDescr _leanTween;
        public override string ItemInfo()
        {
            return "Attack speed potion";
        }

        public override void UsePotion(AliveEntity aliveEntity)
        {
            var animator = aliveEntity.GetComponent<Animator>();
            _startAttackSpeed = animator.GetFloat(_attackSpeed);
            animator.SetFloat(_attackSpeed, _startAttackSpeed + _attackSpeedMultiplier);
            if (_leanTween != null)
            {
                LeanTween.cancel(_leanTween.uniqueId);
            }

            _leanTween = LeanTween.delayedCall(_time, () =>
            {
                animator.SetFloat(_attackSpeed, _startAttackSpeed);
                Debug.Log("Normal");
            });
        }
    }
}