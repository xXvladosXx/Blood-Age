namespace DefaultNamespace.MouseSystem
{
    using UnityEngine;

    public interface IRaycastable
    {
        bool HandleRaycast(Transform gameObject);
    }
}