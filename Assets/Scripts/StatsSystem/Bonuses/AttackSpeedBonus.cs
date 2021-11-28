namespace DefaultNamespace
{
    public class AttackSpeedBonus : IBonus
    {
        public AttackSpeedBonus(float damage) => Value = damage;
        public float Value { get; }
    }
}