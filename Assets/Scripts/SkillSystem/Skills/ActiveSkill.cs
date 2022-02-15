using System;
using System.Collections.Generic;
using System.Linq;
using Entity;
using InventorySystem.Items.Weapon;
using RPGCharacterAnims;
using Runemark.Common;
using Sirenix.Utilities;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using SkillSystem.SkillNodes;
using StateMachine;
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
        private int _continuousEffects = 0;
        private int _currentEffects = 0;
        private Dictionary<string, float> _data = new Dictionary<string, float>();
        private IStateSwitcher _state;
        protected SkillData _skillData;
        private Animator _animator;
        private AliveEntity _user;
        public float GetCooldown => _cooldown;
        public Weapon[] GetWeaponTypeSkill => _weaponType;

        public event Action<AliveEntity> OnSkillCast;

        private static readonly int Canceled = Animator.StringToHash("Canceled");

        public Dictionary<string, float> GetData()
        {
            _data.Clear();

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
           
            _data.Add("Mana", _mana);
            _data.Add("Cooldown", _cooldown);
            return _data;
        }

        public override void ApplySkill(AliveEntity user)
        {
            if (_targeting == null) return;

            _user = user;
            if (!_user.GetMana.HasEnoughMana(_mana))
            {
                Cancel();
                return;
            }
            
            _state = _user.GetComponent<IStateSwitcher>();
            _animator = _user.GetComponent<Animator>();
            _animator.SetBool(Canceled, false);

            _continuousEffects = 0;
            _currentEffects = 0;
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
            _state.SwitchState<IdlePlayerState>();

            _currentEffects = 0;
        }

        private void AcquireTarget(SkillData skillData)
        {
            if (skillData.IsCancelled) return;

            foreach (var filtering in _filtering)
            {
                skillData.Targets = filtering.StartFiltering(skillData.Targets, skillData.GetUser.Targets);
            }

            foreach (var effectApplying in _effectApplying)
            {
                effectApplying.Effect(skillData, Cancel, Finished);

                if (effectApplying is ContinuousEffectApplying)
                {
                    _continuousEffects++;
                }
            }
        }

        private void Finished()
        {
            _currentEffects++;
            OnSkillCast?.Invoke(_skillData.GetUser);
            if (_continuousEffects == _currentEffects)
            {
                
            }
        }
    }
}