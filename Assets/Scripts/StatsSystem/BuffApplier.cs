using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using SaveSystem;
using StatsSystem.Bonuses;
using UnityEngine;

namespace StatsSystem
{
    public class BuffApplier : MonoBehaviour, IModifier
    {
        private Dictionary<CharacteristicBonus, float> _buffEffects = new Dictionary<CharacteristicBonus, float>();
        private Dictionary<CharacteristicBonus, float> _currentBuffEffects = new Dictionary<CharacteristicBonus, float>();
        
        public event Action<Dictionary<CharacteristicBonus, float>> OnBonusAdd;

        public void SetBuff(Buff[] buffs)
        {
            foreach (var buff in buffs)
            {
                if(_buffEffects.ContainsKey(buff.GetCharacteristicBonus)) continue;
                
                _buffEffects.Add(buff.GetCharacteristicBonus, buff.GetLenghtOfEffect);
            }

            _currentBuffEffects = new Dictionary<CharacteristicBonus, float>(_buffEffects);
            OnBonusAdd?.Invoke(_currentBuffEffects);
        }

        private void Update()
        {
            if(_buffEffects.Count == 0) return;
            foreach (var buffEffect in _buffEffects)
            {
                _currentBuffEffects[buffEffect.Key] -= Time.deltaTime;

                if (buffEffect.Value < 0)
                {
                    _currentBuffEffects.Remove(buffEffect.Key);
                    OnBonusAdd?.Invoke(_currentBuffEffects);
                }
            }

            _buffEffects = new Dictionary<CharacteristicBonus, float>(_currentBuffEffects);
        }

        public IEnumerable<IBonus> AddBonus(Characteristics[] characteristics)
        {
            IBonus CharacteristicToBonus(Characteristics c, float value)
                => c switch {
                    Characteristics.CriticalChance => new CriticalChanceBonus(value),
                    Characteristics.CriticalDamage => new CriticalDamageBonus(value),
                    Characteristics.Health => new HealthBonus(value),
                    Characteristics.Damage => new DamageBonus(value),
                    Characteristics.AttackSpeed => new AttackSpeedBonus(value),
                    Characteristics.MovementSpeed => new MovementSpeedBonus(value),
                    Characteristics.ManaRegeneration => new ManaRegenerationBonus(value),
                    Characteristics.HealthRegeneration => new HealthRegenerationBonus(value),
                    Characteristics.Mana => new ManaBonus(value),
                    Characteristics.Evasion => new EvasionBonus(value),
                    Characteristics.Accuracy => new AccuracyBonus(value),
                    _ => throw new IndexOutOfRangeException()
                };

            return _currentBuffEffects.Keys
                .Select(x => CharacteristicToBonus(x.Characteristics, x.Value));
        }
    }
}