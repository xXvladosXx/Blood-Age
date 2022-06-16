using System;
using System.Collections.Generic;
using InventorySystem;
using SkillSystem.Skills;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Ability
{
    public abstract class SlotChoose : MonoBehaviour
    {
        [SerializeField] protected ItemContainer ItemContainer;
        [SerializeField] protected SlotHandler ItemPrefab;

        protected ItemContainer PlayerContainer;
        protected int Index = 0;
        protected Slot SlotToChange;
        protected SlotHandler SlotHandler;
        protected Dictionary<SlotHandler, Slot> SlotOnUI = new Dictionary<SlotHandler, Slot>();

        public event Action OnSlotChange;

        public abstract void SetData(ItemContainer actionItems, ItemContainer knownItems, Slot slotToChange,
            SlotHandler slotHandler);

        protected void OpenAbilityChoose(SlotHandler obj)
        {
            ItemContainer.SwapDragged(PlayerContainer, SlotToChange, SlotOnUI[obj]);
            SlotHandler.SetItem(ItemContainer.Database.GetItemByID(SlotToChange.ItemData.Id) as ActiveSkill);
            gameObject.SetActive(false);
            OnSlotChange?.Invoke();
        }

        protected void UpdateSlots()
        {
            foreach (var slot in SlotOnUI)
            {
                if (slot.Value.ItemData.Id >= 0)
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        ItemContainer.Database.GetItemByID(slot.Value.ItemData.Id).UIDisplay;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text =
                        slot.Value.Amount == 1 || slot.Value.Amount == 0 ? string.Empty : slot.Value.Amount.ToString();
                }
                else
                {
                    slot.Key.gameObject.SetActive(false);
                }
            }
        }
    }
}