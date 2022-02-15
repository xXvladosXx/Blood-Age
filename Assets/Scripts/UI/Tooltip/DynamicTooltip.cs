using UnityEngine.InputSystem.UI;

namespace UI.Tooltip
{
    using System.Collections.Generic;
    using System.Text;
    using InventorySystem;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public abstract class DynamicTooltip : Tooltip
    {
        [SerializeField] protected RectTransform _canvas;
        [SerializeField] protected Vector2 _offset;
        protected void Update()
        {
            Vector2 anchoredPosition = (Mouse.current.position.ReadValue());

            float pivotX = (anchoredPosition.x + _offset.x ) /Screen.width ;
            float pivotY = (anchoredPosition.y + _offset.y) / Screen.height ;

            _canvas.pivot = new Vector2(pivotX, pivotY);            
            _canvas.transform.position = anchoredPosition;
        }

    }
}