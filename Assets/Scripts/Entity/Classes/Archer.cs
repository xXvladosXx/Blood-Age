using System;

namespace Entity.Classes
{
    [Serializable]
    public class Archer : BaseCharacterClass
    {
        public Archer()
        {
            Strength = 10;
            Agility = 25;
            Intelligence = 12;
        }
    }
}