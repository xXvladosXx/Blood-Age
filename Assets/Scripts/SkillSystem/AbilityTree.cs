using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using InventorySystem;
using SaveSystem;
using SkillSystem.Skills;
using UnityEngine;

namespace SkillSystem
{
    public class AbilityTree : MonoBehaviour, ISavable
    {
        [SerializeField] private ItemContainer _unknownAbility;
        [SerializeField] private ItemContainer _knownAbility;
        [SerializeField] private ItemContainer _actionAbility;

        public ItemContainer GetKnownAbilities => _knownAbility;
        public ItemContainer GetActionAbilities => _actionAbility;

        private List<AbilitySkill> _abilitySkills = new List<AbilitySkill>();

        public event Action OnStartRegenerateStamina;
        private void Awake()
        {
            foreach (var item in _actionAbility.GetAllItems())
            {
                _abilitySkills.Add(_actionAbility.FindNecessaryItemInData(item) as AbilitySkill);
            }
           
            _knownAbility.OnItemChange += ReplaceAbility;
            _actionAbility.OnItemChange += ReplaceAbility;
        }

        private void ReplaceAbility(Item arg1, Item arg2)
        {
            _abilitySkills.Clear();
            foreach (var item in _actionAbility.GetAllItems())
            {
                _abilitySkills.Add(_actionAbility.FindNecessaryItemInData(item) as AbilitySkill);
            }
        }

        public bool CastAbility(int index, AliveEntity aliveEntity)
        {
            if (_abilitySkills[index] != null)
            {
                _abilitySkills[index].ApplySkill(aliveEntity);
                return true;
            }

            return false;
        }

        public void CastAbility(AbilitySkill abilitySkill, AliveEntity aliveEntity)
        {
            abilitySkill.ApplySkill(aliveEntity);
        }

        public object CaptureState()
        {
            var items = _knownAbility.GetAllItems();
            return items;
        }

        public void RestoreState(object state)
        {
            var items = state as List<int>;
            _knownAbility.ClearInventory();
            foreach (var item in items)
            {
                var necessaryItem = _knownAbility.FindNecessaryItemInData(item);
                if (necessaryItem != null)
                {
                    _knownAbility.AddItem(necessaryItem.Data, 1);
                }
            }
        }
    }
}