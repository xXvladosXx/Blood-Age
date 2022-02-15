namespace InventorySystem
{
    using System;
    using UnityEngine;

    public abstract class Item : ScriptableObject
    {
        public Sprite UIDisplay;
        public ItemData Data = new ItemData();
        public bool Stackable;
        public ItemCategory Category;

        [SerializeField] private bool StrictSwap = false;
        public bool GetStrictSwap => StrictSwap;
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
        public ItemData(Item inventoryItemObject)
        {
            Id = inventoryItemObject.Data.Id;
            Name = inventoryItemObject.name;
        }

        public ItemData(int id)
        {
            Id = id;
        }
    }
}