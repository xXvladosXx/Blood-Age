namespace a
{
    using System;
    using DefaultNamespace.Entity.Class;
    using Entity.Class;

    [Serializable]
    public class Warrior : BaseCharacterClass
    {
        public Warrior()
        {
            Intelligence = 6;
            Strength = 23;
            Agility = 17;
        }
    }
}