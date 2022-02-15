using System;

namespace Entity.Classes
{
    [Serializable]
    public class Wizard : BaseCharacterClass
    {
        public Wizard()
        {
            Intelligence = 25;
            Strength = 10;
            Agility = 12;
        }
    }
}