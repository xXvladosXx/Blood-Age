namespace InventorySystem
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Inventory/ItemDatabase")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public Item[] Items;
        private Dictionary<int, Item> GetItem = new Dictionary<int, Item>();

        public void OnBeforeSerialize()
        {
            GetItem = new Dictionary<int, Item>();
        }

        public void OnAfterDeserialize()
        {
            for (var i = 0; i < Items.Length; i++)
            {
                Items[i].Data.Id = i;
                GetItem.Add(i, Items[i]);
            }
        }
    }
}