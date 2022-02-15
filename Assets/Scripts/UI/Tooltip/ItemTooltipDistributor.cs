using Entity;
using InventorySystem.Items;
using InventorySystem.Items.Weapon;

namespace UI.Tooltip
{
    using InventorySystem;
    using UI.Inventory;
    using UnityEngine;

    public class ItemTooltipDistributor : Panel
    {
        public static ItemTooltipDistributor Instance;
        
        private AliveEntity _aliveEntity;
    
        
        public override void Initialize(AliveEntity aliveEntity)
        {
            Instance = this;
            _aliveEntity = aliveEntity;
        }
        public void ShowTooltip(Item item, UserInterface userInterface)
        {
            if(item == null) return;
            if (!(item is InventoryItem)) return;
            if (userInterface is DynamicInterface dynamicInterface)
            {
                UnequippedItemTooltip.Instance.ShowTooltip(item);

                foreach (var equippedItem in _aliveEntity.GetItemEquipper.GetEquippedItems)
                {
                    if (item is StandardWeapon && equippedItem is StandardWeapon)
                    {
                        EquippedItemTooltip.Instance.ShowTooltip(equippedItem);
                    }
                }
            }

            if (userInterface is StaticInterface staticInterface)
            {
                EquippedItemTooltip.Instance.ShowTooltip(item, staticInterface.GetItemContainer);
            }
        }

        public void HideTooltip(UserInterface userInterface)
        {
            if (userInterface is DynamicInterface dynamicInterface)
            {
                UnequippedItemTooltip.Instance.HideTooltip();
                EquippedItemTooltip.Instance.HideTooltip();
            }

            if (userInterface is StaticInterface staticInterface)
            {
                EquippedItemTooltip.Instance.HideTooltip();
            }
        }


    }
}