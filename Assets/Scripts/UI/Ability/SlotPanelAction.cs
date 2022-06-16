using System.Collections.Generic;
using Entity;
using InventorySystem;
using SkillSystem;
using SkillSystem.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Ability
{
    public abstract class SlotPanelAction : Panel
    {
        [SerializeField] private SlotHandler[] _slots;

        protected Dictionary<SlotHandler, Slot> SlotOnUI = new Dictionary<SlotHandler, Slot>();
        protected SlotChoose SlotChoose;
        protected ItemContainer KnownItems;
        protected ItemContainer ActionItems;

        private int _index;
        
        protected virtual void Update()
        {
            
        }

        protected void CreateSlots()
        {
            SlotOnUI = new Dictionary<SlotHandler, Slot>();
            _index = 0;

            foreach (var inventorySlot in ActionItems.Container.InventorySlots)
            {
                var slotHandler = _slots[_index];
                slotHandler.OnAbilityClick += OpenItemChoose;
                slotHandler.SetItem(ActionItems.Database.GetItemByID(inventorySlot.ItemData.Id) as ActiveSkill); 
                
                SlotOnUI.Add(slotHandler, inventorySlot);
                _index++;
            }
            
            UpdateSlots();
        }

        private void OpenItemChoose(SlotHandler o)
        {
            if (SlotChoose.gameObject.activeSelf)
            {
                SlotChoose.gameObject.SetActive(false);
            }
            else
            {
                SlotChoose.gameObject.SetActive(true);
                SlotChoose.SetData(ActionItems,KnownItems, SlotOnUI[o], o);
            }

        }
        
        protected void UpdateSlots()
        {
            foreach (var slot in SlotOnUI)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemData.Id >= 0 
                    ? ActionItems.Database.GetItemByID(slot.Value.ItemData.Id).UIDisplay : slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            }
        }
    }
}