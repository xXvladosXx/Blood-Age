using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Stats _stat;

    private void Awake()
    {
        _nameText.text = _stat.ToString();
    }

    public void SetStat(Stats stat, float value)
    {
        if (_stat == stat)
        {
            _valueText.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
