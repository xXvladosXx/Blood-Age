namespace DefaultNamespace
{
    public class HealthBonus : IBonus
    {
        public HealthBonus(float bonus) => Value = bonus;

        public float Value { get; }
    }
}