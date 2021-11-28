namespace DefaultNamespace.ECS.Components.WeaponComponents
{
    using System;
    using global::System;
    using UnityEditor.Animations;
    using UnityEngine;

    [Serializable]
    public struct AnimatorComponent
    {
        public AnimatorController animator;
    }
}