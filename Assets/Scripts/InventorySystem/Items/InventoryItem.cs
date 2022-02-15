namespace InventorySystem.Items
{
    public abstract class InventoryItem : Item
    {
        public int Price;
        public string Description;
        public Rarity Rarity;

        public abstract string ItemInfo();
    }
    
}