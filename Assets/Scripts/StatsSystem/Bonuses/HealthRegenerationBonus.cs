namespace DefaultNamespace
{
    public class HealthRegenerationBonus : IBonus
    {
        public HealthRegenerationBonus(float bonus) => Value = bonus;
        public float Value { get; }
    }
}