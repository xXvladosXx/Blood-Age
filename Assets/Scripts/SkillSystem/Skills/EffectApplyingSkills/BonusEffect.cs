using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StatsSystem;
using UnityEngine;

namespace SkillSystem.Skills.EffectApplyingSkills
{
    [CreateAssetMenu (menuName = "Skill/Effect/BonusToCharacteristic")]
    public class BonusEffect : EffectApplying, ICollectable
    {
        [SerializeField] private string _bonus;
        [SerializeField] private float _modifier;
        [SerializeField] private float _time;
        [SerializeField] public Buff[] _temporaryBuffs;
        
        private float _startValue;
        private LTDescr _leanTween;
        public override void Effect(SkillData skillData, Action cancel, Action finished)
        {
            skillData.GetUser.GetComponent<BuffApplier>().SetBuff(_temporaryBuffs);
        }

        public void AddData(Dictionary<string, StringBuilder> data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            if (_temporaryBuffs.Length > 1)
                stringBuilder.Append("Bonuses:").AppendLine();
            else 
                stringBuilder.Append("Bonus:").AppendLine();
            
            foreach (var buff in _temporaryBuffs)
            {
                switch (buff.GetCharacteristicBonus.Characteristics)
                {
                    case Characteristics.Damage:
                        stringBuilder.Append("Damage: ");
                        break;
                    case Characteristics.Accuracy:
                        stringBuilder.Append("Accuracy: ");
                        break;
                    case Characteristics.Evasion:
                        stringBuilder.Append("Evasion: ");
                        break;
                    case Characteristics.CriticalChance:
                        stringBuilder.Append("Critical chance: ");
                        break;
                    case Characteristics.CriticalDamage:
                        stringBuilder.Append("Critical damage: ");
                        break;
                    case Characteristics.Health:
                        stringBuilder.Append("Health: ");
                        break;
                    case Characteristics.HealthRegeneration:
                        stringBuilder.Append("Health regeneration: ");
                        break;
                }

                stringBuilder.Append(buff.GetCharacteristicBonus.Value).AppendLine();
                stringBuilder.Append("Time: ").Append(buff.GetLenghtOfEffect).AppendLine();
            }
            
            data.Add("Bonus", stringBuilder);
        }
    }
}