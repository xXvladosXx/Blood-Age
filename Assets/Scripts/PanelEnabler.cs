using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entity;
using UI;
using UI.Inventory;
using UnityEngine;

public abstract class PanelEnabler : MonoBehaviour
{
    [SerializeField] protected List<Panel> Panels;
    
    private void Awake()
    {
        Panels = new List<Panel>();
        Panels = GetComponentsInChildren<Panel>().ToList();
        Initialize();
    }

    protected virtual void Initialize() { }

    public void ShowInterface()
    {
        foreach (var userInterface in Panels)
        {
            userInterface.gameObject.SetActive(true);
        }
        
        if(Panels.Count != 0) return;
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void HideInterface()
    {
        foreach (var userInterface in Panels)
        {
            userInterface.gameObject.SetActive(false);
        }
        
        if(Panels.Count != 0) return;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public virtual void MakeInputPossible(bool canEnable)
    {
    }
}
