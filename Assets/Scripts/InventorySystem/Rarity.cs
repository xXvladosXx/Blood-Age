using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu (menuName = "Inventory/Rarity")]
    public class Rarity : ScriptableObject
    {
        [SerializeField] private Color _color;
        public Color GetColor => _color;
    }
}