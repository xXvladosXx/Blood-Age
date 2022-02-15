using Entity;
using SaveSystem;
using SkillSystem.SkillNodes;
using SkillSystem.Skills;
using StatsSystem;
using StatsSystem.Bonuses;

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
        [SerializeField] private List<ActiveSkill> _activeSkills = new List<ActiveSkill>();
        [SerializeField] private ItemContainer _unknownSkills;
        [SerializeField] private ItemContainer _knownSkills;
        [SerializeField] private ItemContainer _skillActionBar;

        private Dictionary<ActiveSkill, float> _skillsInCooldown = new Dictionary<ActiveSkill, float>();
        private Dictionary<ActiveSkill, float> _currentSkillsInCooldown = new Dictionary<ActiveSkill, float>();
        private ItemEquipper _itemEquipper;

        public List<SkillNode> _knownSkillsList = new List<SkillNode>();
        public List<SkillNode> _unknownSkillsList = new List<SkillNode>();
        public Dictionary<ActiveSkill, float> GetSkillsInCooldown => _skillsInCooldown;
        public List<int> GetActionIds => _skillActionBar.GetAllItems();
        public ItemContainer GetActionSkills => _skillActionBar;
        public ItemContainer GetKnownSkills => _knownSkills;

        public event Action OnSkillsChanged;

        private void Awake()
        {
            _knownSkillsList = _knownSkills.GetInventoryItems().OfType<SkillNode>().ToList();
            _unknownSkillsList = _unknownSkills.GetInventoryItems().OfType<SkillNode>().ToList();

            _itemEquipper = GetComponent<ItemEquipper>();

            foreach (var skillNode in _skillActionBar.GetInventoryItems())
            {
                if (skillNode is ActiveSkill activeSkill)
                {
                    _activeSkills.Add(activeSkill);
                    activeSkill.OnSkillCast += (aliveEntity) => StartCooldown(activeSkill);
                }
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
            if (_skillsInCooldown.ContainsKey(activeSkill))
                _skillsInCooldown.Remove(activeSkill);

            _skillsInCooldown.Add(activeSkill, activeSkill.GetCooldown);
            _currentSkillsInCooldown = new Dictionary<ActiveSkill, float>(_skillsInCooldown);
        }

        public bool CanCastSkill(ActiveSkill activeSkill, AliveEntity aliveEntity)
        {
            if (_skillsInCooldown.ContainsKey(activeSkill)) return false;
            if (!activeSkill.GetWeaponTypeSkill.Contains(_itemEquipper.GetCurrentWeapon.GetWeaponType)) return false;
            activeSkill.ApplySkill(aliveEntity);
            return true;
        }

        public void ComboSkillCast(ActiveSkill activeSkill, AliveEntity aliveEntity)
        {
            activeSkill.ApplySkill(aliveEntity);
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

        public bool CheckDependencies(SkillNode activeSkill, SkillUpgradeData skillUpgradeData)
        {
            var hasSkillRequirements = activeSkill.SkillRequirements().All(x => _knownSkillsList.Contains(x));
            if (!hasSkillRequirements || _knownSkillsList.Contains(activeSkill)) return false;

            var levelRequirements = 0;
            levelRequirements = skillUpgradeData.Entity.GetLevel;

            return levelRequirements > activeSkill.LevelRequirement();
        }

        public void UnlockSkill(SkillNode activeSkill, SkillUpgradeData skillUpgradeData)
        {
            if (!CheckDependencies(activeSkill, skillUpgradeData)) return;

            _knownSkills.AddItem(new ItemData(activeSkill), 1);

            _knownSkillsList = _knownSkills.GetInventoryItems().OfType<SkillNode>().ToList();
            _unknownSkillsList = _unknownSkills.GetInventoryItems().OfType<SkillNode>().ToList();

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
                var skillItem = _unknownSkills.FindNecessaryItemInData(item.ItemData.Id);
                if (skillItem != null)
                    skillSaver.UnknownSkills.Add(item);
            }

            foreach (var item in _knownSkills.GetAllSlots())
            {
                var skillItem = _unknownSkills.FindNecessaryItemInData(item.ItemData.Id);
                if (skillItem != null)
                    skillSaver.KnownSkills.Add(item);
            }

            foreach (var item in _skillActionBar.GetAllSlots())
            {
                var skillItem = _unknownSkills.FindNecessaryItemInData(item.ItemData.Id);
                if (skillItem != null)
                    skillSaver.ActionsSkills.Add(item);
            }

            return skillSaver;
        }

        public void RestoreState(object state)
        {
            var skillSaves = (SkillSaver) state;
            _skillActionBar.ClearInventory();
            _unknownSkills.ClearInventory();
            _knownSkills.ClearInventory();

            foreach (var skill in skillSaves.ActionsSkills)
            {
                _skillActionBar.AddItem(skill.ItemData, 1);
            }

            foreach (var skill in skillSaves.UnknownSkills)
            {
                _unknownSkills.AddItem(skill.ItemData, 1);
            }

            foreach (var skill in skillSaves.KnownSkills)
            {
                _knownSkills.AddItem(skill.ItemData, 1);
            }
        }

        [Serializable]
        public class SkillSaver
        {
            public List<Slot> UnknownSkills;
            public List<Slot> KnownSkills;
            public List<Slot> ActionsSkills;
        }
    }
}