using System;
using System.Collections.Generic;
using Entity;
using LootSystem;
using StatsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Inventory
{
    public class LootBarController : MonoBehaviour
    {
        [SerializeField] private LootBar _lootBar;
        [SerializeField] private AliveEntity _player;
        [SerializeField] private float _visibility;

        private Dictionary<ItemPickUp, LootBar> _items = new Dictionary<ItemPickUp, LootBar>();

        private void Awake()
        {
            ItemPickUp.OnItemSpawn += AddLootBar;
            ItemPickUp.OnItemTake += RemoveLootBar;
        }

        private void Update()
        {
            if (Keyboard.current.altKey.isPressed)
            {
                foreach (var lootBar in _items.Values)
                {
                    if (Vector3.Distance(_player.transform.position, lootBar.transform.position) > _visibility)
                    {
                        lootBar.HideBar();
                        continue;
                    }
                    
                    if(lootBar.gameObject.activeSelf) continue;
                    lootBar.ShowBar();
                }
            }
            else
            {
                foreach (var lootBar in _items.Values)
                {
                    lootBar.HideBar();
                }
            }
        }

        private void OnDisable()
        {
            ItemPickUp.OnItemSpawn -= AddLootBar;
            ItemPickUp.OnItemTake -= RemoveLootBar;
        }

        private void RemoveLootBar(ItemPickUp item)
        {
            if(!_items.ContainsKey(item)) return;
            if(_items[item] != null)
            {Destroy(_items[item].gameObject);}
            _items.Remove(item);
        }

        private void AddLootBar(ItemPickUp item, Vector3 position, int amount)
        {
            if(_items.ContainsKey(item)) return;

            var lootBar = Instantiate(_lootBar, position, Quaternion.identity, transform);
            lootBar.HideBar();
            _items.Add(item, lootBar);
            lootBar.SetItemInfo(item, amount);
        }
    }
}