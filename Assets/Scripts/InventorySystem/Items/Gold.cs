using UnityEngine;

namespace InventorySystem.Items
{
    [CreateAssetMenu (menuName = "Inventory/Gold")]
    public class Gold : InventoryItem
    {
        public override string ItemInfo()
        {
            return "Gold";
        }
    }
}