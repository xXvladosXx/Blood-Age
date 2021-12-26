namespace DefaultNamespace.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using a;
    using DefaultNamespace.Entity.Class;
    using global::Entity.Class;
    using global::SkillSystem.MainComponents;
    using InventorySystem;
    using Sirenix.Serialization;
    using StatsSystem;
    using UnityEngine;

    [RequireComponent(typeof(ItemEquipper))]
    [RequireComponent(typeof(BuffApplier))]
    [RequireComponent(typeof(FindStats))]
    [RequireComponent(typeof(Animator))]
    public abstract class AliveEntity : MonoBehaviour, IModifier
    {
        [SerializeField] private int _defaultNumberOfPoints = 2;
        [SerializeField] private DefaultNamespace.Class _serializableClass = DefaultNamespace.Class.Warrior;
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private int _startingLevel = 1;
        [SerializeField] private StarterCharacterData _starterCharacterData;

        [SerializeField] private ParticleSystem _particle;
        
        protected List<Race.Race> EnemyRaces = new List<Race.Race>();
        
        private LevelUp _levelUp;
        private StatsValueStore _statsValueStore;
        private ItemEquipper _itemEquipper;
        private BuffApplier _buffApplier;
        private FindStats _findStats;
        private Health _health;
        protected Race.Race Race; 
        private BaseCharacterClass _characterClass;
        private List<StatBonus> _stats = new List<StatBonus>();

        public Health GetHealth => _health;
        public ItemEquipper GetItemEquipper => _itemEquipper;
        public Race.Race GetRace => Race;
        public StatsValueStore GetStatsValueStore => _statsValueStore;

        private Animator _animator;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");

        public Action<Dictionary<CharacteristicBonus, float>> OnBuffsApply;

        private void Awake()
        {
            _characterClass = _serializableClass switch
            {
                DefaultNamespace.Class.Warrior => new Warrior(),
                DefaultNamespace.Class.Archer => new Archer(),
                DefaultNamespace.Class.Wizard => new Wizard(),
                _ => _characterClass
            };
            
            _findStats = GetComponent<FindStats>();
            _findStats.SetClass(_serializableClass);
            
            _statsValueStore = new StatsValueStore(_findStats, _characterClass);
            _health = new Health(_findStats.GetStat(Characteristics.Health));
            _levelUp = new LevelUp();
            
            _buffApplier = GetComponent<BuffApplier>();
            _itemEquipper = GetComponent<ItemEquipper>();
            _animator = GetComponent<Animator>();
            
            Init();
        }

        private void OnEnable()
        {
            _buffApplier.OnBonusAdd += UpdateCharacteristics;
            
            _health.OnDie += PlayDieAnimation;
            _health.OnHeavyAttackHit += PlayHeavyAttackFalling;
            
            _levelUp.OnExperienceGive += LevelUpOnOnExperienceGive; 
            _statsValueStore.OnStatsChange += StatsValueStoreOnOnStatsChange;
        }

        private void StatsValueStoreOnOnStatsChange()
        {
            
        }

        private void Update()
        {
            print(_findStats.GetStat(Characteristics.Damage));
            print(_findStats.GetStat(Characteristics.Health));
        }

        private void LevelUpOnOnExperienceGive()
        {
            _findStats.UpdateLevel(ref _currentLevel, _levelUp);
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
            _buffApplier.OnBonusAdd -= UpdateCharacteristics;
            _health.OnDie -= PlayDieAnimation;
            _health.OnHeavyAttackHit -= PlayHeavyAttackFalling;
            _levelUp.OnExperienceGive -= LevelUpOnOnExperienceGive;
            _statsValueStore.OnStatsChange -= StatsValueStoreOnOnStatsChange;
        }

        private void UpdateCharacteristics(Dictionary<CharacteristicBonus, float> buffs)
        {
            OnBuffsApply?.Invoke(buffs);
            
            _health.RenewHealthPoints(_findStats.GetStat(Characteristics.Health));
        }

        protected abstract void Init();

        public float GetStat(Characteristics characteristic)
        {
            return _findStats.GetStat(characteristic);
        }

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IBonus BonusTo(Characteristics characteristics)
            {
                return characteristics switch
                {
                    Characteristics.Damage => new DamageBonus(_statsValueStore.AddStat(Characteristics.Damage)),
                    Characteristics.Health => new HealthBonus(_statsValueStore.AddStat(Characteristics.Health)),
                    Characteristics.CriticalChance => new CriticalChanceBonus(_statsValueStore.AddStat(Characteristics.CriticalChance)),
                    Characteristics.CriticalDamage => new CriticalDamageBonus(_statsValueStore.AddStat(Characteristics.CriticalDamage)),
                    _ => throw new ArgumentOutOfRangeException(nameof(characteristics), characteristics, null)
                };
            }


            return characteristics.Select(BonusTo);
        }
    }
}