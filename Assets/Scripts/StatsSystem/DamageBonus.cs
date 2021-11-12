namespace DefaultNamespace
{
    using System;

    [Serializable]
    public class DamageBonus : IBonus
    {
        public float Bonus;
        public DamageBonus(float bonus) => Value = bonus;

        public float Value { get; }
    }
}