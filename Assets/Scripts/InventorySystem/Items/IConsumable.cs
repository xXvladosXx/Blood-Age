using Entity;

namespace InventorySystem.Items
{
    public interface IConsumable
    {
        void Consume(AliveEntity aliveEntity);
    }
}