using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LootSystem
{
    [CreateAssetMenu (menuName = "Inventory/Loot/OrdinaryLoot")]
    public class Loot : ScriptableObject
    {
        [SerializeField] private List<Drop> _loot = new List<Drop>();

        private List<Drop> _droppedLoot = new List<Drop>();
 
        [Serializable]
        public class Drop
        {
            public ItemPickUp InventoryItem;
            public int Amount;
            public int Probability;
        }

        public List<Drop> GetDrop()
        {
            _droppedLoot = new List<Drop>();
           

            foreach (var drop in _loot)
            {
                int roll = Random.Range(0, 100);
                roll -= drop.Probability;

                if (roll < 0)
                    _droppedLoot.Add(drop);
            }

            return _droppedLoot;
        }
        
    }
}