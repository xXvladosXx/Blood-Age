using System;
using Entity;
using LootSystem;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
    public class LootBar : MonoBehaviour
    {
        [SerializeField] private float _positionOffset;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private Camera _camera;
        private ItemPickUp _itemPickUp;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void ShowBar()
        {
            transform.position =
                (_itemPickUp.transform.position + Vector3.up * _positionOffset);
            
            transform.LookAt(_camera.transform.position);
            transform.Rotate(0, 180, 0);
            
            gameObject.SetActive(true);
        }

        public void HideBar()
        {
            gameObject.SetActive(false);
        }
        
        public void SetItemInfo(ItemPickUp item, int amount)
        {
            _textMeshProUGUI.text = amount > 0 ? $"{item.GetInventoryItem.name} ({amount})" : item.GetInventoryItem.name;
            _textMeshProUGUI.color = item.GetInventoryItem.Rarity.GetColor;
            _itemPickUp = item;
        }
    }
}