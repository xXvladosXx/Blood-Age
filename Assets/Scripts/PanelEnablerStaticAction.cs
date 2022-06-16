using System;
using Entity;
using StateMachine;
using StateMachine.BaseStates;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PanelEnablerStaticAction : PanelEnabler
{
    public event Action<PanelEnabler> OnUIChange;

    protected override void Initialize()
    {
        foreach (var panel in Panels)
        {
            panel.OnPanelChange += PanelEnabled;
        }
    }
    
    private void PanelEnabled(Panel panel)
    {
        OnUIChange?.Invoke(this);
    }

    
}