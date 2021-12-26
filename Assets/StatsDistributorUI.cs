using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Entity;
using StatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsDistributorUI : MonoBehaviour
{
    [SerializeField] private Stats _stat;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Button _minus;
    [SerializeField] private Button _plus;

    [SerializeField] private AliveEntity _aliveEntity;
    private StatsValueStore _statsValue;

    private void Awake()
    {
    }

    private void Start()
    {
        _statsValue = _aliveEntity.GetStatsValueStore;

        _plus.onClick.AddListener((() => {Allocate(1);}));
        _minus.onClick.AddListener((() => {Allocate(-1);}));
    }

    private void Update()
    {
        _minus.interactable = _statsValue.CanAssignPoints(_stat, -1);
        _plus.interactable = _statsValue.CanAssignPoints(_stat, 1);

        _valueText.text = _statsValue.GetProposedPoints(_stat).ToString();
    }

    private void Allocate(int points)
    {
        _statsValue.AssignPoints(_stat, points);
    }
}
