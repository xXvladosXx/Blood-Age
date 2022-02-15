using System;
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
            gameObject.SetActive(true);
        }

        public void HideBar()
        {
            gameObject.SetActive(false);
        }
        
        private void LateUpdate()
        {
            transform.position =
                (_itemPickUp.transform.position + Vector3.up * _positionOffset);
            transform.LookAt(_camera.transform);
            transform.Rotate(0, 180, 0);
        }

        public void SetItemInfo(ItemPickUp item, int amount)
        {
            _textMeshProUGUI.text = item.GetInventoryItem.name + amount;
            _textMeshProUGUI.color = item.GetInventoryItem.Rarity.GetColor;
            _itemPickUp = item;
        }
    }
}