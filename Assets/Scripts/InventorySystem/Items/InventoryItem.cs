namespace InventorySystem.Items
{
    public abstract class InventoryItem : Item
    {
        public int Price;
        public Rarity Rarity;

        public abstract string ItemInfo();
    }
    
}