using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using SkillSystem.MainComponents.Strategies;
using SkillSystem.SkillInfo;
using StateMachine;
using StateMachine.BaseStates;
using StateMachine.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SkillSystem.Skills.TargetingSkills
{
    [CreateAssetMenu(fileName = "Targeting", menuName = "Skill/Targeting/OnlyTargetApply", order = 0)]
    public class ImmediateTargetTargeting : Targeting, ICollectable
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _distance;

        private PlayerInputs _player;
        private IStateSwitcher _stateSwitcher;

        public override void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack)
        {
            _player = skillData.GetUser.GetComponent<PlayerInputs>();

            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out raycastHit, 1000, _layerMask))
            {
                raycastHit.collider.TryGetComponent(out AliveEntity aliveEntity);
                
                if (aliveEntity == null)
                {
                    skillData.Cancel();
                    canceledAttack();
                    return;
                }
                skillData.GetUser.GetAttackRegister.GetAttackData.PointTarget = aliveEntity;
                if (Vector3.Distance(aliveEntity.transform.position, skillData.GetUser.transform.position) > _distance)
                {
                    skillData.Cancel();
                    canceledAttack();
                    skillData.GetUser.GetComponent<IStateSwitcher>().SwitchState<AttackPlayerState>();
                    return;
                }
                
                skillData.Target = aliveEntity;
                skillData.GetUser.transform.LookAt(aliveEntity.transform);
            }

            finishedAttack();
        }

        public void AddData(Dictionary<string, StringBuilder> data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if(_distance != 0)
                stringBuilder.Append("Distance: ").Append(_distance).AppendLine();
            
            data.Add("Targeting: ", stringBuilder);
        }
    }
}

