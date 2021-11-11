namespace SkillSystem
{
    using DefaultNamespace;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using SkillSystem.MainComponents;
    using UnityEngine;

    public class SkillBuilder : MonoBehaviour, IModifier
    {
        [SerializeField] private SkillNode[] _passiveSkill;

        public float AddBonus(Characteristics characteristics)
        {
            foreach (var skillNode in _passiveSkill)
            {
                if (skillNode is PassiveSkill passiveSkill)
                {
                    return passiveSkill.AddBonus(characteristics);
                }
            }

            return 0;
        }
    }
}