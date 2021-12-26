namespace DefaultNamespace.StateMachine.PlayerStates
{
    using DefaultNamespace.Entity;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using global::SkillSystem;
    using UnityEngine;

    public class CastPlayerState : BaseMonoState
    {
        private SkillNode[] _skills;
        private Movement _movement;
        
        public override void StartState(AliveEntity aliveEntity)
        {
            _skills = aliveEntity.GetComponent<SkillBuilder>().GetSkillNodes;
            _stateSwitcher = aliveEntity.GetComponent<IStateSwitcher>();
            _movement = aliveEntity.GetComponent<Movement>();
            
            _movement.Cancel();
        }

        public void CastSkill(int index, AliveEntity aliveEntity)
        {
            _movement.Cancel();
            _skills[index].ApplySkill(aliveEntity);
        }

        public void SwitchToIdle()
        {
            _stateSwitcher.SwitchState<IdlePlayerState>();
        }
        
        public override void RunState(AliveEntity aliveEntity)
        {
            
        }
    }
}