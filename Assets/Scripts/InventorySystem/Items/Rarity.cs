namespace InventorySystem.Items
{
    using UnityEngine;

    [CreateAssetMenu (menuName = "Item/Rarity")]
    public class Rarity : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private Color _color;
        
        public string Name => name;
        public Color TextColor => _color;
    }
}