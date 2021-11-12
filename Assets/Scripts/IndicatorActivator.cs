using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class IndicatorActivator : MonoBehaviour
{
    private GameObject _effect;
    
    public void ActivateIndicator(GameObject effect)
    {
        _effect = Instantiate(effect, transform.position, Quaternion.identity, transform);
    }

    public void DeactivateIndicator()
    {
        foreach (Transform child in transform)
        {
            if(child.GetComponent<IDestroyable>() != null)
                child.GetComponent<IDestroyable>().Destroy();
        }
    }
}
