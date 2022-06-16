using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using InventorySystem.Items.Weapon;
using RPGCharacterAnims;
using Runemark.Common;
using Sirenix.Utilities;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using SkillSystem.SkillNodes;
using StateMachine;
using StateMachine.BaseStates;
using StateMachine.PlayerStates;
using StatsSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillSystem.Skills
{
    [CreateAssetMenu(menuName = "Skill/ActiveSkill", order = 0)]
    public class ActiveSkill : SkillNode
    {
        [SerializeField] private Targeting _targeting;
        [SerializeField] private Filtering[] _filtering;
        [SerializeField] private EffectApplying[] _effectApplying;
        [SerializeField] private float _cooldown;
        [SerializeField] private Weapon[] _weaponType;
        [SerializeField] private float _mana;

        private CharacteristicBonus[] _playerPassiveSkillBonus;
        private IStateSwitcher _state;
        protected SkillData _skillData;
        private Animator _animator;
        private AliveEntity _user;
        
        private readonly Dictionary<string, StringBuilder> _data = new Dictionary<string, StringBuilder>();
        
        public float GetCooldown => _cooldown;
        public Weapon[] GetWeaponTypeSkill => _weaponType;

        public event Action<AliveEntity> OnSkillCast;

        private static readonly int Canceled = Animator.StringToHash("Canceled");

        public Dictionary<string, StringBuilder> GetData()
        {
            _data.Clear();
            StringBuilder stringBuilder = new StringBuilder();
            
            if (_targeting is ICollectable collectable)
            {
                collectable.AddData(_data);
            }

            foreach (var effectApplying in _effectApplying)
            {
                if (effectApplying is ICollectable icollectable)
                {
                    icollectable.AddData(_data);
                }
            }

            stringBuilder.Append(_cooldown);
            _data.Add("Cooldown", stringBuilder);
            
            StringBuilder manaStringBuilder = new StringBuilder();
            manaStringBuilder.Append("Mana: ").Append(_mana);
            _data.Add("Mana", manaStringBuilder);
            
            StringBuilder requirementsStringBuilder = new StringBuilder();
            requirementsStringBuilder.Append("Requirements: ").AppendLine();
            foreach (var requiredSkill in Skills)
            {
                requirementsStringBuilder.Append("Skill: ").Append(requiredSkill.Data.Name);
            }
            requirementsStringBuilder.Append("Level: ").Append(RequiredLevel);
            _data.Add("Requirements", requirementsStringBuilder);

            return _data;
        }

        public override void ApplySkill(AliveEntity user)
        {
            if (_targeting == null) return;
            
            
            _user = user;
            
            _state = _user.GetComponent<IStateSwitcher>();
            _animator = _user.GetComponent<Animator>();
            _animator.SetBool(Canceled, false);
           
            _skillData = new SkillData(_user);
            
            _targeting.StartTargeting(_skillData, () => AcquireTarget(_skillData), Cancel);
        }

        protected void Cancel()
        {
            _animator.SetBool(Canceled, true);

            ExecuteState();
        }

        private void ExecuteState()
        {
            _state?.SwitchState<IdleBaseState>();
        }

        private void AcquireTarget(SkillData skillData)
        {
            if (!_user.GetMana.HasEnoughMana(_mana))
            {
                Cancel();
                return;
            }
            
            foreach (var filtering in _filtering)
            {
                skillData.Targets = filtering.StartFiltering(skillData.Targets, skillData.GetUser.Targets);
            }

            foreach (var effectApplying in _effectApplying)
            {
                effectApplying.Effect(skillData, Cancel, Finished);
            }
        }

        private void Finished()
        {
            OnSkillCast?.Invoke(_skillData.GetUser);
        }
    }
}