using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AttackSystem;
using AttackSystem.AttackMechanics;
using DefaultNamespace;
using Entity.Classes;
using InventorySystem;
using InventorySystem.Items;
using SaveSystem;
using SkillSystem;
using StateMachine;
using StatsSystem;
using StatsSystem.Bonuses;
using UnityEngine;
using UnityEngine.AI;

namespace Entity
{
    [RequireComponent(typeof(BuffApplier))]
    [RequireComponent(typeof(FindStats))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(ItemEquipper))]
    public abstract class AliveEntity : MonoBehaviour, IModifier, IDamageable, IHealable
    {
        [SerializeField] private StatsSystem.Class _serializableClass = StatsSystem.Class.Warrior;
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private GameObject _bloodHit;
        [SerializeField] private CharacteristicModifierContainer _statsValueContainer;
        [SerializeField] private StatStartContainer _statStartContainer;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private float _staminaReg = .07f;
        [SerializeField] private string _uniqueIdentifier = System.Guid.NewGuid().ToString();
        [SerializeField] private GameObject _bloodDestroy;

        private LevelUp _levelUp;
        private StatsValueStore _statsValueStore;
        private ItemEquipper _itemEquipper;
        private BuffApplier _buffApplier;

        private Health _health;
        private Mana _mana;
        private Stamina _stamina;

        private float _healthReg;
        private float _manaReg;

        private Collider _collider;

        private AttackMaker _attackMaker;
        private AttackRegister _attackRegister;
        
        protected Movement Movement;
        protected FindStats FindStats;
        protected Outline Outline;
        
        private Animator _animator;
        private List<IRenewable> _renewables = new List<IRenewable>();
        private readonly List<ISavable> _savableComponent = new List<ISavable>();

        public List<AliveEntity> Targets = new List<AliveEntity>();

        public Health GetHealth => _health;
        public Mana GetMana => _mana;
        public Stamina GetStamina => _stamina;
        public ItemEquipper GetItemEquipper => _itemEquipper;
        public StatsValueStore GetStatsValueStore => _statsValueStore;
        public LevelUp GetLevelData => _levelUp;
        public int GetLevel => FindStats.GetLevel;
        public FindStats GetFindStats => FindStats;
        public List<ISavable> GetSavableComponents => _savableComponent;
        public AttackRegister GetAttackRegister => _attackRegister;

        public Class SerializableClass
        {
            get => _serializableClass;
            set => _serializableClass = value;
        }

        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");

        public Action<Dictionary<CharacteristicBonus, float>> OnBuffsApply;
        public event Action OnCharacteristicChange;
        public event Action<AliveEntity> OnDied;

        private void Awake()
        {
            FindStats = GetComponent<FindStats>();
            FindStats.SetClass(_serializableClass);

            _statsValueStore = new StatsValueStore(FindStats, _statsValueContainer, _statStartContainer);

            _health = new Health(FindStats);
            _stamina = new Stamina();
            _mana = new Mana(FindStats.GetStat(Characteristics.Mana));
            _levelUp = new LevelUp(FindStats);

            _attackRegister = new AttackRegister();

            _buffApplier = GetComponent<BuffApplier>();
            _itemEquipper = GetComponent<ItemEquipper>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
            
            Outline = GetComponent<Outline>();
            Movement = GetComponent<Movement>();

            _savableComponent.Add(_levelUp);
            _savableComponent.Add(_mana);
            _savableComponent.Add(_stamina);
            _savableComponent.Add(_health);
            _savableComponent.Add(_statsValueStore);
            
            _renewables.Add(_health);
            _renewables.Add(_mana);
            _renewables.Add(_levelUp);

            Init();

            Outline.enabled = false;
        }

        private void Start()
        {
            foreach (var renewable in _renewables)
            {
                renewable.Renew();
                renewable.OnStatRenewed += UpdateCharacteristics;
            }
        }

        protected abstract void Init();

        protected void Update()
        {
            if (_health.IsDead()) return;
            _mana.AddManaPoints(_manaReg);
            _health.AddHealthPoints(_healthReg);
        }

        protected virtual void OnEnable()
        {
            _buffApplier.OnBonusAdd += UpdateCharacteristics;

            FindStats.OnLevelUp += UpdateCharacteristics;

            _statsValueStore.OnStatsChange += UpdateCharacteristics;
            _itemEquipper.OnEquipmentChange += UpdateCharacteristics;

            _health.OnDie += DieActions;
            _health.OnTakeHit += TakeHit;
            _health.OnBloodyDie += BloodyDeath;

            _levelUp.OnExperienceGive += LevelUpOnOnExperienceGive;
        }

        protected virtual void OnDisable()
        {
            _buffApplier.OnBonusAdd -= UpdateCharacteristics;

            FindStats.OnLevelUp -= UpdateCharacteristics;

            _statsValueStore.OnStatsChange -= UpdateCharacteristics;
            _itemEquipper.OnEquipmentChange -= UpdateCharacteristics;

            _health.OnDie -= DieActions;
            _health.OnTakeHit -= TakeHit;
            _health.OnBloodyDie -= BloodyDeath;

            _levelUp.OnExperienceGive -= LevelUpOnOnExperienceGive;
        }

    
        protected virtual void DieActions()
        {
            _animator.Play(Dead);
            DisableOutline();

            OnDied?.Invoke(this);
        }

        private void BloodyDeath()
        {
            var blood = Instantiate(_bloodDestroy, transform.position, Quaternion.identity);
            Destroy(blood, 4f);

            LeanTween.delayedCall(.1f, () => { gameObject.SetActive(false); });
        }

        private void UpdateCharacteristics(Dictionary<CharacteristicBonus, float> buffs)
        {
            OnBuffsApply?.Invoke(buffs);
            
            _manaReg = FindStats.GetStat(Characteristics.ManaRegeneration);
            _healthReg = FindStats.GetStat(Characteristics.HealthRegeneration);
            
            _health.RenewHealthPoints(FindStats.GetStat(Characteristics.Health));
            _mana.RenewManaPoints(FindStats.GetStat(Characteristics.Mana));
        }

        private void UpdateCharacteristics()
        {
            GetCharacteristics();

            OnCharacteristicChange?.Invoke();
        }
        private void UpdateCharacteristics(InventoryItem inventoryItem)
        {
            GetCharacteristics();

            OnCharacteristicChange?.Invoke();
        }
        private void GetCharacteristics()
        {
            _health.RenewHealthPoints(FindStats.GetStat(Characteristics.Health));
            _mana.RenewManaPoints(FindStats.GetStat(Characteristics.Mana));

            _manaReg = FindStats.GetStat(Characteristics.ManaRegeneration);
            _healthReg = FindStats.GetStat(Characteristics.HealthRegeneration);
        }
        private void TakeHit(AliveEntity obj)
        {
            var blood = Instantiate(_bloodHit, transform.position, Quaternion.identity);
            Destroy(blood, .3f);
        }


        private void LevelUpOnOnExperienceGive()
        {
            FindStats.UpdateLevel(ref _currentLevel, _levelUp.GetCurrentExp);
        }


        public float GetStat(Characteristics characteristic)
        {
            return FindStats.GetStat(characteristic);
        }

        public virtual void ReloadEntity()
        {
            foreach (var renewable in _renewables)
            {
                renewable.Renew();
            }
        }

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IBonus BonusTo(Characteristics characteristics)
            {
                return characteristics switch
                {
                    Characteristics.Damage => new DamageBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.Damage)),
                    Characteristics.Health => new HealthBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.Health)),
                    Characteristics.CriticalChance => new CriticalChanceBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.CriticalChance)),
                    Characteristics.CriticalDamage => new CriticalDamageBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.CriticalDamage)),
                    Characteristics.DeathExperience => new DeathExperienceBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.DeathExperience)),
                    Characteristics.ManaRegeneration => new ManaRegenerationBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.ManaRegeneration)),
                    Characteristics.HealthRegeneration => new HealthRegenerationBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.HealthRegeneration)),
                    Characteristics.Mana => new ManaBonus(_statsValueStore.GetCalculatedStat(Characteristics.Mana)),
                    Characteristics.Evasion => new EvasionBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.Evasion)),
                    Characteristics.Accuracy => new AccuracyBonus(
                        _statsValueStore.GetCalculatedStat(Characteristics.Accuracy)),
                    Characteristics.ExperienceToLevelUp => new DeathExperienceBonus(0),

                    _ => throw new ArgumentOutOfRangeException(nameof(characteristics), characteristics, null)
                };
            }


            return characteristics.Select(BonusTo);
        }

        public void EnableOutLine()
        {
            Outline.enabled = true;
        }

        public void DisableOutline()
        {
            Outline.enabled = false;
        }


        public void FootL()
        {
        }

        public void FootR()
        {
        }

        public void Hit(int vampiric)
        {
            if (_attackMaker != null)
            {
                if (vampiric > 0)
                {
                    _attackMaker.MakeHit(_attackRegister.CalculateAttackData(FindStats, this, _itemEquipper, true));
                    return;
                }
            }

            _attackMaker = _itemEquipper.GetAttackMaker;
            _attackMaker.MakeHit(_attackRegister.CalculateAttackData(FindStats, this, _itemEquipper));
        }

        private void HitRadius(int vampiric)
        {
            if (_attackMaker != null)
            {
                if (vampiric > 0)
                {
                    _attackMaker.ActivateCollider(
                        _attackRegister.CalculateAttackData(FindStats, this, _itemEquipper, true));
                    return;
                }
            }

            _attackMaker = _itemEquipper.GetAttackMaker;
            _attackMaker.ActivateCollider(_attackRegister.CalculateAttackData(FindStats, this, _itemEquipper));
        }

        public void Shoot()
        {
            _attackMaker = _itemEquipper.GetAttackMaker;
            _attackMaker.MakeShoot(_attackRegister.CalculateAttackData(FindStats, this, _itemEquipper), _itemEquipper);
        }

        public void Damage(AttackData attackData)
        {
            _health.TakeHit(attackData);
        }

        public void Heal(float healValue)
        {
            _health.AddHealthPoints(healValue);
        }
    }
}