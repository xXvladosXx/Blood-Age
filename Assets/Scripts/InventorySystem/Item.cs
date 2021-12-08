namespace InventorySystem
{
    using System;
    using UnityEngine;

    public abstract class Item : ScriptableObject
    {
        [SerializeField] protected float _damageBonus = 0;
        [SerializeField] protected float _healthBonus = 0;
        [SerializeField] protected float _criticalDamage = 0;
        [SerializeField] protected float _criticalChance;
        
        public Sprite UIDisplay;
        public bool Stackable;
        public ItemCategory Category;
        public ItemData Data = new ItemData();
        
        //public abstract string Description { get; }
    }
    
    [Serializable]
    public class ItemData
    {
        public string Name;
        public int Id = -1;

        public ItemData()
        {
            Name = "";
            Id = -1;
        }
        public ItemData(Item itemObject)
        {
            Id = itemObject.Data.Id;
            Name = itemObject.name;
        }
    }
}