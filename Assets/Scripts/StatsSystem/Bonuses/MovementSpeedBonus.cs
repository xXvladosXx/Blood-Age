namespace DefaultNamespace
{
    public class MovementSpeedBonus : IBonus
    {
        public MovementSpeedBonus(float bonus) => Value = bonus;
        public float Value { get; }
    }
}