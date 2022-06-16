using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ComboKeySpawner : MonoBehaviour
{
    public static ComboKeySpawner Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _keyStart;
    [SerializeField] private TextMeshProUGUI _keyEndCombo;
    
    private void Awake()
    {
        Instance = this;
    }

    public void SetChainKey()
    {
        _keyStart.text = "SPACE";
    }

    public void SetComboKey()
    {
        _keyEndCombo.text = "RCM";
    }
    public void ResetChainKey()
    {
        _keyStart.text = "";
    }

    public void ResetComboKey()
    {
        _keyEndCombo.text = "";
    }
}
