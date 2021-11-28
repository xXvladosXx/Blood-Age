namespace InventorySystem
{
    using UnityEngine;

    public abstract class Item : ScriptableObject
    {
        [SerializeField] protected float _damageBonus = 0;
        [SerializeField] protected float _healthBonus = 0;
        [SerializeField] protected float _criticalDamage = 0;
        [SerializeField] protected float _criticalChance;
    }
}