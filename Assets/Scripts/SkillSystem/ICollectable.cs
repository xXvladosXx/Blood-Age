using System.Text;

namespace SkillSystem
{
    using System.Collections.Generic;

    public interface ICollectable
    {
        void AddData(Dictionary<string, StringBuilder> data);
    }
}