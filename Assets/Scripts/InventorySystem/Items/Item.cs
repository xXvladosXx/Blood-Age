namespace InventorySystem
{
    using System;
    using InventorySystem.Items;
    using UnityEngine;

    public abstract class Item : ScriptableObject
    {
        [SerializeField] protected Rarity _rarity;

        public string Description;
        public Sprite UIDisplay;
        public bool Stackable;
        public ItemCategory Category;
        public ItemData Data = new ItemData();

        public ItemCategory GetItemCategory => Category;
        
        public abstract string ItemInfo();
        
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