using Entity;

namespace InventorySystem.Items
{
    public abstract class Potion : InventoryItem, IConsumable
    {
        protected abstract void UsePotion(AliveEntity aliveEntity);
        public void Consume(AliveEntity aliveEntity)
        {
            UsePotion(aliveEntity);
        }
    }
}