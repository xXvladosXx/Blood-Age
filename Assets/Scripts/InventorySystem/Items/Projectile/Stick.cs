namespace AttackSystem.Weapon
{
    using System.Text;
    using UnityEngine;

    [CreateAssetMenu (menuName = "Weapon/Stick")]
    public class Stick : Projectile
    {
        public override string ItemInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(_rarity.Name).AppendLine();
            stringBuilder.Append("<color=green>Use: ").Append(" Some text").Append("</color>").AppendLine();

            return stringBuilder.ToString();
        }
    }
}