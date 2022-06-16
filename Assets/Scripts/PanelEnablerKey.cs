using System;
using Entity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelEnablerKey : PanelEnabler
{
    [SerializeField] private Key _key;
    public event Action<PanelEnablerKey> OnUIChange;

    private bool _isAbleToEnable;
    private void Update()
    {
        if (!isActiveAndEnabled) return;
        
        if (Keyboard.current[_key].wasPressedThisFrame)
        {
            OnUIChange?.Invoke(this);
        }
    }

    public override void MakeInputPossible(bool canEnable)
    {
        base.MakeInputPossible(canEnable);
        _isAbleToEnable = canEnable;
    }

}