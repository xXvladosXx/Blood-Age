using Entity;
using InventorySystem;
using MouseSystem;
using UnityEngine;

namespace LootSystem
{
    public class LootObject : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Loot _loot;
        public Loot GetLoot => _loot;
        public Loot Loot { get; set;}

        public CursorType GetCursorType() => CursorType.PickUp;
        public void ClickAction()
        {
            
        }

        public bool HandleRaycast(PlayerEntity player) => true;
    }
}