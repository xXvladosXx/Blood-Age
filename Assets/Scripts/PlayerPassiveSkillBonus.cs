namespace DefaultNamespace
{
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/PassivePlayer")]
    public class PlayerPassiveSkillBonus : ScriptableObject
    {
        public Characteristics Characteristics;
        public float Value;
    }
}