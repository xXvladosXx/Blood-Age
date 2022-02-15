using System;

namespace Entity.Classes
{
    [Serializable]
    public class Warrior : BaseCharacterClass
    {
        public Warrior()
        {
            Intelligence = 10;
            Strength = 25;
            Agility = 12;
        }
    }
}