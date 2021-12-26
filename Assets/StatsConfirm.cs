using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Entity;
using StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsConfirm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private AliveEntity _aliveEntity;
    
    private StatsValueStore _statsValue;
    public event Action OnStatsConfirmed;

    private void Start()
    {
        _statsValue = _aliveEntity.GetStatsValueStore;

        _confirmButton.onClick.AddListener(CharacteristicUpdated);
    }

    private void CharacteristicUpdated()
    {
        _statsValue.Confirm();
        OnStatsConfirmed?.Invoke();
    }


    private void Update()
    {
        _valueText.text = _statsValue.GetUnassignedPoints.ToString();
    }
}
