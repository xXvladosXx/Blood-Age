using System;
using InventorySystem;
using InventorySystem.Items;
using SkillSystem.Skills;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Ability
{
    public class SlotHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _background;
        private ActiveSkill _activeSkill;
        private bool _startCooldown;
        private float _time;

        public event Action<SlotHandler> OnAbilityClick;

        public void SetItem(ActiveSkill item)
        {
            _activeSkill = item;
        }

        public void StartCooldown(ActiveSkill item, float time)
        {
            if (_activeSkill == null) return;
            if (_activeSkill.Data.Id == item.Data.Id)
            {
                _startCooldown = true;
                _time = time;
            }
        }

        private void Update()
        {
            if (_startCooldown)
            {
                _background.fillAmount = _time / _activeSkill.GetCooldown;
            }
        }

        public void RefreshIcon(ActiveSkill activeSkill)
        {
            if (_activeSkill == null) return;
            if (_activeSkill.Data.Id == activeSkill.Data.Id)
            {
                _startCooldown = false;
                _background.fillAmount = 0;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnAbilityClick?.Invoke(this);
        }
    }
}