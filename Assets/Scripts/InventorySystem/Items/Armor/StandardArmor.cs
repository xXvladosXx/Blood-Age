namespace InventorySystem
{
    using System.Text;
    using UnityEngine;
    
    [CreateAssetMenu (fileName = "StandardArmor")]
    public class StandardArmor : Item
    {
        [SerializeField] private float _armorValue;
        public override string ItemInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(_rarity.Name).AppendLine();
            stringBuilder.Append("<color=green>Use: ").Append(" Some text").Append("</color>").AppendLine();

            return stringBuilder.ToString();
        }
    }
}