using Entity;
using UnityEngine;

namespace InventorySystem.Items.Potions
{
    [CreateAssetMenu (menuName = "Inventory/Potion/ManaPotion")]
    public class ManaPotion : Potion
    {
        [SerializeField] private float _manaPoints;
        public override string ItemInfo()
        {
            return $"Mana potion {_manaPoints}";
        }

        protected override void UsePotion(AliveEntity aliveEntity)
        {
            aliveEntity.GetMana.AddManaPoints(_manaPoints);
        }

    }
}