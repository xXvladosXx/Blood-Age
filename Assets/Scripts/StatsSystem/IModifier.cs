namespace DefaultNamespace
{
    using System.Collections.Generic;

    public interface IModifier
    {
        IEnumerable<IBonus> AddBonus(Characteristics[] characteristics);
    }
}