/*namespace SkillSystem
{
    using System.Collections.Generic;
    using DefaultNamespace.SkillSystem.SkillNodes;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Skill/SkillDatabase")]
    public class SkillDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public SkillNode[] SkillNodes;
        private Dictionary<int, SkillNode> GetSkillNodes = new Dictionary<int, SkillNode>();

        public void OnBeforeSerialize()
        {
            GetSkillNodes = new Dictionary<int, SkillNode>();
        }

        public void OnAfterDeserialize()
        {
            for (var i = 0; i < SkillNodes.Length; i++)
            {
                SkillNodes[i].Data.Id = i;
                GetSkillNodes.Add(i, SkillNodes[i]);
            }
        }
    }
}*/