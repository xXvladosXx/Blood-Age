namespace DefaultNamespace.Entity
{
    using System;
    using System.Collections.Generic;
    using global::SkillSystem.MainComponents;
    using InventorySystem;
    using Sirenix.Serialization;
    using UnityEngine;

    [RequireComponent(typeof(ItemEquipper))]
    [RequireComponent(typeof(BuffApplier))]
    [RequireComponent(typeof(FindStats))]
    public abstract class AliveEntity : MonoBehaviour
    {
        protected List<Race.Race> EnemyRaces = new List<Race.Race>();

        protected FindStats FindStats;
        protected Health Health;
        protected Race.Race Race; 
        public Health GetHealth => Health;
        public ItemEquipper GetItemEquipper => ItemEquipper;
        public Race.Race GetRace => Race;

        protected ItemEquipper ItemEquipper;
        protected BuffApplier BuffApplier;
        private Animator _animator;
        
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");

        private void Awake()
        {
            FindStats = GetComponent<FindStats>();
            ItemEquipper = GetComponent<ItemEquipper>();
            BuffApplier = GetComponent<BuffApplier>();
            _animator = GetComponent<Animator>();
            
            Health = new Health(FindStats.GetStat(Characteristics.Health));
            
            Init();
        }

        private void OnEnable()
        {
            BuffApplier.OnBonusAdded += UpdateCharacteristics;
            Health.OnDie += PlayDieAnimation;
            Health.OnHeavyAttackHit += PlayHeavyAttackFalling;
        }

        private void PlayDieAnimation()
        {
            _animator.SetBool(Dead, true);
        }

        private void PlayHeavyAttackFalling()
        {
            _animator.SetTrigger(HeavyAttack);
        }

        private void OnDisable()
        {
            BuffApplier.OnBonusAdded -= UpdateCharacteristics;
            Health.OnDie -= PlayDieAnimation;
            Health.OnHeavyAttackHit -= PlayHeavyAttackFalling;
        }

        private void UpdateCharacteristics()
        {
            Health.RenewHealthPoints(FindStats.GetStat(Characteristics.Health));
        }

        protected abstract void Init();

        public float GetStat(Characteristics characteristic)
        {
            return FindStats.GetStat(Characteristics.CriticalChance);
        }
    }
}