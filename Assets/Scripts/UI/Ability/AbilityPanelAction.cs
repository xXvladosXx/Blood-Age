using Entity;
using SkillSystem;

namespace UI.Ability
{
    public class AbilityPanelAction : SlotPanelAction
    {
        public override void Initialize(AliveEntity aliveEntity)
        {
            var abilityTree = aliveEntity.GetComponent<AbilityTree>();
            SlotChoose = GetComponentInChildren<SlotChoose>();
            KnownItems = abilityTree.GetKnownAbilities;
            ActionItems = abilityTree.GetActionAbilities;

            SlotChoose.OnSlotChange += UpdateSlots;
            
            CreateSlots();
        }
    }
}