using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LootSystem
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private Loot _lootToDrop;
        
        public void SpawnLoot(Vector3 position)
        {
            foreach (var drop in _lootToDrop.GetDrop())
            {
                var lootObject = Instantiate(drop.InventoryItem, new Vector3(position.x, position.y+2, position.z), Quaternion.Euler(0,0,90), transform.parent);
                int amount = Random.Range(drop.MinAmount, drop.MaxAmount);
                lootObject.Amount = amount;
            }
        }
    }
}