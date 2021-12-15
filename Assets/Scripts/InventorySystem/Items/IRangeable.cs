namespace AttackSystem.Weapon
{
    public interface IRangeable
    {
        ProjectileType GetProjectileType();
    }

    public enum ProjectileType
    {
        Arrow,
        Stick,
        Dagger
    }
}