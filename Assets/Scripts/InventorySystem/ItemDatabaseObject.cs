namespace InventorySystem
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Inventory/ItemDatabase")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public Item[] Items;
        private readonly Dictionary<int, Item> _idItems = new Dictionary<int, Item>();

        [ContextMenu("Update ID's")]
        public void UpdateID()
        {
            _idItems.Clear();
            
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Data.Id != i)
                {
                    Items[i].Data.Id = i;
                }
                _idItems.Add(i, Items[i]);
            }
        }

        public Item GetItemByID(int id)
        {
            _idItems.TryGetValue(id, out var item);

            return item;
        }
        
        public void OnBeforeSerialize()
        {
            UpdateID();
        }

        public void OnAfterDeserialize()
        {
            UpdateID();
        }
    }
}