using Entity;
using InventorySystem;
using MouseSystem;
using UnityEngine;

namespace LootSystem
{
    public class LootObject : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Loot _loot;
        public CursorType GetCursorType() => CursorType.PickUp;

        public bool HandleRaycast(PlayerEntity player) => true;
    }
}