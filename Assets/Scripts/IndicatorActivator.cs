using System.Collections;
using System.Collections.Generic;
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
            child.GetComponent<DestroyObject>().DestroyItself();
        }
    }
}
