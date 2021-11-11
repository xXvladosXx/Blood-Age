namespace SkillSystem.MainComponents
{
    using DefaultNamespace;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skills/Passive/HealthBonus")]
    public class PassiveSkill : SkillNode
    {
        [SerializeField] private Characteristics _characteristic;
        [SerializeField] private float _bonus;
        
        public override void ApplySkill(ClassChooser classChooser)
        {
           
        }

        public float AddBonus(Characteristics characteristics)
        {
            if (characteristics != _characteristic) return 0;
            
            return _bonus;
        }
    }
}