namespace DefaultNamespace.ECS.Components.WeaponComponents
{
    using System;
    using global::System;
    using UnityEngine;

    [Serializable]
    public struct WeaponComponent
    {
        public float attackRange;
        public GameObject weaponPrefab;
    }
}