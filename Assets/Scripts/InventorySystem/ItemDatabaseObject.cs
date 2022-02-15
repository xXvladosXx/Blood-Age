namespace InventorySystem
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Inventory/ItemDatabase")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public Item[] Items;
        
        [ContextMenu("Update ID's")]
        public void UpdateID()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Data.Id != i)
                    Items[i].Data.Id = i;
            }
        }

        public void OnBeforeSerialize()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Data.Id != i)
                    Items[i].Data.Id = i;
            }
        }

        public void OnAfterDeserialize()
        {
            UpdateID();
        }
    }
}