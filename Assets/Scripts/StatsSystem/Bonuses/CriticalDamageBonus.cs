namespace DefaultNamespace
{
    public class CriticalDamageBonus : IBonus
    {
        public CriticalDamageBonus(float bonus) => Value = bonus;
        public float Value { get; }
    }
}