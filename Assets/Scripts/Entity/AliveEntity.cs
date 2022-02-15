using System;
using System.Collections.Generic;
using System.Linq;
using AttackSystem;
using AttackSystem.AttackMechanics;
using DefaultNamespace;
using Entity.Classes;
using InventorySystem;
using LootSystem;
using SaveSystem;
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
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(NavMeshAgent))]

    public abstract class AliveEntity : MonoBehaviour, IModifier, IDestroyable, IHealable
    {
        [SerializeField] private StatsSystem.Class _serializableClass = StatsSystem.Class.Warrior;
        [SerializeField] private int _currentLevel = 1;
        [SerializeField] private string _ally = "Player";
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
        private FindStats _findStats;
        
        private Health _health;
        private Mana _mana;
        private Stamina _stamina;

        private float _healthReg;
        private float _manaReg;
        
        private BaseCharacterClass _characterClass;
        private Outline _outline;
        private LootSpawner _lootSpawner;
        
        private Movement _movement;
        private Collider _collider;
        
        private AttackMaker _attackMaker;
        private AttackRegister _attackRegister;

        protected List<Race.Race> EnemyRaces = new List<Race.Race>();
        protected List<ISavable> _savableComponent = new List<ISavable>();
        protected Race.Race Race;

        public List<AliveEntity> Targets = new List<AliveEntity>();
        
        public Health GetHealth => _health;
        public Mana GetMana => _mana;
        public Stamina GetStamina => _stamina;
        public ItemEquipper GetItemEquipper => _itemEquipper;
        public Race.Race GetRace => Race;
        public StatsValueStore GetStatsValueStore => _statsValueStore;
        public LevelUp GetLevelData => _levelUp;
        public string GetUniqueIdentifier => _uniqueIdentifier;

        public Class SerializableClass
        {
            get => _serializableClass;
            set => _serializableClass = value;
        }

        public int GetLevel => _currentLevel;

        public List<ISavable> GetSavableComponents => _savableComponent;

        private Animator _animator;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");

        public Action<Dictionary<CharacteristicBonus, float>> OnBuffsApply;
        public AttackRegister GetAttackRegister => _attackRegister;

        public event Action OnCharacteristicChange;
        public event Action<AliveEntity> OnDied;

        private void Awake()
        {
            _characterClass = _serializableClass switch
            {
                Class.Warrior => new Warrior(),
                Class.Archer => new Archer(),
                Class.Wizard => new Wizard(),
                _ => _characterClass
            };
            
            _findStats = GetComponent<FindStats>();
            _findStats.SetClass(_serializableClass);

            if (_statsValueContainer == null)
            {
                _statsValueStore = new StatsValueStore(_findStats, _characterClass, null, null);
            }
            
            _statsValueStore = new StatsValueStore(_findStats, _characterClass, _statsValueContainer, _statStartContainer);
            
            _health = new Health(_findStats);
            _stamina = new Stamina();
            _mana = new Mana(_findStats.GetStat(Characteristics.Mana));
            _levelUp = new LevelUp(_findStats);
            
            _attackRegister = new AttackRegister();

            _buffApplier = GetComponent<BuffApplier>();
            _itemEquipper = GetComponent<ItemEquipper>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
            _outline = GetComponent<Outline>();
            _movement = GetComponent<Movement>();
            _lootSpawner = GetComponent<LootSpawner>();

            if(_itemEquipper!= null)
                _itemEquipper.OnWeaponChanged += maker => _attackMaker = maker;
            
            _savableComponent.Add(_levelUp);
            _savableComponent.Add(_mana);
            _savableComponent.Add(_stamina);
            _savableComponent.Add(_health);
            _savableComponent.Add(_statsValueStore);
            
            _outline.enabled = false;
            
            Init();
        }

        protected virtual void Update()
        {
            if(_health.IsDead()) return;
            _mana.AddManaPoints(_manaReg);
            _health.AddHealthPoints(_healthReg);
            _stamina.AddStaminaPoints(_staminaReg);
        }

        private void OnEnable()
        {
            _buffApplier.OnBonusAdd += UpdateCharacteristics;
            _findStats.OnLevelUp += UpdateCharacteristics;
            _statsValueStore.OnStatsChange += UpdateCharacteristics;
            _itemEquipper.OnEquipmentChange += UpdateCharacteristics;
            
            _health.OnDie += DieActions;
            _health.OnTakeHit += TakeHit;
            _health.OnBloodyDie += BloodyDeath;

            _levelUp.OnExperienceGive += LevelUpOnOnExperienceGive;
        }

        private void TakeHit(AliveEntity obj)
        {
            var blood = Instantiate(_bloodHit, transform.position, Quaternion.identity);
            Destroy(blood, .3f);
        }


        private void LevelUpOnOnExperienceGive()
        {
            _findStats.UpdateLevel(ref _currentLevel, _levelUp);
        }

        private void DieActions()
        {
            _animator.Play(Dead);
            DisableOutline();
            
            if (_lootSpawner != null)
                _lootSpawner.SpawnLoot(transform.position);
            
            OnDied?.Invoke(this);
            _collider.enabled = false;
            
            LeanTween.delayedCall(3f, () =>
            {
                Destroy(_movement);
                Destroy(GetComponent<Rigidbody>());
                gameObject.SetActive(false);
            });
        }

        private void OnDisable()
        {
            _buffApplier.OnBonusAdd -= UpdateCharacteristics;
            _findStats.OnLevelUp -= UpdateCharacteristics;
            _statsValueStore.OnStatsChange -= UpdateCharacteristics;
            _itemEquipper.OnEquipmentChange -= UpdateCharacteristics;
            
            _health.OnDie -= DieActions;
            _health.OnTakeHit -= TakeHit;
            _health.OnBloodyDie -= BloodyDeath;
            
            _levelUp.OnExperienceGive -= LevelUpOnOnExperienceGive;
        }

        private void BloodyDeath()
        {
            var blood = Instantiate(_bloodDestroy, transform.position, Quaternion.identity);
            Destroy(blood, 4f);
            
            
            LeanTween.delayedCall(.1f, () =>
            {
                Destroy(_movement);
                Destroy(GetComponent<Rigidbody>());
                gameObject.SetActive(false);
            });
        }

        private void UpdateCharacteristics(Dictionary<CharacteristicBonus, float> buffs)
        {
            OnBuffsApply?.Invoke(buffs);
        }

        private void UpdateCharacteristics()
        {
            _health.RenewHealthPoints(_findStats.GetStat(Characteristics.Health));

            _manaReg = _findStats.GetStat(Characteristics.ManaRegeneration);
            _healthReg = _findStats.GetStat(Characteristics.HealthRegeneration);
            
            OnCharacteristicChange?.Invoke();
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
                    Characteristics.Damage => new DamageBonus(_statsValueStore.GetCalculatedStat(Characteristics.Damage)),
                    Characteristics.Health => new HealthBonus(_statsValueStore.GetCalculatedStat(Characteristics.Health)),
                    Characteristics.CriticalChance => new CriticalChanceBonus(_statsValueStore.GetCalculatedStat(Characteristics.CriticalChance)),
                    Characteristics.CriticalDamage => new CriticalDamageBonus(_statsValueStore.GetCalculatedStat(Characteristics.CriticalDamage)),
                    Characteristics.DeathExperience => new DeathExperienceBonus(_statsValueStore.GetCalculatedStat(Characteristics.DeathExperience)),
                    Characteristics.ManaRegeneration => new ManaRegenerationBonus(_statsValueStore.GetCalculatedStat(Characteristics.ManaRegeneration)),
                    Characteristics.HealthRegeneration => new HealthRegenerationBonus(_statsValueStore.GetCalculatedStat(Characteristics.HealthRegeneration)),
                    Characteristics.Mana => new ManaBonus(_statsValueStore.GetCalculatedStat(Characteristics.Mana)),
                    Characteristics.Evasion => new EvasionBonus(_statsValueStore.GetCalculatedStat(Characteristics.Evasion)),
                    Characteristics.Accuracy => new AccuracyBonus(_statsValueStore.GetCalculatedStat(Characteristics.Accuracy)),
                    Characteristics.ExperienceToLevelUp => new DeathExperienceBonus(0),

                    _ => throw new ArgumentOutOfRangeException(nameof(characteristics), characteristics, null)
                };
            }


            return characteristics.Select(BonusTo);
        }

        public void EnableOutLine()
        {
            _outline.enabled = true;
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
        }
        

        public void FootL()
        {
            
        }
        
        public void FootR()
        {
            
        }

        public void Hit()
        {
            _attackMaker.MakeHit(_attackRegister.CalculateAttackData(_findStats, this, _itemEquipper));
        }

        private void HitRadius()
        {
            _attackMaker.ActivateCollider(_attackRegister.CalculateAttackData(_findStats, this,_itemEquipper));
        }
        
        public void Shoot()
        {
            _attackMaker.MakeShoot(_attackRegister.CalculateAttackData(_findStats, this, _itemEquipper), _itemEquipper);
        }

        public void Destroy(AttackData attackData)
        {
            _health.TakeHit(attackData);
        }

        public void Heal(float healValue)
        {
            _health.AddHealthPoints(healValue);
        }
    }
}