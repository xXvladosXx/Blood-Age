namespace InventorySystem
{
    using UnityEngine;
    
    [CreateAssetMenu (fileName = "StandardArmor")]
    public class StandardArmor : Item
    {
        [SerializeField] private float _armorValue;
    }
}