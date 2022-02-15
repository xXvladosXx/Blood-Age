using System.Collections.Generic;
using DefaultNamespace;

namespace StatsSystem
{
    public interface IModifier
    {
        IEnumerable<IBonus> AddBonus(Characteristics[] characteristics);
    }
}