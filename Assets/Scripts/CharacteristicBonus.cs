namespace DefaultNamespace
{
    using UnityEngine;

    [CreateAssetMenu (menuName = "Bonus/PassiveSkill")]
    public class CharacteristicBonus : ScriptableObject
    {
        public Characteristics Characteristics;
        public float Value;
    }
}