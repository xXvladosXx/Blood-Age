using Entity;
using SaveSystem;
using SkillSystem.SkillNodes;
using SkillSystem.Skills;
using StatsSystem;
using StatsSystem.Bonuses;
using UI.Skill;

namespace SkillSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using InventorySystem;
    using UnityEngine;

    public class SkillTree : MonoBehaviour, IModifier, ISavable
    {
        [SerializeField] private List<SkillNode> _skillNodes;
        [SerializeField] private ItemContainer _unknownSkills;
        [SerializeField] private ItemContainer _knownSkills;
        [SerializeField] private ItemContainer _skillActionBar;
        [SerializeField] private int _pointsToUpgrade;

        private Dictionary<ActiveSkill, float> _skillsInCooldown = new Dictionary<ActiveSkill, float>();
        private Dictionary<ActiveSkill, float> _currentSkillsInCooldown = new Dictionary<ActiveSkill, float>();
        private ItemEquipper _itemEquipper;

        public List<SkillNode> _knownSkillsList = new List<SkillNode>();
        public List<SkillNode> _unknownSkillsList = new List<SkillNode>();
        private List<ActiveSkill> _activeSkills = new List<ActiveSkill>();

        public Dictionary<ActiveSkill, float> GetSkillsInCooldown => _skillsInCooldown;
        public ItemContainer GetActionSkills => _skillActionBar;
        public ItemContainer GetKnownSkills => _knownSkills;

        public int GetPoints => _pointsToUpgrade;
        public event Action OnSkillsChanged;
        
        private void Awake()
        {
            _knownSkillsList = _knownSkills.GetInventoryItems().OfType<SkillNode>().ToList();
            _unknownSkillsList = _unknownSkills.GetInventoryItems().OfType<SkillNode>().ToList();

            _itemEquipper = GetComponent<ItemEquipper>();

            ChangeActionSkills();

            _skillActionBar.OnInventoryChange += ChangeActionSkills;
            _knownSkills.OnInventoryChange += ChangeActionSkills;
        }

        private void ChangeActionSkills()
        {
            foreach (var skillNode in _skillActionBar.GetInventoryItems())
            {
                if (skillNode is ActiveSkill activeSkill)
                {
                    activeSkill.OnSkillCast += (aliveEntity) => StartCooldown(activeSkill);
                }
            }
            
            _activeSkills.Clear();
            foreach (var item in _skillActionBar.GetAllItems())
            {
                _activeSkills.Add(_skillActionBar.Database.GetItemByID(item) as ActiveSkill);
            }
        }
        

        private void Update()
        {
            if (_skillsInCooldown.Count == 0) return;

            foreach (var skillCooldown in _skillsInCooldown)
            {
                _currentSkillsInCooldown[skillCooldown.Key] -= Time.deltaTime;
                if (skillCooldown.Value < 0)
                {
                    _currentSkillsInCooldown.Remove(skillCooldown.Key);
                }
            }

            _skillsInCooldown = new Dictionary<ActiveSkill, float>(_currentSkillsInCooldown);
        }

        private void StartCooldown(ActiveSkill activeSkill)
        {
            if (_skillsInCooldown.ContainsKey(activeSkill)) return;

            _skillsInCooldown.Add(activeSkill, activeSkill.GetCooldown);
            _currentSkillsInCooldown = new Dictionary<ActiveSkill, float>(_skillsInCooldown);
        }

        public bool CanCastSkill(int index)
        {
            if (_activeSkills[index] == null) return false;
            if (_skillsInCooldown.ContainsKey(_activeSkills[index])) return false;
            if (!_activeSkills[index].GetWeaponTypeSkill.Contains(_itemEquipper.GetCurrentWeapon.GetWeaponType)) return false;
           
            return true;
        }
        public void CastSkill(int index, AliveEntity aliveEntity)
        {
            if (_activeSkills[index] != null)
            {
                _activeSkills[index].ApplySkill(aliveEntity);
            }
        }
        
        public void ComboSkillCast(ActiveSkill activeSkill, AliveEntity aliveEntity)
        {
            activeSkill.ApplySkill(aliveEntity);
        }

        public void UpgradeSkill()
        {
            _pointsToUpgrade--;
        }

        public void AddPoints()
        {
            _pointsToUpgrade++;
            OnSkillsChanged?.Invoke();
        }

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IEnumerable<CharacteristicBonus> AllMatchedPassiveSkillBonuses(PassiveSkill skill) =>
                skill._playerPassiveSkillBonus
                    .Where(x => characteristics.Contains(x.Characteristics));

            IBonus CharacteristicToBonus(Characteristics c, float value)
                => c switch
                {
                    Characteristics.CriticalChance => new CriticalChanceBonus(value),
                    Characteristics.CriticalDamage => new CriticalDamageBonus(value),
                    Characteristics.Health => new HealthBonus(value),
                    Characteristics.Damage => new DamageBonus(value),
                    Characteristics.AttackSpeed => new AttackSpeedBonus(value),
                    Characteristics.MovementSpeed => new MovementSpeedBonus(value),
                    Characteristics.ManaRegeneration => new ManaRegenerationBonus(value),
                    Characteristics.HealthRegeneration => new HealthRegenerationBonus(value),
                    Characteristics.Mana => new ManaBonus(value),
                    Characteristics.Evasion => new EvasionBonus(value),
                    Characteristics.Accuracy => new AccuracyBonus(value),
                    _ => throw new IndexOutOfRangeException()
                };

            return _skillNodes
                .OfType<PassiveSkill>()
                .SelectMany(AllMatchedPassiveSkillBonuses)
                .Select(x => CharacteristicToBonus(x.Characteristics, x.Value));
        }

        public bool HasSkill(SkillNode activeSkill)
        {
            return _knownSkillsList.Contains(activeSkill);
        }

        public bool MeetsTheConditionsOfRequiredSkills(SkillNode activeSkill)
        {
            return activeSkill.SkillRequirements().All(x => _knownSkillsList.Contains(x));
        }

        public bool CheckLevel(SkillNode activeSkill, SkillUpgradeData skillUpgradeData)
        {
            var level = skillUpgradeData.Entity.GetLevel;

            return level >= activeSkill.LevelRequirement();
        }

        public bool EnoughPoints()
        {
            if (_pointsToUpgrade <= 0) return false;

            return true;
        }

        public void UnlockSkill(SkillNode activeSkill, SkillUpgradeData skillUpgradeData)
        {
            _knownSkills.AddItem(new ItemData(activeSkill), 1);

            _knownSkillsList = _knownSkills.GetInventoryItems().OfType<SkillNode>().ToList();
            _unknownSkillsList = _unknownSkills.GetInventoryItems().OfType<SkillNode>().ToList();
            skillUpgradeData.RemovePoints();

            OnSkillsChanged?.Invoke();
        }

        public object CaptureState()
        {
            var skillSaver = new SkillSaver
            {
                UnknownSkills = new List<Slot>(),
                KnownSkills = new List<Slot>(),
                ActionsSkills = new List<Slot>()
            };

            foreach (var item in _unknownSkills.GetAllSlots())
            {
                var skillItem = _unknownSkills.Database.GetItemByID(item.ItemData.Id);
                if (skillItem != null)
                    skillSaver.UnknownSkills.Add(item);
            }

            foreach (var item in _knownSkills.GetAllSlots())
            {
                var skillItem = _unknownSkills.Database.GetItemByID(item.ItemData.Id);
                if (skillItem != null)
                    skillSaver.KnownSkills.Add(item);
            }

            foreach (var item in _skillActionBar.GetAllSlots())
            {
                var skillItem = _unknownSkills.Database.GetItemByID(item.ItemData.Id);
                if (skillItem != null)
                    skillSaver.ActionsSkills.Add(item);
            }

            skillSaver.Points = _pointsToUpgrade;

            return skillSaver;
        }

        public void RestoreState(object state)
        {
            var skillSaver = (SkillSaver) state;
            _skillActionBar.ClearInventory();
            _unknownSkills.ClearInventory();
            _knownSkills.ClearInventory();

            foreach (var skill in skillSaver.ActionsSkills)
            {
                _skillActionBar.AddItem(skill.ItemData, 1);
            }

            foreach (var skill in skillSaver.UnknownSkills)
            {
                _unknownSkills.AddItem(skill.ItemData, 1);
            }

            foreach (var skill in skillSaver.KnownSkills)
            {
                _knownSkills.AddItem(skill.ItemData, 1);
            }

            _pointsToUpgrade = skillSaver.Points;
            OnSkillsChanged?.Invoke();
        }

        [Serializable]
        public class SkillSaver
        {
            public List<Slot> UnknownSkills;
            public List<Slot> KnownSkills;
            public List<Slot> ActionsSkills;
            public int Points;
        }
    }
}