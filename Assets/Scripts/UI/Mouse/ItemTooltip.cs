using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace.UI.Stats;
using InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    [SerializeField] private RectTransform _canvas;
    [SerializeField] private RectTransform _background;
    [SerializeField] private ItemComparer _itemComparer;
    
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemSlotText;
    [SerializeField] private TextMeshProUGUI _itemStatsText;
    [SerializeField] private Vector2 _offset;

    private StringBuilder _stringBuilder;
    private RectTransform _scale;
    private List<ItemComparer> _itemComparers;

    private void Awake()
    {
        Instance = this;
        _scale = GetComponent<RectTransform>();
        _itemComparers = new List<ItemComparer>();
        
        HideTooltip();
        
        _stringBuilder = new StringBuilder();
    }

    private void Update()
    {
        if(gameObject.activeSelf == false) return;
        
        Vector2 anchoredPosition = (Mouse.current.position.ReadValue()) / _canvas.transform.localScale.x;

        if (anchoredPosition.x + _background.rect.width > _canvas.rect.width)
        {
            anchoredPosition.x = _canvas.rect.width - _background.rect.width;
            print("Left");
            
        }

        if (anchoredPosition.y + _background.rect.height > _canvas.rect.height)
        {
            anchoredPosition.y = _canvas.rect.height - _background.rect.height;
            anchoredPosition.y -= _background.rect.height;
        }

        _scale.anchoredPosition = anchoredPosition + _offset;

    }
    
    public void ShowTooltip(Item overlappedItem)
    {
        if(overlappedItem == null) return;
        _itemNameText.text = overlappedItem.name;
        _itemSlotText.text = overlappedItem.GetItemCategory.ToString();

        _stringBuilder.Length = 0;
        _stringBuilder.Append(overlappedItem.ItemInfo());

        _itemStatsText.text = _stringBuilder.ToString();

        gameObject.SetActive(true);
        Update();
    }

    public void HideTooltip()
    {
       gameObject.SetActive(false);
       
       foreach (var itemComparer in _itemComparers)
       {
           Destroy(itemComparer.gameObject);
       }
       
       _itemComparers.Clear();
    }
    
    public void AddInfo(List<Item> equippedItems)
    {
        foreach (var equippedItem in equippedItems)
        {
            ItemComparer itemComparer = Instantiate(_itemComparer, transform);
            
            itemComparer.SetStat(equippedItem.Data.Name, equippedItem.ItemInfo());
            _itemComparers.Add(itemComparer);
        }
    }
}
