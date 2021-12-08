namespace InventorySystem
{
    using UnityEngine;

    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private Inventory _inventory;
        public Inventory GetInventory => _inventory;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IEquipable itemToEquip)) return;

           _inventory.AddItem(new ItemData(other.GetComponent<ItemPickUp>().GetItem()), 
                other.GetComponent<ItemPickUp>().GetAmount);
        }
    }
}