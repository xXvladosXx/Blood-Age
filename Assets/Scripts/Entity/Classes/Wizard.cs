namespace DefaultNamespace.Entity.Class
{
    using System;
    using global::Entity.Class;

    [Serializable]
    public class Wizard : BaseCharacterClass
    {
        public Wizard()
        {
            Intelligence = 23;
            Strength = 6;
            Agility = 17;
        }
    }
}