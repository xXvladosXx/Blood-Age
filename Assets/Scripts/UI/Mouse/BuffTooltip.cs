namespace UI.Mouse
{
    using System;
    using System.Globalization;
    using DefaultNamespace;
    using TMPro;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class BuffTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _characteristics;
        [SerializeField] private TextMeshProUGUI _bonusValue;
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private RectTransform _background;
        [SerializeField] private Vector2 _offset;

        public static BuffTooltip Instance;
        
        private RectTransform _scale;

        private void Awake()
        {
            Instance = this;
            _scale = GetComponent<RectTransform>();

            HideBuffTooltip();
        }

        private void Update()
        {
            if(gameObject.activeSelf == false) return;
        
            Vector2 anchoredPosition = (Mouse.current.position.ReadValue()) / _canvas.transform.localScale.x;

            if (anchoredPosition.x + _background.rect.width + _offset.x > _canvas.rect.width)
            {
                anchoredPosition.x = _canvas.rect.width - _background.rect.width;
            }

            if (anchoredPosition.y + _background.rect.height + _offset.y > _canvas.rect.height)
            {
                anchoredPosition.y -= (2 * _offset.y) + _background.rect.height;
            }

            _scale.anchoredPosition = anchoredPosition + _offset;
        }

        public void HideBuffTooltip()
        {
            gameObject.SetActive(false);
        }

        public void ShowBuffTooltip(Characteristics characteristic, float bonusValue)
        {
            gameObject.SetActive(true);
            
            _characteristics.text = characteristic.ToString();
            _bonusValue.text = bonusValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}