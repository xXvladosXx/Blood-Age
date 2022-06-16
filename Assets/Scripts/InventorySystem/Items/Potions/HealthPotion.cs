using Entity;
using UnityEngine;

namespace InventorySystem.Items.Potions
{
    [CreateAssetMenu (menuName = "Inventory/Potion/HealthPotion")]
    public class HealthPotion : Potion
    {
        [SerializeField] private float _healthPoints;
        public override string ItemInfo()
        {
            return $"Health potion {_healthPoints}";
        }

        protected override void UsePotion(AliveEntity aliveEntity)
        {
            aliveEntity.GetHealth.AddHealthPoints(_healthPoints);
        }

        
    }
}