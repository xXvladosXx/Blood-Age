using System.Collections.Generic;
using DefaultNamespace;
using Entity;
using StatsSystem;
using UnityEngine;

namespace UI.Skill
{
    public class BuffPanel : Panel
    {
        [SerializeField] private BuffDisplay _healthBuff;

        private Dictionary<BuffDisplay, Characteristics> _buffs;

        private void Awake()
        {
            _buffs = new Dictionary<BuffDisplay, Characteristics>();
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

        public override void Initialize(AliveEntity aliveEntity)
        {
            aliveEntity.OnBuffsApply += AddBuff;
        }
    }
}