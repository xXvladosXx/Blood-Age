using System;
using System.Collections;
using System.Collections.Generic;
using CharacterSelecting;
using StatsSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterSpawnManager : MonoBehaviour
{
    [SerializeField] private SelectedCharacterData selectedCharacterData;
    
    [SerializeField] private GameObject _archer;
    [SerializeField] private GameObject _warrior;
    [SerializeField] private GameObject _wizard;
    
    private void Awake()
    {
        switch (selectedCharacterData.Class)
        {
            case Class.Archer:
                Instantiate(_archer, transform.position, Quaternion.identity);
                break;
            case Class.Warrior:
                Instantiate(_warrior, transform.position, Quaternion.identity);
                break;
            case Class.Wizard:
                Instantiate(_wizard, transform.position, Quaternion.identity);
                break;
            case Class.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}
