using InventorySystem.Items;

namespace InventorySystem
{
    public interface IEquipable
    {
        InventoryItem GetItem();
    }
}