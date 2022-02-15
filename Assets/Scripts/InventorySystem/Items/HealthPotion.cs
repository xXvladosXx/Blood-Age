using Entity;
using UnityEngine;

namespace InventorySystem.Items
{
    [CreateAssetMenu (menuName = "Inventory/Potion/HealthPotion")]
    public class HealthPotion : Potion
    {
        [SerializeField] private float _healthPoints;
        public override string ItemInfo()
        {
            return "Health potion";
        }

        public override void UsePotion(AliveEntity aliveEntity)
        {
            aliveEntity.GetHealth.AddHealthPoints(_healthPoints);
        }

        
    }
}