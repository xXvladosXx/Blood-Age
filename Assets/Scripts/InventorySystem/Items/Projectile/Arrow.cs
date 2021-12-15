namespace AttackSystem.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DefaultNamespace;
    using InventorySystem;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Weapon/Arrow")]
    public class Arrow : Projectile
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