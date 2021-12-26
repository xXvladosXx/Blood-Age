namespace DefaultNamespace.Entity.Class
{
    using System;
    using global::Entity.Class;

    [Serializable]
    public class Archer : BaseCharacterClass
    {
        public Archer()
        {
            Strength = 6;
            Agility = 23;
            Intelligence = 17;
        }
    }
}