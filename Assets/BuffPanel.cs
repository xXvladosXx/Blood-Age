using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Entity;
using UnityEngine;

public class BuffPanel : MonoBehaviour
{
    [SerializeField] private AliveEntity _aliveEntity;
    [SerializeField] private BuffDisplay _healthBuff;

    private Dictionary<BuffDisplay, Characteristics> _buffs;

    private void Awake()
    {
        _buffs = new Dictionary<BuffDisplay, Characteristics>();
        _aliveEntity.OnBuffsApply += AddBuff;
    }

    private void AddBuff(Dictionary<CharacteristicBonus, float> buffs)
    {
        foreach (var buff in buffs)
        {
            if(_buffs.ContainsValue(buff.Key.Characteristics)) continue;            
            
            BuffDisplay buffDisplay = Instantiate(_healthBuff, transform);
            _buffs.Add(buffDisplay, buff.Key.Characteristics);
            buffDisplay.OnBuffDisplayDestroy += RemoveKey;
            buffDisplay.ChangeArrow(buff.Value, buff.Key);
        }
    }

    private void RemoveKey(BuffDisplay obj)
    {
        _buffs.Remove(obj);
    }
}