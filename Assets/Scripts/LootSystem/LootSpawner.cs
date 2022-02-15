using System;
using UnityEngine;

namespace LootSystem
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private Loot _lootToDrop;
        [SerializeField] private LootObject _lootPrefab;
        
        public void SpawnLoot(Vector3 position)
        {
            foreach (var drop in _lootToDrop.GetDrop())
            {
                var lootObject = Instantiate(drop.InventoryItem, position, Quaternion.Euler(0,0,90));
                lootObject.Amount = drop.Amount;
            }
        }
    }
}