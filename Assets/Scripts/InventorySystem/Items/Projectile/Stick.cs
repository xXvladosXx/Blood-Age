using System.Text;
using UnityEngine;

namespace InventorySystem.Items.Projectile
{
    [CreateAssetMenu (menuName = "Weapon/Stick")]
    public class Stick : Projectile
    {
        public override string ItemInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<color=green>Use: ").Append(" Some text").Append("</color>").AppendLine();

            return stringBuilder.ToString();
        }
    }
}