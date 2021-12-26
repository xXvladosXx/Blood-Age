using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UI.Mouse;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _arrow;
    [SerializeField] private Image _foregroundImage;

    private float _time;
    private CharacteristicBonus _characteristicBonus;
    public event Action<BuffDisplay> OnBuffDisplayDestroy;

    private void Update()
    {
        _time -= Time.deltaTime;
        _foregroundImage.fillAmount -= 1.0f /_time*(Time.deltaTime/2);

        if (_time < 0)
        {
            OnBuffDisplayDestroy?.Invoke(this);
            Destroy(gameObject);
            BuffTooltip.Instance.HideBuffTooltip();
        }
    }

    public void ChangeArrow(float time, CharacteristicBonus characteristicBonus)
    {
        _time = time;
        _characteristicBonus = characteristicBonus;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BuffTooltip.Instance.ShowBuffTooltip(_characteristicBonus.Characteristics, _characteristicBonus.Value);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuffTooltip.Instance.HideBuffTooltip();
    }
}
