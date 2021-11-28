namespace DefaultNamespace
{
    using System;

    [Serializable]
    public class DamageBonus : IBonus
    {
        public DamageBonus(float bonus) => Value = bonus;

        public float Value { get; }
    }
}